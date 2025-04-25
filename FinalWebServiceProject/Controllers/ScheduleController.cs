using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetProject.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using NetProject.Services;
using NetProject.DTO;

namespace FinalWebServiceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ProjectContext db;

        public ScheduleController(ProjectContext db)
        {
            this.db = db;
            this.db.Database.EnsureCreated();
        }

        [HttpGet]
        public async Task<IActionResult> GetSchedules([FromQuery] string month)
        {
            if (!DateOnly.TryParse($"{month}-01", out var monthStart))
                return BadRequest("Invalid month format. Use YYYY-MM.");

            var monthEnd = monthStart.AddMonths(1);

            var daysInMonth = await db.Days
                .Where(d => d.Date >= monthStart && d.Date < monthEnd)
                .ToListAsync();

            var dayIds = daysInMonth.Select(d => d.DayId).ToList();

            var employeeDays = await db.EmployeeDays
                .Where(ed => dayIds.Contains(ed.DayId))
                .ToListAsync();

            var employeeIds = employeeDays.Select(ed => ed.EmployeeId).Distinct().ToList();

            var employees = await db.Employees
                .Where(e => employeeIds.Contains(e.EmployeeId))
                .ToListAsync();

            var dayEmployeeMap = dayIds.ToDictionary(
                dayId => dayId,
                dayId => employeeDays
                    .Where(ed => ed.DayId == dayId)
                    .Select(ed =>
                    {
                        var emp = employees.FirstOrDefault(e => e.EmployeeId == ed.EmployeeId);
                        return emp != null
                            ? new EmployeeShiftDto { Name = emp.Username, Position = ed.Position }
                            : null;
                    })
                    .Where(e => e != null)
                    .ToList()
            );

            var result = daysInMonth.Select(day =>
            {
                dayEmployeeMap.TryGetValue(day.DayId, out var employeesForDay);

                return new
                {
                    date = day.Date.ToString("yyyy-MM-dd"),
                    employees = employeesForDay
                };
            });



            return Ok(result);
        }

        [HttpGet("templates")]
        public async Task<IActionResult> GetTemplates()
        {
            var templates = db.Schedules.Include(e => e.Days).ThenInclude(e => e.EmployeeDays).ToList();
            return Ok(templates);
        }

        [HttpGet("{id}")]
        public Schedule GetSchedule(int id)
        {
            Schedule schedule = db.Schedules.FirstOrDefault(e => e.ScheduleId == id);
            if (schedule == null)
            {
                NotFound();
            }

            return schedule;
        }

        [HttpPost]
        public ActionResult<Schedule> AddSchedule([FromBody] Schedule newSchedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Schedules.Add(newSchedule);
            foreach (var day in newSchedule.Days) {
                foreach(var ED in day.EmployeeDays)
                {
                    db.EmployeeDays.Add(ED);
                }
                db.Days.Add(day);            
            }
            db.SaveChanges();

            return CreatedAtAction(nameof(GetSchedule), new { id = newSchedule.ScheduleId }, newSchedule);
        }



    }
}
