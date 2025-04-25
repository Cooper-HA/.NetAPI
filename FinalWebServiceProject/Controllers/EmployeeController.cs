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


namespace FinalWebServiceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ProjectContext db;

        public EmployeeController(ProjectContext db)
        {
            this.db = db;
            this.db.Database.EnsureCreated();
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return db.Employees.AsEnumerable();
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            Employee employee = db.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null)
            {
                NotFound();
            }

            return Ok(employee);
        }
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<EmployeeDay>>> GetEmployeeSchedule(int employeeId, string month)
        {
            var startOfMonth = DateOnly.Parse($"{month}-01");
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            Console.WriteLine($"Start of Month: {startOfMonth}, End of Month: {endOfMonth}");
            var shifts = await (from ed in db.EmployeeDays
                                join d in db.Days on ed.DayId equals d.DayId
                                where ed.EmployeeId == employeeId &&
                                      d.Date >= startOfMonth &&
                                      d.Date <= endOfMonth
                                select new
                                {
                                    EmployeeDayId = ed.EmployeeDayID,
                                    ed.EmployeeId,
                                    ed.StartTime,
                                    ed.EndTime,
                                    ed.Position,
                                    DayDate = d.Date
                                }).ToListAsync();


            if (shifts == null || !shifts.Any())
            {
                return NotFound();
            }

            
            return Ok(shifts);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] NetProject.Domain.LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = db.Employees.FirstOrDefault(e => e.Username == request.Username);

            if(user != null && EmployeeService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Ok(new
                {
                    username = user.Username,
                    role = user._isManager,
                    userId = user.EmployeeId
                });
            }
            else
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }
        }


    }
}
