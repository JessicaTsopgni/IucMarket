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
    public class AccountController : ControllerBase
    {
        private const string Error = "An error occured. Please try again later.";
        private readonly UserService service;

        public AccountController(UserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index(string pageToken = null, int pageSize = 50)
        {
            try
            {
                return Ok
                (
                    await service.GetUsersAsync(pageToken, pageSize)
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
                    await service.GetUserAsync(id)
                );
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            try
            {
                return Ok
                (
                    await service.RegisterAsync
                    (
                        new RegisterCommand
                        (
                            user.Email,
                            user.Password,
                            user.FullName,
                            user.PhoneCountryCode,
                            user.PhoneNumber,
                            false,
                            user.Role,
                            user.Status
                        )
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
        public async Task<IActionResult> Put(string id, [FromBody] User user)
        {
            try
            {
                return Ok
                (
                    await service.EditAsync
                    (
                        id,
                        new RegisterCommand
                        (
                            user.Email,
                            user.Password,
                            user.FullName,
                            user.PhoneCountryCode,
                            user.PhoneNumber,
                            false,
                            user.Role,
                            user.Status
                        )
                    )
                );
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

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                return Ok(await service.LoginAsync(new LoginCommand(email, password)));
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.ToString());
                return BadRequest(Error);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Owners()
        {
            try
            {
                return Ok(await service.GetOwnersAsync());
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
