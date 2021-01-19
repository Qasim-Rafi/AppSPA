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
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IFilesRepository _fileRepo;
        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly IMapper _mapper;
        ServiceResponse<object> _serviceResponse;
        public MessageRepository(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository filesRepository)
        {
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _mapper = mapper;
            _fileRepo = filesRepository;
            _serviceResponse = new ServiceResponse<object>();
        }

        public async Task<ServiceResponse<object>> GetUsersForChat()
        {
            List<ChatUserForListDto> Users = new List<ChatUserForListDto>();
            if (_LoggedIn_UserRole == Enumm.UserType.Admin.ToString())
            {
                Users = (from u in _context.Users
                         join csU in _context.ClassSectionUsers
                         on u.Id equals csU.UserId

                         join cs in _context.ClassSections
                         on csU.ClassSectionId equals cs.Id

                         where u.UserTypeId == (int)Enumm.UserType.Teacher
                         && u.SchoolBranchId == _LoggedIn_BranchID
                         select new ChatUserForListDto
                         {
                             UserId = u.Id,
                             UserName = u.FullName,
                             Description = cs.Class.Name + " " + cs.Section.SectionName,
                             Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Url = _fileRepo.AppendImagePath(x.Name)
                             }).ToList(),
                         }).ToList();
            }
            else if (_LoggedIn_UserRole == Enumm.UserType.Teacher.ToString())
            {
                var ClassSections = await (from cla in _context.ClassLectureAssignment
                                           join cs in _context.ClassSections
                                           on cla.ClassSectionId equals cs.Id
                                           where cla.TeacherId == _LoggedIn_UserID
                                           select new ClassSectionForResultListDto
                                           {
                                               ClassSectionId = cs.Id,
                                               Class = cs.Class.Name,
                                               Section = cs.Section.SectionName
                                           }).Distinct().ToListAsync();
                Users = await (from u in _context.Users
                               join csU in _context.ClassSectionUsers
                               on u.Id equals csU.UserId

                               join cs in _context.ClassSections
                               on csU.ClassSectionId equals cs.Id

                               where ClassSections.Select(m => m.ClassSectionId).Contains(csU.ClassSectionId)
                               && u.UserTypeId == (int)Enumm.UserType.Student
                               select new ChatUserForListDto
                               {
                                   UserId = u.Id,
                                   UserName = u.FullName,
                                   Description = cs.Class.Name + " " + cs.Section.SectionName,
                                   Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       Url = _fileRepo.AppendImagePath(x.Name)
                                   }).ToList(),
                               }).Distinct().ToListAsync();

            }
            else if (_LoggedIn_UserRole == Enumm.UserType.Student.ToString())
            {
                var ClassSections = await (from u in _context.Users
                                           join csU in _context.ClassSectionUsers
                                           on u.Id equals csU.UserId

                                           join cs in _context.ClassSections
                                           on csU.ClassSectionId equals cs.Id

                                           where csU.UserId == _LoggedIn_UserID
                                           select new ClassSectionForResultListDto
                                           {
                                               ClassSectionId = cs.Id,
                                               Class = cs.Class.Name,
                                               Section = cs.Section.SectionName
                                           }).Distinct().ToListAsync();
                Users = (from u in _context.Users
                         join cla in _context.ClassLectureAssignment
                         on u.Id equals cla.TeacherId

                         join cs in _context.ClassSections
                         on cla.ClassSectionId equals cs.Id

                         where u.UserTypeId == (int)Enumm.UserType.Teacher
                         && ClassSections.Select(m => m.ClassSectionId).Contains(cla.ClassSectionId)
                         && u.SchoolBranchId == _LoggedIn_BranchID
                         select new ChatUserForListDto
                         {
                             UserId = u.Id,
                             UserName = u.FullName,
                             Description = cs.Class.Name + " " + cs.Section.SectionName,
                             Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                             {
                                 Id = x.Id,
                                 Name = x.Name,
                                 Url = _fileRepo.AppendImagePath(x.Name)
                             }).ToList(),
                         }).ToList();
            }
            _serviceResponse.Success = true;
            _serviceResponse.Data = Users;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetChatMessages(int userId)
        {
            var ChatMessages = await (from m in _context.Messages
                                          //join r in _context.MessageReplies
                                          //on m.Id equals r.MessageId

                                      where m.MessageFromUserId == _LoggedIn_UserID
                                      && m.MessageToUserId == userId
                                      select new MessageForListDto
                                      {
                                          Id = m.Id,
                                          MessageFromUserId = m.MessageFromUserId,
                                          MessageFromUser = m.MessageFromUser.FullName,
                                          Comment = m.Comment,
                                          MessageToUserId = m.MessageToUserId,
                                          MessageToUser = m.MessageToUser.FullName
                                      }).ToListAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Data = ChatMessages;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> SendMessage(MessageForAddDto model)
        {
            var ToAdd = new Message
            {
                Comment = model.Comment,
                MessageToUserId = model.MessageToUserId,
                IsRead = false,
                CreatedDateTime = DateTime.Now,
                MessageFromUserId = _LoggedIn_UserID,
                MessageReplyId = model.MessageReplyId,
            };
            if (model.files != null && model.files.Count() > 0)
            {
                for (int i = 0; i < model.files.Count(); i++)
                {
                    var dbPath = _fileRepo.SaveFile(model.files[i]);
                    if (string.IsNullOrEmpty(ToAdd.Attachment))
                        ToAdd.Attachment += dbPath;
                    else
                        ToAdd.Attachment = ToAdd.Attachment + "||" + dbPath;
                }
            }
            await _context.Messages.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> SendReply(ReplyForAddDto model)
        {
            var ToAdd = new MessageReply
            {
                Reply = model.Reply,
                ReplyToUserId = model.ReplyToUserId,
                IsRead = false,
                CreatedDateTime = DateTime.Now,
                ReplyFromUserId = _LoggedIn_UserID,
                MessageId = model.MessageId,
            };
            if (model.files != null && model.files.Count() > 0)
            {
                for (int i = 0; i < model.files.Count(); i++)
                {
                    var dbPath = _fileRepo.SaveFile(model.files[i]);
                    if (string.IsNullOrEmpty(ToAdd.Attachment))
                        ToAdd.Attachment += dbPath;
                    else
                        ToAdd.Attachment = ToAdd.Attachment + "||" + dbPath;
                }
            }
            await _context.MessageReplies.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }


    }
}
