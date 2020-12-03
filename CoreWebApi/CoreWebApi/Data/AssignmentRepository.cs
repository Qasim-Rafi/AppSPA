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
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public AssignmentRepository(DataContext context, IWebHostEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _HostEnvironment = HostEnvironment;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString()).ToString();
        }
        public async Task<bool> AssignmentExists(string name)
        {
            if (await _context.Assignments.AnyAsync(x => x.AssignmentName == name))
                return true;
            return false;
        }
        public async Task<Assignment> GetAssignment(int id)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(u => u.Id == id);
            return assignment;
        }

        public async Task<object> GetAssignments()
        {
            var assignments = await _context.Assignments.Include(m => m.ClassSection).ToListAsync();
            var ToReturn = assignments.Select(o => new AssignmentDtoForList
            {
                Id = o.Id,
                AssignmentName = o.AssignmentName,
                ClassSection = _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId)?.Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId)?.SectionName,
                RelatedMaterial = o.RelatedMaterial,
                Details = o.Details,

            }).ToList();
            return ToReturn;
        }
        public async Task<Assignment> AddAssignment(AssignmentDtoForAdd assignment)
        {

            var objToCreate = new Assignment
            {
                AssignmentName = assignment.AssignmentName,
                CreatedById = _LoggedIn_UserID,
                CreatedDateTime = DateTime.Now,
                Details = assignment.Details,
                TeacherName = assignment.TeacherName,
                ClassSectionId = assignment.ClassSectionId,
                SchoolBranchId = _LoggedIn_BranchID
            };


            if (assignment.files != null && assignment.files.Count() > 0)
            {

                string contentRootPath = _HostEnvironment.ContentRootPath;
                var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");
                for (int i = 0; i < assignment.files.Count(); i++)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(assignment.files[i].FileName);
                    var fullPath = Path.Combine(pathToSave);
                    var dbPath = Path.Combine("StaticFiles", "Images", fileName); //you can add this path to a list and then return all dbPaths to the client if require
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                    var filePath = Path.Combine(fullPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await assignment.files[i].CopyToAsync(stream);
                    }
                    if (i == 0)
                        objToCreate.RelatedMaterial = dbPath;
                    else
                        objToCreate.RelatedMaterial = objToCreate.RelatedMaterial + "||" + dbPath;
                }
            }
            await _context.Assignments.AddAsync(objToCreate);
            await _context.SaveChangesAsync();
            return objToCreate;

        }
        public async Task<Assignment> EditAssignment(int id, AssignmentDtoForEdit assignment)
        {


            Assignment dbObj = _context.Assignments.FirstOrDefault(s => s.Id.Equals(id));
            if (dbObj != null)
            {
                dbObj.AssignmentName = assignment.AssignmentName;
                dbObj.Details = assignment.Details;
                dbObj.ClassSectionId = assignment.ClassSectionId;
                //dbObj.TeacherName = assignment.TeacherName;

                // _context.Assignments.Attach(assignment);
                // _context.Entry(assignment).State = EntityState.Modified;
                if (assignment.files != null && assignment.files.Count() > 0)
                {

                    string contentRootPath = _HostEnvironment.ContentRootPath;
                    var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");
                    for (int i = 0; i < assignment.files.Count(); i++)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(assignment.files[i].FileName);
                        var fullPath = Path.Combine(pathToSave);
                        var dbPath = Path.Combine("StaticFiles", "Images", fileName); //you can add this path to a list and then return all dbPaths to the client if require
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                        var filePath = Path.Combine(fullPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await assignment.files[i].CopyToAsync(stream);
                        }
                        if (i == 0)
                            dbObj.RelatedMaterial = dbPath;
                        else
                            dbObj.RelatedMaterial = dbObj.RelatedMaterial + " || " + dbPath;
                    }
                }
                _context.Assignments.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            return dbObj;

        }
    }
}
