using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Exist.Models;

namespace Exist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole<int>> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/<CountryController>
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {               
                return Ok(await _roleManager.Roles.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // GET api/<CountryController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _roleManager.FindByIdAsync(id.ToString()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<CountryController>
        [HttpPost("{name}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Post(string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole<int>(name));
                    if (result.Succeeded)
                    {
                        return Ok(await _roleManager.FindByNameAsync(name));
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<CountryController>
        [HttpPost("role/{roleId}/user/{userId}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> AddRoleToUser(int roleId, int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception("Not found user with this id");
                }

                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role == null)
                {
                    throw new Exception("Not found role with this id");
                }

                await _userManager.AddToRoleAsync(user, role.Name);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PATCH
        [HttpPatch("id/{id}/name/{name}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Patch(int id, string name)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());

                if (role == null)
                {
                    throw new Exception("Not found role with this id");
                }

                role.Name = name;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return Ok(role);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<CountryController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                await _roleManager.DeleteAsync(role);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE
        [HttpDelete("role/{roleId}/user/{userId}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int roleId, int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    throw new Exception("Not found user with this id");
                }

                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role == null)
                {
                    throw new Exception("Not found role with this id");
                }

                var res = await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (res.Succeeded)
                {
                    return Ok();
                }

                return BadRequest(res.Errors);                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
