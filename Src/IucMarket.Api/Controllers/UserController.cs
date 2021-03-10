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
    public class UserController : ControllerBase
    {
        private readonly UserService service;

        public UserController(UserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<UserList> Get(string pageToken = null)
        {
            return await service.GetUsersAsync(pageToken);
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
            catch(Exception)
            {
                return BadRequest("An error occured. Please try again later.");
            }
        }
    }
}
