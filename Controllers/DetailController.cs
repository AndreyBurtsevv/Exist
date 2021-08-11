using Exist.Models;
using Exist.Services;
using Exist.Services.Interfaces;
using Exist.Sorting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailController : ControllerBase
    {
        private readonly IDetailSercvice _detaiService;

        public DetailController(IDetailSercvice detaiService)
        {
            _detaiService = detaiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string sort = "id", int page = 1)
        {
            var details = await _detaiService.GetAll();
            var sortedResult = details.AsQueryable().Sort(sort);

            string url = Request.Scheme + "://" + Request.Host + "/" + Request.PathBase.Value + $"api/detail?sort={sort}&";

            return Ok(PaginationService<Detail>.Do(sortedResult, page, url));
        }

        // GET api/<DetailController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _detaiService.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST api/<DetailController>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Post([FromBody] Detail detail)
        {
            try
            {
                return Ok(await _detaiService.Create(detail));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // PATCH api/<DetailController>/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Patch(int id, [FromBody] Detail detail)
        {
            try
            {
                return Ok(await _detaiService.Update(id, detail));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // DELETE api/<DetailController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _detaiService.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
