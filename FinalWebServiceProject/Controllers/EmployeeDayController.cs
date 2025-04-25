using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProject.Domain;
using NetProject.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalWebServiceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDayController : ControllerBase
    {
        private readonly ProjectContext db;

        public EmployeeDayController(ProjectContext context)
        {
            db = context;
        }

        // GET: api/EmployeeDay
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDay>>> GetEmployeeDays()
        {
            return await db.EmployeeDays.ToListAsync();
        }

        // GET: api/EmployeeDay/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDay>> GetEmployeeDay(int id)
        {
            var employeeDay = await db.EmployeeDays.FindAsync(id);

            if (employeeDay == null)
            {
                return NotFound();
            }

            return employeeDay;
        }

        // POST: api/EmployeeDay
        [HttpPost]
        public async Task<ActionResult<EmployeeDay>> PostEmployeeDay([FromBody] EmployeeDayDto dto)
        {
            var day = db.Days.FirstOrDefault(d => d.Date == DateOnly.Parse(dto.Date));
            if (day == null)
            {
                db.Days.Add(new Day() { Date = DateOnly.Parse(dto.Date) });
                db.SaveChanges();
                day = db.Days.FirstOrDefault(d => d.Date == DateOnly.Parse(dto.Date));
            }
            dto.EmployeeDay.DayId = day.DayId;
            db.EmployeeDays.Add(dto.EmployeeDay);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeDay), new { id = dto.EmployeeDay.EmployeeDayID }, dto.EmployeeDay);
        }

        // PUT: api/EmployeeDay/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeDay(int id, EmployeeDay updated)
        {
            if (id != updated.EmployeeDayID)
            {
                return BadRequest();
            }

            db.Entry(updated).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeDayExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/EmployeeDay/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeDay(int id)
        {
            var employeeDay = await db.EmployeeDays.FindAsync(id);
            if (employeeDay == null)
            {
                return NotFound();
            }

            db.EmployeeDays.Remove(employeeDay);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeDayExists(int id)
        {
            return db.EmployeeDays.Any(e => e.EmployeeDayID == id);
        }
    }
}
