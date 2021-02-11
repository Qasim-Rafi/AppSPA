using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public StudentRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _mapper = mapper;
            _serviceResponse = new ServiceResponse<object>();
        }

        public async Task<ServiceResponse<object>> AddFee(StudentFeeDtoForAdd model)
        {
            var ToAdd = new StudentFee
            {
                StudentId = model.StudentId,
                ClassSectionId = model.ClassSectionId,
                Remarks = model.Remarks,
                Paid = model.Paid,
                Month = DateTime.Now.ToString("MMMM"),
                CreatedDateTime = DateTime.Now,
                CreatedById = _LoggedIn_UserID,
            };
            await _context.StudentFees.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public Task<ServiceResponse<object>> GetFee()
        {
            throw new NotImplementedException();
        }
    }
}
