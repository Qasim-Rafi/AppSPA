using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
   public interface IAuthRepository
    {
        Task<ServiceResponse<object>> Register(UserForRegisterDto user, string regNo);
        Task<ServiceResponse<object>> ForgotPassword(ForgotPasswordDto user);
        Task<ServiceResponse<object>> ResetPassword(ResetPasswordDto model);

        Task<User> Login(string username, string password, int schoolBranchId);
        Task<object> GetSchoolDetails(string regNo, int branchId);
        Task<bool> UserExists(string  userName, string  schoolName);
        //Task<ServiceResponse<object>> UploadFile(UploadFileDto model);
        Task<User> ExStudentLogin(string username, string password, int tutorId, int subjectId);
        Task<ServiceResponse<object>> ExStudentRegister(ExStudentForRegisterDto model);
        ServiceResponse<object> SiteCheck();

    }
}

