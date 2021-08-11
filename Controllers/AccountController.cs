using AutoMapper;
using Exist.Models;
using Exist.Services.Interfaces;
using Exist.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
                                    IMapper mapper, IJwtAuthManager jwtAuthManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthManager = jwtAuthManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Registration registrator)
        {
            if (ModelState.IsValid)
            {
                User user = new() { UserName = registrator.UserName };

                if (registrator.Email != null)
                {
                    user.Email = registrator.Email;
                }

                IdentityResult response = await _userManager.CreateAsync(user, registrator.Password);

                if (response.Succeeded)
                {
                    var result = _mapper.Map<User, UserView>(user);

                    return Ok(result);
                }
                else
                {
                    return BadRequest(response.Errors.ToList()[0].Code);
                }
            }
            return BadRequest("User data is not correct");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginView login)
        {
            if (ModelState.IsValid)
            {
                var response = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, true, false);

                if (response.Succeeded)
                {
                    var user = _userManager.Users.First(x => x.UserName == login.UserName);

                    UserView userview = _mapper.Map<User, UserView>(user);

                    var jwt = await _jwtAuthManager.GenerateTokens(user.UserName, DateTime.UtcNow);

                    return Ok(new { userview, jwt });
                }
                else
                {
                    throw new Exception("В авторизации отказано");
                }
            }
            return BadRequest("User data is not correct");
        }

        [HttpPost]
        [Route("logout")]
        public async Task Logout([FromBody] RefreshToken request)
        {
            await _jwtAuthManager.DeleteRefresh(request.AccessToken, DateTime.Now);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserView updateUser, string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);

                UpdateUserAsync(updateUser, user);

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var userView = _mapper.Map<User, UserView>(user);
                    return Ok(userView);
                }
                else
                {
                    throw new Exception("Отказ в обновлении ");
                }
            }
            return BadRequest("User data is not correct");
        }

        private static void UpdateUserAsync(UpdateUserView updateUser, User user)
        {
            if (!string.IsNullOrEmpty(updateUser.Password))
            {
                PasswordHasher<User> passwordHasher = new();
                user.PasswordHash = passwordHasher.HashPassword(user, updateUser.Password);
            }

            if (!string.IsNullOrEmpty(updateUser.Email))
            {
                user.Email = updateUser.Email;
            }

            if (!string.IsNullOrEmpty(updateUser.UserName))
            {
                user.UserName = updateUser.UserName;
            }

            if (!string.IsNullOrEmpty(updateUser.PhoneNumber))
            {
                user.PhoneNumber = updateUser.PhoneNumber;
            }
        }

        [HttpGet]
        [Route("all-users")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = _userManager.Users.ToList();

                return Ok(_mapper.Map<List<User>, List<UserView>>(users));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                return Ok(_mapper.Map<User, UserView>(user));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken request)
        {
            return await Task.Factory.StartNew<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.NewToken))
                    {
                        return Unauthorized();
                    }

                    var jwtResult = _jwtAuthManager.Refresh(request.NewToken, request.AccessToken, DateTime.UtcNow);

                    return Ok(jwtResult);
                }
                catch (SecurityTokenException e)
                {
                    return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
                }
            });
        }
    }
}
