namespace LMS.Server.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using LMS.Server.Data;
    using LMS.Server.Models;
    using LMS.Shared.ViewModels.LeaveTypes;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public LeaveTypeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateLeaveType([FromBody] CreateEditLeaveTypeParameters parameters)
        {
            var leaveType = new LeaveType
            {
                Name = parameters.Name,
                CreatedOn = DateTime.UtcNow,
            };

            dbContext.LeaveTypes.Add(leaveType);
            await dbContext.SaveChangesAsync();

            return Ok("Leave type created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateLeaveType(int id, [FromBody] CreateEditLeaveTypeParameters parameters)
        {
            var leaveType = dbContext.LeaveTypes.FirstOrDefault(lt => lt.Id == id);

            if (leaveType == null)
            {
                return NotFound("Leave type not found.");
            }

            leaveType.Name = parameters.Name;
            leaveType.ModifiedOn = DateTime.UtcNow;

            dbContext.LeaveTypes.Update(leaveType);
            await dbContext.SaveChangesAsync();

            return Ok("Leave type updated successfully.");
        }
    }
}
