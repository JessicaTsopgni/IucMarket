using IucMarket.Dtos;
using IucMarket.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IucMarket.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleTypeController : ControllerBase
    {
        private const string Error = "An error occured. Please try again later.";
        private readonly CategoryService service;
        private readonly IWebHostEnvironment env;

        public ArticleTypeController(CategoryService service, IWebHostEnvironment env)
        {
            this.service = service;
            this.env = env;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 100)
        {
            try
            {
                return Ok
                (
                    await service.GetCategoriesAsync
                    (
                        pageIndex, 
                        pageSize
                    )
                );
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                return Ok
                (
                    await service.GetCategoryAsync(id)
                );
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryAddCommand command)
        {
            try
            {
                return Ok
                (
                    await service.AddAsync(command)
                );
            }
            catch(DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CategoryAddCommand command)
        {
            try
            {
                var oldCategory = await service.GetCategoryAsync(id);
                if (oldCategory == null)
                    throw new KeyNotFoundException($"Category {id} not found !");

                await service.EditAsync
                (
                    id,
                    command
                );
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var category = await service.GetCategoryAsync(id);
                await service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

    }
}
