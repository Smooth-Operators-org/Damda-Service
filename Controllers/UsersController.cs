using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Damda_Service.Data;
using Damda_Service.Models;
using Microsoft.Extensions.Logging;
using Damda_Service.Services;
using Microsoft.AspNetCore.Cors;

namespace Damda_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserService _userService;
        private readonly DataContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            DataContext context,
            UserService userService,
            ILogger<UsersController> logger
        )
        {
            _context = context;
            _userService = userService;
            _logger = logger;

        }

        // POST: Register
        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser(UserRequest request)
        {
            try
            {
                return Ok(await _userService.PostUser(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        // GET: api/Users/5
        [HttpGet("{serial}")]
        public async Task<ActionResult> GetUserInfo(string serial)
        {
            try
            {
                return Ok(await _userService.GetUserBySerial(serial));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{serial}")]
        public async Task<IActionResult> UpdateUser(string serial, [FromBody]UserInfo userinfo)
        {
            try
            {
                return Ok(await _userService.UpdateUser(serial, userinfo));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{serial}")]
        public async Task<ActionResult<User>> DeleteUser(string serial)
        {
            try
            {
                return Ok(await _userService.DeleteUser(serial));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }
    }
}
