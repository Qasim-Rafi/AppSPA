using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly IFilesRepository _filesRepository;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public AssignmentRepository(DataContext context, IWebHostEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository filesRepository)
        {
            _context = context;
            _HostEnvironment = HostEnvironment;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _mapper = mapper;
            _filesRepository = filesRepository;
            _serviceResponse = new ServiceResponse<object>();
        }
        public async Task<bool> AssignmentExists(string name)
        {
            if (await _context.Assignments.AnyAsync(x => x.AssignmentName == name))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetAssignment(int id)
        {
            var ToReturn = await _context.Assignments.Select(o => new AssignmentDtoForDetail
            {
                Id = o.Id,
                AssignmentName = o.AssignmentName,
                ClassSectionId = o.ClassSectionId,
                ClassSection = (_context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true) != null && _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true) != null) ? _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true).Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true).SectionName : "",
                RelatedMaterial = o.RelatedMaterial,
                Details = o.Details,
                ReferenceUrl = o.ReferenceUrl,
            }).FirstOrDefaultAsync(u => u.Id == id);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAssignments()
        {
            var ToReturn = await _context.Assignments.Include(m => m.ClassSection).Select(o => new AssignmentDtoForList
            {
                Id = o.Id,
                AssignmentName = o.AssignmentName,
                ClassSectionId = o.ClassSectionId,
                ClassSection = (_context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true) != null && _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true) != null) ? _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true).Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true).SectionName : "",
                RelatedMaterial = o.RelatedMaterial,
                Details = o.Details,
                ReferenceUrl = o.ReferenceUrl,
            }).ToListAsync();
            
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse; ;
        }
        public async Task<ServiceResponse<object>> AddAssignment(AssignmentDtoForAdd assignment)
        {

            var objToCreate = new Assignment
            {
                AssignmentName = assignment.AssignmentName,
                CreatedById = _LoggedIn_UserID,
                CreatedDateTime = DateTime.Now,
                Details = assignment.Details,
                TeacherName = assignment.TeacherName,
                ClassSectionId = assignment.ClassSectionId,
                ReferenceUrl = assignment.ReferenceUrl,
                SchoolBranchId = _LoggedIn_BranchID
            };

            if (assignment.files != null && assignment.files.Count() > 0)
            {              
                for (int i = 0; i < assignment.files.Count(); i++)
                {
                    var dbPath = _filesRepository.SaveFile(assignment.files[i]);
                    if (i == 0)
                        objToCreate.RelatedMaterial = dbPath;
                    else
                        objToCreate.RelatedMaterial = objToCreate.RelatedMaterial + "||" + dbPath;
                }
            }
            await _context.Assignments.AddAsync(objToCreate);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> EditAssignment(int id, AssignmentDtoForEdit assignment)
        {


            Assignment dbObj = _context.Assignments.FirstOrDefault(s => s.Id.Equals(id));
            if (dbObj != null)
            {
                dbObj.AssignmentName = assignment.AssignmentName;
                dbObj.Details = assignment.Details;
                dbObj.ClassSectionId = assignment.ClassSectionId;
                dbObj.ReferenceUrl = assignment.ReferenceUrl;


                if (assignment.files != null && assignment.files.Count() > 0)
                {

                    for (int i = 0; i < assignment.files.Count(); i++)
                    {
                        var dbPath = _filesRepository.SaveFile(assignment.files[i]);
                        if (i == 0)
                            dbObj.RelatedMaterial = dbPath;
                        else
                            dbObj.RelatedMaterial = dbObj.RelatedMaterial + "||" + dbPath;
                    }
                }
                _context.Assignments.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;

        }
    }
}
