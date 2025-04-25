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
    public class TimeOffController: ControllerBase
    {
        private readonly ProjectContext db;

        public TimeOffController(ProjectContext db)
        {
            this.db = db;
            this.db.Database.EnsureCreated();
        }
    
        [HttpGet]
        public async Task<IActionResult> GetTOR()
        {
            var torList = await (
                from tr in db.TimeRequests
                join e in db.Employees on tr.EmployeeId equals e.EmployeeId
                select new
                {
                    tr.TimeRequestId,
                    EmployeeName = e.FirstName + " " + e.LastName,
                    tr.StartDate,
                    tr.EndDate,
                    tr.Reason,
                    requestStatus = tr.RequestStatus.ToString()
                }
            ).ToListAsync();

            return Ok(torList);
        }
        [HttpGet("employee/{id}")]
        public async Task<IActionResult> GetTOR(int id)
        {
            var torList = await (
                from tr in db.TimeRequests
                join e in db.Employees on tr.EmployeeId equals e.EmployeeId
                where tr.EmployeeId == id
                select new
                {
                    tr.TimeRequestId,
                    EmployeeName = e.FirstName + " " + e.LastName,
                    tr.StartDate,
                    tr.EndDate,
                    tr.Reason,
                    requestStatus = tr.RequestStatus.ToString()
                }
            ).ToListAsync();

            return Ok(torList);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTOR(int id, [FromBody] TimeRequest request)
        {
            var TOR = db.TimeRequests.FirstOrDefault(tr => tr.TimeRequestId == id);
            if (TOR == null)
            {
                return NotFound();
            }

            TOR.RequestStatus = (TimeRequest.Status?) Enum.ToObject(typeof(TimeRequest.Status), request.RequestStatus);
            db.SaveChanges();

            var result = new
            {
                TOR.TimeRequestId,
                TOR.EmployeeId,
                TOR.StartDate,
                TOR.EndDate,
                TOR.Reason,
                RequestStatus = TOR.RequestStatus.ToString()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> TimeOffRequest([FromBody] NetProject.Domain.TimeRequest request)
        {
            if(request.RequestStatus != TimeRequest.Status.PENDING)
            {
                request.RequestStatus = TimeRequest.Status.PENDING;
            }

            db.Add(request);
            db.SaveChanges();
            var result = new
            {
                request.TimeRequestId,
                request.EmployeeId,
                request.StartDate,
                request.EndDate,
                request.Reason,
                RequestStatus = request.RequestStatus.ToString()
            };
            return Ok(result);
        }
    }
}
