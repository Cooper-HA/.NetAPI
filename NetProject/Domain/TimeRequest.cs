using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetProject.Domain
{
    public class TimeRequest
    {
        public int TimeRequestId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int EmployeeId { get; set; }
        public string ?Reason { get; set; }
        public Status ?RequestStatus { get; set; }
        public enum Status
        {
            PENDING,
            APPROVED,
            REJECTED,
            COMPLETED,
            CANCELLED
        }
    }
}
