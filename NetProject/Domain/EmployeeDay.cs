using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.Domain
{
    public class EmployeeDay
    {
        [Key]
        public int EmployeeDayID { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Position { get; set; }

        [AllowNull]
        public DateTime ?AcctualStartTime { get; set; }

        [AllowNull]
        public DateTime ?AcctualEndTime { get; set; }
        public int DayId { get; set; }
        public EmployeeDay() { 
        }

        //public override string ToString()
        //{
            
        //    var employeeName = Employee != null ? $"{Employee.FirstName} {Employee.LastName}" : "No Employee Assigned";
        //    var AST = AcctualStartTime ?? new DateTime(2000, 1, 1, 0, 0, 0);
        //    var AET = AcctualEndTime ?? new DateTime(2000, 1, 1, 0, 0, 0);
        //    return $"Employee: {employeeName}, Day: {Date}, Scheduled: {StartTime:HH:mm} - {EndTime:HH:mm}, Actual: {AST:HH:mm}-{AET:HH:mm}";
        //}
    }
}
