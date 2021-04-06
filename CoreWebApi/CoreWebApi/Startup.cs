using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Extensions;
using CoreWebApi.Helpers;
using CoreWebApi.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace CoreWebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _HostEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment HostEnvironment)
        {
            Configuration = configuration;
            _HostEnvironment = HostEnvironment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddDistributedMemoryCache();

                services.AddControllers().AddNewtonsoftJson();
                services.AddCors();
                services.AddAutoMapper(typeof(UserRepository).Assembly);
                services.AddApplicationServices(Configuration);
                services.AddIdentityServices(Configuration, _HostEnvironment);


                var EmailMetadata = Configuration.GetSection("EmailSettings").Get<EmailSettings>();
                services.AddSingleton(EmailMetadata);

                IFileProvider physicalProvider = new PhysicalFileProvider(Path.Combine(_HostEnvironment.ContentRootPath, "SchoolDocuments"));//(@"D:\Published\VImages");
                services.AddSingleton<IFileProvider>(physicalProvider);

                services.AddSignalR();
                //services.AddSignalR(hubOptions =>
                //{
                //    hubOptions.EnableDetailedErrors = true;
                //    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
                //    //hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(10);
                //});


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
