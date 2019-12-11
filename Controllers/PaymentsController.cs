using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Damda_Service.Models;
using Damda_Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Damda_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private PaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            PaymentService paymentService,
            ILogger<PaymentsController> logger
        )
        {
            _paymentService = paymentService;
            _logger = logger;

        }

        // GET: api/payments/getlist/{serial}/{serial}
        [HttpGet("getlist/{userSerial}/{groupSerial}")]
        public async Task<ActionResult> GetUserList(string userSerial, string groupSerial)
        {
            try
            {
                return Ok(await _paymentService.GetUserPaymentList(userSerial, groupSerial));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterUser(PaymentRequest request)
        {
            try
            {
                return Ok(await _paymentService.PostPayment(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

    }
}