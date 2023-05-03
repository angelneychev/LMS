namespace LMS.Server.Controllers
{
    using LMS.Server.Data;
    using LMS.Server.Models;
    using LMS.Shared.ViewModels.LeaveStatuses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LeaveStatusController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public LeaveStatusController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateLeaveStatus([FromBody] CreateEditLeaveStatusParameters parameters)
        {
            var leaveStatus = new LeaveStatus
            {
                Name = parameters.Name,
                CreatedOn = DateTime.UtcNow,
            };

            dbContext.LeaveStatuses.Add(leaveStatus);
            await dbContext.SaveChangesAsync();

            return Ok("Leave status created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateLeaveStatus(int id, [FromBody] CreateEditLeaveStatusParameters parameters)
        {
            var leaveStatus = dbContext.LeaveStatuses.FirstOrDefault(ls => ls.Id == id);

            if (leaveStatus == null)
            {
                return NotFound("Leave status not found.");
            }

            leaveStatus.Name = parameters.Name;
            leaveStatus.ModifiedOn = DateTime.UtcNow;

            dbContext.LeaveStatuses.Update(leaveStatus);
            await dbContext.SaveChangesAsync();

            return Ok("Leave status updated successfully.");
        }
    }
}
