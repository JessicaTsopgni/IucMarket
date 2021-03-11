using IucMarket.Entities;
using IucMarket.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private const string Error = "An error occured. Please try again later.";
        private readonly ProductService service;

        public ArticleController(ProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 100)
        {
            try
            {
                return Ok
                (
                    await service.GetProductsAsync(pageIndex, pageSize)
                );
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
                    await service.GetProductAsync(id)
                );
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            try
            {
                return Ok
                (
                    await service.AddAsync
                    (
                        product
                    )
                );
            }
            catch(DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Product product)
        {
            try
            {
                await service.EditAsync
                (
                    id,
                    product
                );
                return Ok();
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
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
                await service.DeleteAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }
    }
}
