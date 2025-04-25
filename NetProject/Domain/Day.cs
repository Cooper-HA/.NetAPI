using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.Domain
{
    public class Day
    {

        [Key]
        public int DayId { get; set; }
        public DateOnly Date { get; set; }
        public List<EmployeeDay> EmployeeDays { get; set; }

        public Day()
        {

        }
    }
}
