using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Damda_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private CouponService _couponService;
        private readonly ILogger<CouponController> _logger;

        public CouponController(
        CouponService couponService,
        ILogger<CouponController> logger
        )
        {
            _couponService = couponService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser(CouponRequest request)
        {
            try
            {
                return Ok(await _couponService.PostCoupon(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

    }




}