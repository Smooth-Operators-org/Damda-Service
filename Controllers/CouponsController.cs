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
    public class CouponsController : ControllerBase
    {
        private CouponService _couponService;
        private readonly ILogger<CouponsController> _logger;

        public CouponsController(
        CouponService couponService,
        ILogger<CouponsController> logger
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

        // GET: api/Coupon/
        [HttpGet()]
        public async Task<ActionResult> GetCoupons()
        {
            try
            {
                return Ok(await _couponService.GetAllCoupons());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

    }




}