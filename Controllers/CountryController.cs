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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string sort = "id", int page = 1)
        {
            var countries = await _countryService.GetAll();
            var sortedResult = countries.AsQueryable().Sort(sort);

            string url = Request.Scheme + "://" + Request.Host + "/" + Request.PathBase.Value + $"api/country?sort={sort}&";

            return Ok(PaginationService<Country>.Do(sortedResult, page, url));
        }

        // GET api/<CountryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _countryService.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<CountryController>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> PostAsync([FromBody] Country country)
        {
            try
            {
                return Ok(await _countryService.Create(country));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PATCH api/<CountryController>/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> PATCH(int id, [FromBody] Country country)
        {
            try
            {
                return Ok(await _countryService.Update(id, country));
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
                await _countryService.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
