using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Damda_Service.Data;
using Damda_Service.Models;
using Damda_Service.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace Damda_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private GroupService _groupService;
        private readonly DataContext _context;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(
            DataContext context,
            GroupService groupservice,
            ILogger<GroupsController> logger

            )
        {
            _groupService = groupservice;
            _context = context;
            _logger = logger;
        }

        // POST: Register
        [HttpPost("Register")]
        public async Task<ActionResult> Register(GroupRequest request)
        {
            try
            {
                return Ok(await _groupService.PostGroup(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }
        
        // PUT
        [HttpPut("{serial}")]
        public async Task<ActionResult> UpdateGroup(string serial, [FromBody]GroupRequest request)
        {
            try
            {
                return Ok(await _groupService.UpdateGroup(serial, request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        // POST: Register
        [HttpPost("AddUser")]
        public async Task<ActionResult> AddUser(GroupHasUsersRequest request)
        {
            try
            {
                return Ok(await _groupService.PostGroupHasUsers(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        // GET: api/Groups/5
        [HttpGet("{serial}")]
        public async Task<ActionResult> GetGroupInfo(string serial)
        {
            try
            {
                return Ok(await _groupService.GetGroupBySerial(serial));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

        // DELETE: api/Groups/5
        [HttpDelete("{serial}")]
        public async Task<ActionResult> DeleteGroup(string serial)
        {
            try
            {
                return Ok(await _groupService.DeleteGroup(serial));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex);
            }
        }

    }
}
