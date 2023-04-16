using Infrastructure.Common;
using Infrastructure.DTOs.Department;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentReadDto>>> GetDepartments()
        {
            return Ok(await _departmentService.GetDepartmentsAsync());
        }

        // GET: api/Department/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentReadDto>> GetDepartmentById(int id)
        {
            try
            {
                return Ok(await _departmentService.GetDepartmentByIdAsync(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Department
        [HttpPost]
        public async Task<ActionResult<DepartmentReadDto>> CreateDepartment(DepartmentCreateDto departmentCreateDto)
        {
            var departmentReadDto = await _departmentService.CreateDepartmentAsync(departmentCreateDto);
            return CreatedAtAction(nameof(GetDepartmentById), new { id = departmentReadDto.Id }, departmentReadDto);
        }

        // PUT: api/Department/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, DepartmentUpdateDto departmentUpdateDto)
        {
            try
            {
                await _departmentService.UpdateDepartmentAsync(id, departmentUpdateDto);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Department/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                await _departmentService.DeleteDepartmentAsync(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        //GET /api/Department/by-company/{companyId}
        [HttpGet("by-company/{companyId}")]
        public async Task<IActionResult> GetDepartmentsByCompany(int companyId)
        {
            var departments = await _departmentService.GetDepartmentsByCompanyAsync(companyId);
            return Ok(departments);
        }

    }
}
