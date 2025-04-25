using System;
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
    public class DayController: ControllerBase
    {
        private readonly ProjectContext db;

        public DayController(ProjectContext db)
        {
            this.db = db;
            this.db.Database.EnsureCreated();
        }
    
        [HttpGet]
        public async Task<IActionResult> GetDays()
        {
            var dayList = db.Days.Include(e => e.EmployeeDays).ToList();

            return Ok(dayList);
        }
        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetDate(string date)
        {
            
            var day = db.Days.FirstOrDefault(d => d.Date == DateOnly.Parse(date));
            return Ok(day);
        }
    }
}
