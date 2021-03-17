using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreWebApi.Middleware;
using AutoMapper;
using CoreWebApi.Controllers;
using CoreWebApi.Data;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using static CoreWebApi.Helpers.GenericFunctions;
using CoreWebApi.Hubs;

namespace CoreWebApi
{
    public class Startup
    {
        bool isDev;
        private readonly IWebHostEnvironment _HostEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment env, IWebHostEnvironment HostEnvironment)
        {
            Configuration = configuration;
            isDev = env.IsDevelopment();
            _HostEnvironment = HostEnvironment;
        }



        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddDistributedMemoryCache();

                services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                services.AddControllers().AddNewtonsoftJson();
                services.AddCors();
                services.AddAutoMapper(typeof(UserRepository).Assembly);
                services.AddScoped<IAuthRepository, AuthRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IClassRepository, ClassRepository>();
                services.AddScoped<ISectionRepository, SectionRepository>();
                services.AddScoped<ISubjectRepository, SubjectRepository>();
                services.AddScoped<IAttendanceRepository, AttendanceRepository>();
                services.AddScoped<ILeaveRepository, LeaveRepository>();
                services.AddScoped<IAssignmentRepository, AssignmentRepository>();
                services.AddScoped<IDashboardRepository, DashboardRepository>();
                services.AddScoped<IExamRepository, ExamRepository>();
                services.AddScoped<ISchoolRepository, SchoolRepository>();
                services.AddScoped<ITeacherRepository, TeacherRepository>();
                services.AddScoped<IMessageRepository, MessageRepository>();
                services.AddScoped<IResultRepository, ResultRepository>();
                services.AddScoped<IStudentRepository, StudentRepository>();
                services.AddScoped<IAdminRepository, AdminRepository>();



                services.AddScoped<ILookupRepository, LookupRepository>();
                services.AddScoped<IFilesRepository, FilesRepository>();
                services.AddScoped<IEmailRepository, EmailRepository>();
                services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

                var EmailMetadata = Configuration.GetSection("EmailSettings").Get<EmailSettings>();
                services.AddSingleton(EmailMetadata);

                IFileProvider physicalProvider = new PhysicalFileProvider(Path.Combine(_HostEnvironment.ContentRootPath, "SchoolDocuments"));//(@"D:\Published\VImages");
                services.AddSingleton<IFileProvider>(physicalProvider);

                services.AddSignalR();
                //services.AddSignalR(hubOptions =>
                //{
                //    hubOptions.EnableDetailedErrors = true;
                //    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
                //});

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(optinos =>
                    {
                        optinos.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings").GetSection("Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

                if (isDev)
                {
                    services.AddSwaggerGen(swagger =>
                    {
                        swagger.DocumentFilter<ApplyDocumentVendorExtensions>();
                        //This is to generate the Default UI of Swagger Documentation  
                        swagger.SwaggerDoc("v2", new OpenApiInfo
                        {
                            Version = "v2",
                            Title = "LMS Web Api",
                            Description = "ASP.NET Core 3.1 Web API"
                        });
                        // To Enable authorization using Swagger (JWT)  
                        swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.ApiKey,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        });
                        swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                        {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}

                        }
                        });

                    });

                }


            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                Console.WriteLine(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                //app.UseMiddleware<ExceptionMiddleware>();
                app.UseDeveloperExceptionPage();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v2/swagger.json", "PlaceInfo Services"); options.DefaultModelsExpandDepth(-1); });

                }

                //app.UseHttpsRedirection();
                app.UseAuthentication();

                //app.UseStaticFiles();
                //if (env.IsDevelopment())
                //{
                //app.UseFileServer(new FileServerOptions
                //{
                //    FileProvider = new PhysicalFileProvider(Configuration.GetSection("AppSettings").GetSection("VirtualDirectoryPath").Value),//(@"D:\Published\VImages"),
                //    RequestPath = new PathString("/Images"),
                //    EnableDirectoryBrowsing = false
                //});
                //app.UseStaticFiles(new StaticFileOptions()
                //{
                //    FileProvider = new PhysicalFileProvider(Path.Combine(_HostEnvironment.WebRootPath, "StaticFiles")),
                //    RequestPath = new PathString("/StaticFiles")
                //});
                //}
                //else if (env.IsProduction())
                //{
                //    app.UseStaticFiles(new StaticFileOptions()
                //    {
                //        FileProvider = new PhysicalFileProvider(Path.Combine("https://e-learningbox.com/webAPI/wwwroot", "StaticFiles")),
                //        RequestPath = new PathString("/StaticFiles")
                //    });
                //}
                app.UseRouting();

                app.UseAuthorization();

                app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

               
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<ChatHub>("/notificationHub");
                    endpoints.MapHub<WebRtcHub>("/rtcHub");
                });
            }
            catch (Exception ex)
            {
                //Log.Exception(ex);
                Console.WriteLine(ex);
            }
        }
    }
}
