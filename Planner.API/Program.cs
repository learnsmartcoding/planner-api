using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Planner.Common.Filters;
using Planner.Data;
using System.Configuration;

namespace Planner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;
            // Add services to the container.

            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddMicrosoftIdentityWebApi(options =>
                   {
                       configuration.Bind("AzureAdB2C", options);

                       options.TokenValidationParameters.NameClaimType = "name";
                   },
           options => { configuration.Bind("AzureAdB2C", options); });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<PlannerContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DbContext"))
              //.EnableSensitiveDataLogging()
              );

            builder.Services.Scan(scan => scan.FromCallingAssembly().AddClasses().AsMatchingInterface().WithScopedLifetime());
            builder.Services.AddScoped<IPlanScheduleRepository, PlanScheduleRepository>();
            builder.Services.AddCors(o => o.AddPolicy("default", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("default");

            app.MapControllers();

            app.Run();
        }
    }
}

