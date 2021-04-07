using CoreWebApi.Data;
using CoreWebApi.IData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreWebApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlServer(config.GetConnectionString("DefaultConnection")));

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
            services.AddScoped<ISemesterFeeRepository, SemesterFeeRepository>();
            services.AddScoped<ITutorRepository, TutorRepository>();



            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IFilesRepository, FilesRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }
    }
}
