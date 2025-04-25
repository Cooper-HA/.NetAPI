using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.Domain
{
    public class Schedule
    {
        public int ScheduleId {get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Day> Days { get; set; }
        public Schedule() { }
    }
}
