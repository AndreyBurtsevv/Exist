using Exist.ViewModels;
using System;
using System.Threading.Tasks;

namespace Exist.Services.Interfaces
{
    public interface IJwtAuthManager
    {
        Task<JwtAuthResult> GenerateTokens(string userName, DateTime now);

        Task<JwtAuthResult> Refresh(string refreshToken, string accessToken, DateTime now);

        Task<bool> DeleteRefresh(string accessToken, DateTime now);
    }
}
