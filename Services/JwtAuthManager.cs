using Exist.Models;
using Exist.Services.Interfaces;
using Exist.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Exist.Services
{
    public class JwtAuthManager : IJwtAuthManager
    {
        public IImmutableDictionary<string, RefreshTokenResult> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
        private readonly ConcurrentDictionary<string, RefreshTokenResult> _usersRefreshTokens;  // can store in a database or a distributed cache
        private readonly JwtConfig _jwtConfig;
        private readonly byte[] _secret;
        private readonly IServiceProvider _provider;

        public JwtAuthManager(JwtConfig jwtConfig, IServiceProvider provider)
        {
            _jwtConfig = jwtConfig;
            _provider = provider;
            _usersRefreshTokens = new ConcurrentDictionary<string, RefreshTokenResult>();
            _secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);
        }

        public async Task<JwtAuthResult> GenerateTokens(string userName, DateTime now)
        {
            User user = null;
            IList<string> roles = null;
            using (var scope = _provider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<UserManager<User>>();
                user = await service.FindByNameAsync(userName);
                roles = await service.GetRolesAsync(user);
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            AddRolesToClaims(claims, roles);

            var jwtToken = new JwtSecurityToken(
                expires: now.AddMinutes(_jwtConfig.TokenExp),
                claims: claims, 
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            var refreshToken = new RefreshTokenResult
            {
                RefreshToken = GenerateRefreshTokenString(),
                ExpireAt = now.AddMinutes(_jwtConfig.RefreshTokenExp)
            };

            _usersRefreshTokens.AddOrUpdate(userName, refreshToken, (s, t) => refreshToken);

            return new JwtAuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }

        public Task<bool> DeleteRefresh(string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userName = principal.Identity.Name;

            if (!_usersRefreshTokens.TryGetValue(userName, out var existingRefreshToken))
            {
                throw new SecurityTokenException("Invalid token");
            }

            if (existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return Task.FromResult(
                _usersRefreshTokens.Remove(userName, out _)
                );
        }

        public async Task<JwtAuthResult> Refresh(string refreshToken, string accessToken, DateTime now)
        {
            var (principal, jwtToken) = DecodeJwtToken(accessToken);

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var userName = principal.Identity.Name;

            if (!_usersRefreshTokens.TryGetValue(userName, out var existingRefreshToken))
            {
                throw new SecurityTokenException("Invalid token");
            }

            if (existingRefreshToken.ExpireAt < now)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return await GenerateTokens(userName, now);
        }

        private (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(_secret),
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.FromMinutes(_jwtConfig.TokenExp)
                    },
                    out var validatedToken);

            return (principal, validatedToken as JwtSecurityToken);
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
