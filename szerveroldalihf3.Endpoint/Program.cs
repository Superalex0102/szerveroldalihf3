using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using szerveroldalihf3.Endpoint.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using szerveroldalihf3.Logic.Dto;
using szerveroldalihf3.Logic;
using szerveroldalihf3.Data;
using szerveroldalihf3.Entities.Entity;

namespace szerveroldalihf3.Endpoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ExceptionFilter>();
                opt.Filters.Add<ValidationFilter>();
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Jira API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            builder.Services.AddTransient(typeof(Repository<>));
            builder.Services.AddTransient<JiraLogic>();
            builder.Services.AddTransient<DtoProvider>();
            builder.Services.AddTransient<JiraHub>();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<JiraContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "jirahazi.com",
                    ValidIssuer = "jirahazi.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"] ?? throw new Exception("jwt:key not found in appsettings.json")))
                };
            });

            builder.Services.AddDbContext<JiraContext>(opt =>
            {
                opt
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=JiraDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True")
                .UseLazyLoadingProxies();
            });

            builder.Services.AddHangfire(config =>
                    config.UseSqlServerStorage("Server=(localdb)\\MSSQLLocalDB;Database=JiraHangfire;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"));
            builder.Services.AddHangfireServer();

            builder.Services.AddSignalR();

            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();



            app.MapControllers();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<JiraHub>("/forumHub");
            });

            app.UseHangfireDashboard();

            app.Run();
        }
    }
}
