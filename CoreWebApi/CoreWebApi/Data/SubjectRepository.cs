using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly DataContext _context;
        ServiceResponse<object> _serviceResponse;
        private readonly IMapper _mapper;
        public int _LoggedIn_UserID = 0;
        public int _LoggedIn_BranchID = 0;
        public string _LoggedIn_UserName = "";
        public SubjectRepository(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }


        public async Task<bool> SubjectExists(string name)
        {
            if (await _context.Subjects.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.SchoolBranchId == _LoggedIn_BranchID))
                return true;
            return false;
        }
        public async Task<ServiceResponse<object>> GetSubject(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(u => u.Id == id);
            if (subject != null)
            {
                _serviceResponse.Data = _mapper.Map<SubjectDtoForDetail>(subject);
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> GetSubjects()
        {
            var branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();
            if (branch.Id == _LoggedIn_BranchID)
            {
                var subjects = await _context.Subjects.Where(m => m.CreatedBy == _LoggedIn_UserID && m.SchoolBranchId == branch.Id).ToListAsync();// m.Active == true &&
                _serviceResponse.Data = _mapper.Map<IEnumerable<SubjectDtoForDetail>>(subjects);
            }
            else
            {
                var subjects = await _context.Subjects.Where(m => m.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();// m.Active == true &&
                _serviceResponse.Data = _mapper.Map<IEnumerable<SubjectDtoForDetail>>(subjects);
            }
            _serviceResponse.Success = true;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetAssignedSubject(int id)
        {

            var subject = await (from s in _context.Subjects
                                 join ass in _context.SubjectAssignments
                                 on s.Id equals ass.SubjectId
                                 join c in _context.Class
                                 on ass.ClassId equals c.Id
                                 join sch in _context.SchoolBranch
                                 on ass.SchoolBranchId equals sch.Id
                                 where ass.Id == id
                                 && s.Active == true
                                 && s.SchoolBranchId == _LoggedIn_BranchID
                                 select new AssignSubjectDtoForDetail
                                 {
                                     Id = s.Id,
                                     ClassId = ass.ClassId,
                                     ClassName = c.Name,
                                     SchoolId = ass.SchoolBranchId,
                                     SchoolName = sch.BranchName,
                                     //TableOfContent = ass.TableOfContent,
                                 }).FirstOrDefaultAsync();

            if (subject != null)
            {
                var childrens = await (from s in _context.Subjects
                                       join ass in _context.SubjectAssignments
                                       on s.Id equals ass.SubjectId
                                       where ass.ClassId == subject.ClassId
                                       && s.Active == true
                                       && s.SchoolBranchId == _LoggedIn_BranchID
                                       select s).Select(x => new SubjectDtoForDetail
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                       }).ToListAsync();
                subject.Children.AddRange(childrens);

                _serviceResponse.Data = subject;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> GetAssignedSubjects()
        {
            var subjects = (from ass in _context.SubjectAssignments
                            join c in _context.Class
                            on ass.ClassId equals c.Id
                            join sch in _context.SchoolBranch
                            on ass.SchoolBranchId equals sch.Id
                            where ass.SchoolBranchId == _LoggedIn_BranchID
                            && c.Active == true
                            && sch.Active == true
                            select new
                            {
                                ClassId = c.Id,
                                ClassName = c.Name,
                                SchoolBranchId = sch.Id,
                                SchoolName = sch.BranchName,
                                //TableOfContent = ass.TableOfContent,
                            }).Distinct().ToList().Select(o => new AssignSubjectDtoForList
                            {
                                Id = _context.SubjectAssignments.FirstOrDefault(m => m.ClassId == o.ClassId).Id,
                                ClassId = o.ClassId,
                                ClassName = o.ClassName,
                                SchoolId = o.SchoolBranchId,
                                SchoolName = o.SchoolName,
                                //TableOfContent = o.TableOfContent,
                            }).ToList();


            foreach (var item in subjects)
            {
                var childrens = await (from s in _context.Subjects
                                       join ass in _context.SubjectAssignments
                                       on s.Id equals ass.SubjectId
                                       where ass.ClassId == item.ClassId
                                       && s.Active == true
                                       && s.SchoolBranchId == _LoggedIn_BranchID
                                       select s).Select(x => new ChipsDto
                                       {
                                           Value = x.Id,
                                           Display = x.Name,
                                       }).ToListAsync();
                item.Children.AddRange(childrens);
            }
            _serviceResponse.Data = subjects;
            _serviceResponse.Success = true;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> AddSubject(SubjectDtoForAdd model)
        {
            try
            {
                var ListToAdd = new List<Subject>();
                var subject = model;
                if (!await SubjectExists(subject.Name))
                {
                    ListToAdd.Add(new Subject
                    {
                        Name = subject.Name,
                        Active = true,
                        CreditHours = subject.CreditHours,
                        CreatedBy = _LoggedIn_UserID,
                        CreatedDateTime = DateTime.Now,
                        SchoolBranchId = _LoggedIn_BranchID,
                    });
                    await _context.Subjects.AddRangeAsync(ListToAdd);
                    await _context.SaveChangesAsync();

                    _serviceResponse.Message = CustomMessage.Added;
                    _serviceResponse.Success = true;
                }
                else
                {
                    _serviceResponse.Message = CustomMessage.RecordAlreadyExist;
                    _serviceResponse.Success = false;
                }
                return _serviceResponse;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
                return _serviceResponse;
            }

        }
        public async Task<ServiceResponse<object>> AssignSubjects(AssignSubjectDtoForAdd model)
        {
            try
            {
                var ListToAdd = new List<SubjectAssignment>();
                foreach (var SubjectId in model.SubjectIds)
                {
                    ListToAdd.Add(new SubjectAssignment
                    {
                        SubjectId = SubjectId,
                        ClassId = model.ClassId,
                        SchoolBranchId = _LoggedIn_BranchID,
                        //TableOfContent = model.TableOfContent,
                        CreatedById = _LoggedIn_UserID,
                        CreatedDateTime = DateTime.Now
                    });
                }

                await _context.SubjectAssignments.AddRangeAsync(ListToAdd);
                await _context.SaveChangesAsync();

                _serviceResponse.Message = CustomMessage.Added;
                _serviceResponse.Success = true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> EditSubject(SubjectDtoForEdit subject)
        {
            try
            {
                Subject checkExist = _context.Subjects.FirstOrDefault(s => s.Name.ToLower() == subject.Name.ToLower() && s.SchoolBranchId == _LoggedIn_BranchID);
                if (checkExist != null && checkExist.Id != subject.Id)
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.RecordAlreadyExist;
                    return _serviceResponse;
                }
                Subject ObjToUpdate = _context.Subjects.FirstOrDefault(s => s.Id.Equals(subject.Id));
                if (ObjToUpdate != null)
                {
                    ObjToUpdate.Name = subject.Name;
                    ObjToUpdate.CreditHours = subject.CreditHours;
                    ObjToUpdate.Active = subject.Active;
                    _context.Subjects.Update(ObjToUpdate);
                    await _context.SaveChangesAsync();
                }
                _serviceResponse.Message = CustomMessage.Updated;
                _serviceResponse.Success = true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> EditAssignedSubject(int id, AssignSubjectDtoForEdit model)
        {
            try
            {
                if (model.SubjectIds.Count() > 0)
                {
                    var ToRemove = _context.SubjectAssignments.Where(s => s.ClassId.Equals(model.ClassId)).ToList();
                    if (ToRemove.Count() > 0)
                    {
                        _context.SubjectAssignments.RemoveRange(ToRemove);
                        await _context.SaveChangesAsync();

                        var ListToAdd = new List<SubjectAssignment>();
                        foreach (var SubjectId in model.SubjectIds)
                        {
                            ListToAdd.Add(new SubjectAssignment
                            {
                                SubjectId = SubjectId,
                                ClassId = model.ClassId,
                                SchoolBranchId = _LoggedIn_BranchID,
                                //TableOfContent = model.TableOfContent,
                                CreatedById = _LoggedIn_UserID,
                                CreatedDateTime = DateTime.Now
                            });
                        }

                        await _context.SubjectAssignments.AddRangeAsync(ListToAdd);
                        await _context.SaveChangesAsync();
                        _serviceResponse.Message = CustomMessage.Updated;
                        _serviceResponse.Success = true;
                    }
                    else
                    {
                        _serviceResponse.Message = CustomMessage.RecordNotFound;
                        _serviceResponse.Success = false;
                    }
                    return _serviceResponse;
                }
                else
                {
                    _serviceResponse.Message = CustomMessage.DataNotProvided;
                    _serviceResponse.Success = false;
                    return _serviceResponse;
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    _serviceResponse.Success = false;
                    _serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
                return _serviceResponse;
            }
        }


        public async Task<ServiceResponse<object>> ActiveInActiveSubject(int id, bool status)
        {

            var subject = _context.Subjects.Where(m => m.Id == id).FirstOrDefault();
            subject.Active = status;
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;
            return _serviceResponse;

        }
        public async Task<ServiceResponse<object>> GetAllSubjectContent(int classId, int subjectId)
        {
            //var ToReturn = await _context.SubjectContents
            //    .Select(o => new SubjectContentOneDtoForList
            //    {
            //        ClassId = o.ClassId,
            //        Class = o.Class.Name,
            //        Subjects = _context.SubjectContents.Where(m => m.ClassId == o.ClassId).Select(p => new SubjectContentTwoDtoForList
            //        {
            //            SubjectId = p.SubjectId,
            //            Subject = p.Subject.Name,
            //            Contents = _context.SubjectContents.Where(m => m.SubjectId == p.SubjectId).Select(q => new SubjectContentThreeDtoForList
            //            {
            //                SubjectContentId = q.Id,
            //                Heading = q.Heading,
            //                ContentOrder = q.ContentOrder,
            //                ContentDetails = _context.SubjectContentDetails.Where(m => m.SubjectContentId == q.Id).Select(r => new SubjectContentDetailDtoForList
            //                {
            //                    SubjectContentDetailId = r.Id,
            //                    DetailHeading = r.Heading,
            //                    DetailOrder = r.Order,
            //                }).ToList()
            //            }).ToList()
            //        }).Distinct().ToList()
            //    }).Distinct().ToListAsync();
            //var ToReturn = _context.SubjectContents
            //    .Select(o => new
            //    {
            //        ClassId = o.ClassId,
            //        Class = o.Class.Name,
            //    }).Distinct().Select(p => new SubjectContentOneDtoForList
            //    {
            //        ClassId = p.ClassId,
            //        Class = p.Class,
            //        Subjects = _context.SubjectContents.Where(m => m.ClassId == p.ClassId).Select(p => new
            //        {
            //            SubjectId = p.SubjectId,
            //            Subject = p.Subject.Name,
            //        }).Distinct().Select(q => new SubjectContentTwoDtoForList
            //        {
            //            SubjectId = q.SubjectId,
            //            Subject = q.Subject,
            //            Contents = _context.SubjectContents.Where(m => m.SubjectId == q.SubjectId).Select(q => new
            //            {
            //                SubjectContentId = q.Id,
            //                Heading = q.Heading,
            //                ContentOrder = q.ContentOrder,
            //            }).Distinct().Select(r => new SubjectContentThreeDtoForList
            //            {
            //                SubjectContentId = r.SubjectContentId,
            //                Heading = r.Heading,
            //                ContentOrder = r.ContentOrder,
            //                ContentDetails = _context.SubjectContentDetails.Where(m => m.SubjectContentId == r.SubjectContentId).Select(s => new SubjectContentDetailDtoForList
            //                {
            //                    SubjectContentDetailId = s.Id,
            //                    DetailHeading = s.Heading,
            //                    DetailOrder = s.Order,
            //                }).Distinct().ToList()
            //            }).ToList()
            //        }).ToList()
            //    }).ToList();
            var ClassList = await _context.SubjectContents.Select(o => new SubjectContentOneDtoForList
            {
                ClassId = o.ClassId,
                Class = o.Class.Name,
            }).Distinct().ToListAsync();
            for (int one = 0; one < ClassList.Count(); one++)
            {
                var itemOne = ClassList[one];
                itemOne.Subjects = _context.SubjectContents.Where(m => m.ClassId == itemOne.ClassId).Select(p => new SubjectContentTwoDtoForList
                {
                    SubjectId = p.SubjectId,
                    Subject = p.Subject.Name,
                }).Distinct().ToList();
                for (int two = 0; two < itemOne.Subjects.Count(); two++)
                {
                    var itemTwo = itemOne.Subjects[two];
                    itemTwo.Contents = _context.SubjectContents.Where(m => m.SubjectId == itemTwo.SubjectId).Select(q => new SubjectContentThreeDtoForList
                    {
                        SubjectContentId = q.Id,
                        Heading = q.Heading,
                        ContentOrder = q.ContentOrder,
                    }).Distinct().ToList();
                    for (int three = 0; three < itemTwo.Contents.Count(); three++)
                    {
                        var itemThree = itemTwo.Contents[three];
                        itemThree.ContentDetails = _context.SubjectContentDetails.Where(m => m.SubjectContentId == itemThree.SubjectContentId).Select(r => new SubjectContentDetailDtoForList
                        {
                            SubjectContentDetailId = r.Id,
                            DetailHeading = r.Heading,
                            DetailOrder = r.Order,
                        }).ToList();
                    }
                }
            }
            _serviceResponse.Data = ClassList;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjectContentById(int id)
        {
            var ToReturn = await _context.SubjectContents.Where(m => m.Id == id).FirstOrDefaultAsync();
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubjectContents(List<SubjectContentDtoForAdd> model)
        {
            var ListToAdd = new List<SubjectContent>();
            foreach (var item in model)
            {
                ListToAdd.Add(new SubjectContent
                {
                    Heading = item.Heading,
                    Active = true,
                    ContentOrder = item.ContentOrder,
                    CreatedDateTime = DateTime.Now,
                    SubjectId = item.SubjectId,
                    ClassId = item.ClassId
                });
            }

            await _context.SubjectContents.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateSubjectContent(SubjectContentDtoForEdit model)
        {
            var toUpdate = _context.SubjectContents.Where(m => m.Id == model.Id).FirstOrDefault();
            toUpdate.Heading = model.Heading;
            toUpdate.ContentOrder = model.ContentOrder;
            toUpdate.SubjectId = model.SubjectId;
            toUpdate.ClassId = model.ClassId;

            _context.SubjectContents.Update(toUpdate);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddSubjectContentDetails(List<SubjectContentDetailDtoForAdd> model)
        {
            var ListToAdd = new List<SubjectContentDetail>();
            foreach (var item in model)
            {
                ListToAdd.Add(new SubjectContentDetail
                {
                    Heading = item.Heading,
                    Active = true,
                    Order = item.Order,
                    CreatedDateTime = DateTime.Now,
                    SubjectContentId = item.SubjectContentId
                });
            }

            await _context.SubjectContentDetails.AddRangeAsync(ListToAdd);
            await _context.SaveChangesAsync();

            _serviceResponse.Message = CustomMessage.Added;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetSubjectContentDetailById(int id)
        {
            var ToReturn = await _context.SubjectContentDetails.Where(m => m.Id == id).FirstOrDefaultAsync();
            _serviceResponse.Data = ToReturn;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> UpdateSubjectContentDetail(SubjectContentDetailDtoForEdit model)
        {
            var toUpdate = _context.SubjectContentDetails.Where(m => m.Id == model.Id).FirstOrDefault();
            toUpdate.Heading = model.Heading;
            toUpdate.Order = model.Order;
            toUpdate.SubjectContentId = model.SubjectContentId;

            _context.SubjectContentDetails.Update(toUpdate);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Updated;
            return _serviceResponse;
        }
    }
}
