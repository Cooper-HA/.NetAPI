
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetProject.Domain;
using Microsoft.Extensions.Configuration;

using FinalWebServiceProject.Functions;

namespace FinalWebServiceProject
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(
              "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Cafe"
            ));

            builder.Services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowLocalhost", policy =>
            //    {
            //        policy.WithOrigins("http://localhost", "https://eeeaccounting.com/")
            //              .AllowAnyHeader()
            //              .AllowAnyMethod();
            //    });
            //});
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        
            app.UseCors(policy =>
                policy.WithOrigins("http://localhost", "https://eeeaccounting.com")
                      .AllowAnyMethod()
                      .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}
