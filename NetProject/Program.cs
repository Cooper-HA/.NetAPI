using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetProject.Domain;

namespace CafeProject
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            ProjectContext _context = new();
            _context.Database.EnsureCreated();
            List<EmployeeDay> Days =_context.EmployeeDays.Where(e => e.EmployeeId == 1).ToList();
            Console.WriteLine(Days.Count);
            for (int i = 0; i < Days.Count; i++) { 
                Console.WriteLine(Days[i]);
            }
        }
    }
}
