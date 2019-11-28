using System;
using System.Threading.Tasks;
using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Damda_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;
        public AuthController(
            DataContext context,
            AuthService authService,
            ILogger<AuthController> logger
            )
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(AuthLogin authLogin)
        {
            var auth = await _authService.Login(authLogin);

            try
            {
                if (auth.GetType() == typeof(UserInfo))
                {
                    return Ok(auth);

                }
                else
                {
                    return BadRequest(new StatusResponse { message =  "User of Password Incorrect" });
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }
    }
}