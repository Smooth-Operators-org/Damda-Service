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
    [EnableCors("AllowMyOrigin")]
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

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroup()
        {
            return await _context.Group.ToListAsync();
        }

        /*
        // GET: api/Groups/5
        [HttpGet("{serial}")]
        public async Task<ActionResult<User>> GetGroupInfo(string serial)
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
        */

        // PUT: api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            if (id != @group.GroupId)
            {
                return BadRequest();
            }

            _context.Entry(@group).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int id)
        {
            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }

            _context.Group.Remove(@group);
            await _context.SaveChangesAsync();

            return @group;
        }

        private bool GroupExists(int id)
        {
            return _context.Group.Any(e => e.GroupId == id);
        }
    }
}
