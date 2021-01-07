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
        private string _LoggedIn_UserRole = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public AssignmentRepository(DataContext context, IWebHostEnvironment HostEnvironment, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository filesRepository)
        {
            _context = context;
            _HostEnvironment = HostEnvironment;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _mapper = mapper;
            _filesRepository = filesRepository;
            _serviceResponse = new ServiceResponse<object>();
        }
        public async Task<bool> AssignmentExists(string name)
        {
            if (await _context.ClassSectionAssignment.AnyAsync(x => x.AssignmentName == name))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetAssignment(int id)
        {
            var ToReturn = await _context.ClassSectionAssignment.Select(o => new AssignmentDtoForDetail
            {
                Id = o.Id,
                AssignmentName = o.AssignmentName,
                ClassSectionId = o.ClassSectionId,
                ClassSection = (_context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true) != null && _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true) != null) ? _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true).Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true).SectionName : "",
                RelatedMaterial = _filesRepository.AppendMultiDocPath(o.RelatedMaterial),
                FileName = SplitValues(o.FileName),
                Details = o.Details,
                ReferenceUrl = o.ReferenceUrl,
                SubjectId = o.SubjectId,
                DueDateTime = o.DueDateTime != null ? DateFormat.ToDate(o.DueDateTime.ToString()) : "",
                IsPosted = o.IsPosted,
            }).FirstOrDefaultAsync(u => u.Id == id);
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetAssignments()
        {
            if (!string.IsNullOrEmpty(_LoggedIn_UserRole))
            {
                List<AssignmentDtoForList> ToReturn = new List<AssignmentDtoForList>();
                if (_LoggedIn_UserRole == Enumm.UserType.Student.ToString())
                {
                    var ids = _context.ClassSectionAssigmentSubmissions.Where(m => m.StudentId == _LoggedIn_UserID).Select(m => m.ClassSectionAssignmentId).ToList();

                    ToReturn = await (from csAssign in _context.ClassSectionAssignment
                                      join subject in _context.Subjects
                                      on csAssign.SubjectId equals subject.Id

                                      join classSection in _context.ClassSections
                                      on csAssign.ClassSectionId equals classSection.Id

                                      join classSectionUser in _context.ClassSectionUsers
                                      on classSection.Id equals classSectionUser.ClassSectionId

                                      //join assignSub in _context.ClassSectionAssigmentSubmissions
                                      //on csAssign.Id equals assignSub.ClassSectionAssignmentId into AssignSubmission
                                      //from assignSub in AssignSubmission.DefaultIfEmpty()

                                      where classSectionUser.UserId == _LoggedIn_UserID
                                      && csAssign.SchoolBranchId == _LoggedIn_BranchID
                                      && !ids.Contains(csAssign.Id)
                                      && subject.Active == true
                                      && classSection.Active == true
                                      && csAssign.IsPosted == true
                                      && csAssign.DueDateTime.Value.Date >= DateTime.Now.Date
                                      orderby csAssign.Id descending
                                      select csAssign).Select(o => new AssignmentDtoForList
                                      {
                                          Id = o.Id,
                                          AssignmentName = o.AssignmentName,
                                          ClassSectionId = o.ClassSectionId,
                                          ClassSection = (_context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true) != null && _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true) != null) ? _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true).Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true).SectionName : "",
                                          RelatedMaterial = _filesRepository.AppendMultiDocPath(o.RelatedMaterial),
                                          FileName = SplitValues(o.FileName),
                                          Details = o.Details,
                                          ReferenceUrl = o.ReferenceUrl,
                                          SubjectId = o.SubjectId,
                                          DueDateTime = o.DueDateTime != null ? DateFormat.ToDate(o.DueDateTime.ToString()) : "",
                                          IsPosted = o.IsPosted,
                                      }).ToListAsync();

                }
                else if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
                {
                    ToReturn = await (from csAssign in _context.ClassSectionAssignment
                                      join subject in _context.Subjects
                                      on csAssign.SubjectId equals subject.Id

                                      join classSection in _context.ClassSections
                                      on csAssign.ClassSectionId equals classSection.Id

                                      where csAssign.CreatedById == _LoggedIn_UserID
                                      && csAssign.SchoolBranchId == _LoggedIn_BranchID
                                      && subject.Active == true
                                      && classSection.Active == true
                                      orderby csAssign.Id descending
                                      select csAssign).Select(o => new AssignmentDtoForList
                                      {
                                          Id = o.Id,
                                          AssignmentName = o.AssignmentName,
                                          ClassSectionId = o.ClassSectionId,
                                          ClassSection = (_context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true) != null && _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true) != null) ? _context.Class.FirstOrDefault(m => m.Id == o.ClassSection.ClassId && m.Active == true).Name + " " + _context.Sections.FirstOrDefault(m => m.Id == o.ClassSection.SectionId && m.Active == true).SectionName : "",
                                          RelatedMaterial = _filesRepository.AppendMultiDocPath(o.RelatedMaterial),
                                          FileName = SplitValues(o.FileName),
                                          Details = o.Details,
                                          ReferenceUrl = o.ReferenceUrl,
                                          SubjectId = o.SubjectId,
                                          DueDateTime = o.DueDateTime != null ? DateFormat.ToDate(o.DueDateTime.ToString()) : "",
                                          IsPosted = o.IsPosted,
                                      }).ToListAsync();
                }

                _serviceResponse.Success = true;
                _serviceResponse.Data = ToReturn;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.UserNotLoggedIn;
                return _serviceResponse;
            }
        }
        private static List<string> SplitValues(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new List<string>();
            else
                return value.Split(separator: "||").ToList();
        }
        public async Task<ServiceResponse<object>> AddAssignment(AssignmentDtoForAdd assignment)
        {

            DateTime DueDateTime = DateTime.ParseExact(assignment.DueDateTime, "MM/dd/yyyy", null);

            var UserObj = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
            var SubjectObj = _context.Subjects.Where(m => m.Id == assignment.SubjectId).FirstOrDefault();
            var AssignmentName = $"{DateTime.Now.ToShortDateString()} - {UserObj?.FullName} - {SubjectObj?.Name}";
            var objToCreate = new ClassSectionAssignment
            {
                AssignmentName = AssignmentName,
                Details = assignment.Details,
                ClassSectionId = assignment.ClassSectionId,
                SubjectId = assignment.SubjectId,
                ReferenceUrl = assignment.ReferenceUrl,
                DueDateTime = DueDateTime,
                IsPosted = assignment.IsPosted,
                SchoolBranchId = _LoggedIn_BranchID,
                CreatedById = _LoggedIn_UserID,
                CreatedDateTime = DateTime.Now,
            };

            if (assignment.files != null && assignment.files.Count() > 0)
            {
                for (int i = 0; i < assignment.files.Count(); i++)
                {
                    var dbPath = _filesRepository.SaveFile(assignment.files[i]);

                    if (string.IsNullOrEmpty(objToCreate.RelatedMaterial))
                        objToCreate.RelatedMaterial += dbPath;
                    else
                        objToCreate.RelatedMaterial = objToCreate.RelatedMaterial + "||" + dbPath;
                    if (string.IsNullOrEmpty(objToCreate.FileName))
                        objToCreate.FileName = objToCreate.FileName + assignment.files[i].FileName + ",," + dbPath;
                    else
                        objToCreate.FileName = objToCreate.FileName + "||" + assignment.files[i].FileName + ",," + dbPath;
                }
            }
            await _context.ClassSectionAssignment.AddAsync(objToCreate);
            await _context.SaveChangesAsync();
            List<Notification> NotificationsToAdd = new List<Notification>();
            var ToUsers = (from csUser in _context.ClassSectionUsers
                           join u in _context.Users
                           on csUser.UserId equals u.Id
                           where csUser.ClassSectionId == objToCreate.ClassSectionId
                           && csUser.SchoolBranchId == _LoggedIn_BranchID
                           && u.Role == Enumm.UserType.Student.ToString()
                           select csUser.UserId).ToList();
            foreach (var UserId in ToUsers)
            {
                NotificationsToAdd.Add(new Notification
                {
                    Description = GenericFunctions.NotificationDescription(new string[] {
                        "Assignment:",
                        "You have been assigned a new assignment",
                        SubjectObj?.Name,
                        DueDateTime.ToShortDateString()
                    }, UserObj?.FullName),
                    CreatedById = _LoggedIn_UserID,
                    CreatedDateTime = DateTime.Now,
                    IsRead = false,
                    UserIdTo = UserId
                });
            }
            await _context.Notifications.AddRangeAsync(NotificationsToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> EditAssignment(int id, AssignmentDtoForEdit assignment)
        {
            ClassSectionAssignment dbObj = _context.ClassSectionAssignment.FirstOrDefault(s => s.Id.Equals(id));
            if (dbObj != null)
            {
                DateTime DueDateTime = DateTime.ParseExact(assignment.DueDateTime, "MM/dd/yyyy", null);
                var UserObj = _context.Users.Where(m => m.Id == _LoggedIn_UserID).FirstOrDefault();
                var SubjectObj = _context.Subjects.Where(m => m.Id == assignment.SubjectId).FirstOrDefault();
                var AssignmentName = $"{DateTime.Now.ToShortDateString()} - {UserObj?.FullName} - {SubjectObj?.Name}";
                dbObj.AssignmentName = AssignmentName;
                dbObj.Details = assignment.Details;
                dbObj.ClassSectionId = assignment.ClassSectionId;
                dbObj.ReferenceUrl = assignment.ReferenceUrl;
                dbObj.IsPosted = assignment.IsPosted;
                dbObj.DueDateTime = DueDateTime;
                dbObj.SubjectId = assignment.SubjectId;

                if (assignment.files != null && assignment.files.Count() > 0)
                {
                    for (int i = 0; i < assignment.files.Count(); i++)
                    {
                        var dbPath = _filesRepository.SaveFile(assignment.files[i]);
                        if (string.IsNullOrEmpty(dbObj.RelatedMaterial))
                            dbObj.RelatedMaterial = dbObj.RelatedMaterial + dbPath;
                        else
                            dbObj.RelatedMaterial = dbObj.RelatedMaterial + "||" + dbPath;
                        if (string.IsNullOrEmpty(dbObj.FileName))
                            dbObj.FileName = dbObj.FileName + assignment.files[i].FileName + ",," + dbPath;
                        else
                            dbObj.FileName = dbObj.FileName + "||" + assignment.files[i].FileName + ",," + dbPath;
                    }
                }
                _context.ClassSectionAssignment.Update(dbObj);
                await _context.SaveChangesAsync();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> DeleteDoc(string Path, string fileName)
        {
            var RelatedMaterial = fileName.Split(",,")[1];
            var dbObj = await _context.ClassSectionAssignment.Where(m => m.RelatedMaterial.Contains(RelatedMaterial)).FirstOrDefaultAsync();
            if (dbObj != null)
            {
                string newRelatedMaterial = dbObj.RelatedMaterial.Remove(dbObj.RelatedMaterial.IndexOf(RelatedMaterial), RelatedMaterial.Length);
                string newFileName = dbObj.FileName.Remove(dbObj.FileName.IndexOf(fileName), fileName.Length);
                dbObj.RelatedMaterial = newRelatedMaterial;
                dbObj.FileName = newFileName;
                _context.ClassSectionAssignment.Update(dbObj);
                await _context.SaveChangesAsync();
                FileInfo file = new FileInfo(Path);
                if (file.Exists)
                {
                    file.Delete();
                }
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.FileDeleted;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> SubmitAssignment(SubmitAssignmentDtoForAdd model)
        {
            var objToCreate = new ClassSectionAssigmentSubmission
            {
                ClassSectionAssignmentId = model.AssignmentId,
                Description = model.Description,
                StudentId = _LoggedIn_UserID,
                CreatedDatetime = DateTime.Now,
            };

            if (model.files != null && model.files.Count() > 0)
            {
                for (int i = 0; i < model.files.Count(); i++)
                {
                    var dbPath = _filesRepository.SaveFile(model.files[i]);
                    if (string.IsNullOrEmpty(objToCreate.SubmittedMaterial))
                        objToCreate.SubmittedMaterial = objToCreate.SubmittedMaterial + dbPath;
                    else
                        objToCreate.SubmittedMaterial = objToCreate.SubmittedMaterial + "||" + dbPath;
                }
            }
            await _context.ClassSectionAssigmentSubmissions.AddAsync(objToCreate);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
    }
}
