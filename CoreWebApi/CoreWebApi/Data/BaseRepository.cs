using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class BaseRepository
    {
        protected int _LoggedIn_UserID = 0;
        protected readonly int _LoggedIn_BranchID = 0;
        protected string _LoggedIn_UserName = "";
        protected string _LoggedIn_UserRole = "";
        protected readonly string _LoggedIn_SchoolExamType = "";
        protected ServiceResponse<object> _serviceResponse;
        protected DataContext _context;

        public BaseRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _LoggedIn_SchoolExamType = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.ExamType.ToString());
            _serviceResponse = new ServiceResponse<object>();
            _context = context;
        }
    }
}
