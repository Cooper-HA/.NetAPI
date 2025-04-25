using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace NetProject.Domain
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public string ?FirstName { get; set; }
        public string ?LastName { get; set; }

        [JsonIgnore]
        [System.ComponentModel.DataAnnotations.Required]
        public string PasswordHash { get; set; }
        public double ?HoursWorked { get; set; }
        public string ?Username { get; set; }
        public string ?DefaultPos {  get; set; }
        public bool _isManager { get; set; } = false;
        public List<EmployeeDay> ?EmployeeDays { get; set; }


        public Employee() { }
    }
}
