using Exist.Models;
using Exist.Services;
using Exist.Services.Interfaces;
using Exist.Sorting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string sort = "id", int page = 1)
        {
            var companies = await _companyService.GetAll();
            var sortedResult = companies.AsQueryable().Sort(sort);

            string url = Request.Scheme + "://" + Request.Host + "/" + Request.PathBase.Value + $"api/company?sort={sort}&";

            return Ok(PaginationService<Company>.Do(sortedResult, page, url));
        }

        // GET api/<CompanyController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _companyService.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<CompanyController>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Post([FromBody] Company company)
        {
            try
            {
                return Ok(await _companyService.Create(company));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message + ex.InnerException);
            }
        }

        // PATCH api/<CompanyController>/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Put(int id, [FromBody] Company company)
        {
            try
            {
                return Ok(await _companyService.Update(id, company));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PUT 
        [HttpPut("company/{companyId}/group/{groupId}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Put(int companyId, int groupId)
        {
            try
            {
                return Ok(await _companyService.AddGroup(companyId, groupId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<CompanyController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _companyService.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE
        [HttpDelete("company/{companyId}/group/{groupId}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int companyId, int groupId)
        {
            try
            {
                await _companyService.DeleteGroup(companyId, groupId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
