using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

using NetProject.Services;

namespace NetProject.Domain
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }
        public ProjectContext() : base() { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<EmployeeDay> EmployeeDays { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TimeRequest> TimeRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Cafe"
            ).LogTo(Console.WriteLine,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information)
            .EnableSensitiveDataLogging();


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeDay>()
                .HasOne<Employee>()
                .WithMany(e => e.EmployeeDays)
                .HasForeignKey(ed => ed.EmployeeId);

            modelBuilder.Entity<EmployeeDay>()
                .HasOne<Day>()
                .WithMany(d => d.EmployeeDays)
                .HasForeignKey(ed => ed.DayId   );

            modelBuilder.Entity<TimeRequest>(tr =>
            {
                tr.Property(c => c.EmployeeId).IsRequired();
                tr.HasOne<Employee>()
                    .WithMany()
                    .HasForeignKey(cp => cp.EmployeeId);
            });

            var employees = new List<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "Alice", LastName = "Doe", DefaultPos = "Cook",PasswordHash = EmployeeService.HashPassword("123"), Username = "Alice"}, //EmployeeService.HashPassword("1234")},
                new Employee { EmployeeId = 2, FirstName = "Bob", LastName = "Bonavil", DefaultPos = "Dish", PasswordHash = EmployeeService.HashPassword("qwerty"), Username= "Bob"},//EmployeeService.HashPassword("qwerty")}
                new Employee { EmployeeId = 3, FirstName = "Steve", LastName = "Stebins", DefaultPos = "Server", PasswordHash = EmployeeService.HashPassword("!@#"), Username= "Steve"},//EmployeeService.HashPassword("qwerty")}
                new Employee { EmployeeId = 4, FirstName = "Jonathan", LastName = "Ernhart", DefaultPos = "Bus", PasswordHash = EmployeeService.HashPassword("coder"), Username= "Jonathan"},//EmployeeService.HashPassword("qwerty")}
                new Employee { EmployeeId = 5, FirstName = "James", LastName = "Matter", DefaultPos = "RM", PasswordHash = EmployeeService.HashPassword("mark"), Username= "James"},//EmployeeService.HashPassword("qwerty")}
                new Employee { EmployeeId = 6, FirstName = "Anna", LastName = "Osborn", DefaultPos = "Server", PasswordHash = EmployeeService.HashPassword("Karana"), Username= "Anna"},//EmployeeService.HashPassword("qwerty")}
                new Employee { EmployeeId = 7, FirstName = "Jenny", LastName = "Knofler", DefaultPos = "Cook", PasswordHash = EmployeeService.HashPassword("jump"), Username= "Jenny"}//EmployeeService.HashPassword("qwerty")}

            };
            modelBuilder.Entity<Employee>().HasData(employees);
            var days = new List<Day>();
            var today = DateOnly.FromDateTime(DateTime.Now);

            for (int i = 1; i <= 10; i++)
            {
                var day = new Day() { DayId = i, Date = today.AddDays(i) };
                days.Add(day);
            }

            var employeeDays = new List<EmployeeDay>
            {
                new EmployeeDay {EmployeeDayID = 1, EmployeeId = 1, DayId = 1, Position = "Cook", StartTime = new DateTime(2025, 3, 11, 9,0,0), EndTime = new DateTime(2025, 3, 11, 16,0,0)},
                new EmployeeDay {EmployeeDayID = 2, EmployeeId = 1, DayId = 2, Position = "Cook", StartTime = new DateTime(2025, 3, 11, 9,0,0), EndTime = new DateTime(2025, 3, 11, 16,0,0)},
                new EmployeeDay {EmployeeDayID = 3, EmployeeId = 2, DayId = 1, Position = "Dish", StartTime = new DateTime(2025, 3, 11, 6,30,0), EndTime = new DateTime(2025, 3, 11, 16,0,0)}
            };


            modelBuilder.Entity<Day>().HasData(days);
            modelBuilder.Entity<EmployeeDay>().HasData(employeeDays);

            modelBuilder.Entity<TimeRequest>().HasData(new List<TimeRequest>{ 
                new TimeRequest() { TimeRequestId = 1,EmployeeId = 1, StartDate=new DateOnly(2025, 5, 10), EndDate = new DateOnly(2025, 5, 17), Reason = "Family Trip"}
            });

        }
    }
}
