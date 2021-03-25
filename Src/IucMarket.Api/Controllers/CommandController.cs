using IucMarket.Dtos;
using IucMarket.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IucMarket.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private const string Error = "An error occured. Please try again later.";
        private readonly OrderService service;
        private readonly IWebHostEnvironment env;

        public CommandController(OrderService service, IWebHostEnvironment env)
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
                    await service.GetOrdersAsync
                    (
                        ArticleController.GetPathTemplate(Request),
                        pageIndex, 
                        pageSize
                    )
                );
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
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
                    await service.GetOrderAsync(id, ArticleController.GetPathTemplate(Request))
                );
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderAddCommand command)
        {
            try
            {
                return Ok
                (
                    await service.AddAsync(command, ArticleController.GetPathTemplate(Request))
                );
            }
            catch(DuplicateWaitObjectException ex)
            {
                return Conflict(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] OrderEditCommand command)
        {
            try
            {               
                await service.EditAsync
                (
                    id,
                    command,
                    ArticleController.GetPathTemplate(Request)
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
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
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
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
                //return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

    }
}
