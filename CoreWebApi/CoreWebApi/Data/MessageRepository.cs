using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.Hubs;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
            if (!string.IsNullOrEmpty(_LoggedIn_UserRole))
            {
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
                                 UserIds = new List<int>() { u.Id },
                                 Names = u.FullName,
                                 UserName = u.Username,
                                 Description = cs.Class.Name + " " + cs.Section.SectionName,
                                 Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Url = _fileRepo.AppendImagePath(x.Name)
                                 }).ToList(),
                             }).Distinct().ToList();

                    Users.AddRange((from u in _context.Users
                                    where u.UserTypeId == (int)Enumm.UserType.Admin
                                    && u.SchoolBranchId == _LoggedIn_BranchID
                                    && u.Id != _LoggedIn_UserID
                                    select new ChatUserForListDto
                                    {
                                        UserIds = new List<int>() { u.Id },
                                        Names = u.FullName,
                                        UserName = u.Username,
                                        Description = "",
                                        Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Url = _fileRepo.AppendImagePath(x.Name)
                                        }).ToList(),
                                    }).Distinct().ToList());

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
                                                   Classs = cs.Class.Name,
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
                                       UserIds = new List<int>() { u.Id },
                                       Names = u.FullName,
                                       UserName = u.Username,
                                       Description = cs.Class.Name + " " + cs.Section.SectionName,
                                       Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           Url = _fileRepo.AppendImagePath(x.Name)
                                       }).ToList(),
                                   }).Distinct().ToListAsync();

                    Users.AddRange((from u in _context.Users
                                    join csU in _context.ClassSectionUsers
                                    on u.Id equals csU.UserId

                                    join cs in _context.ClassSections
                                    on csU.ClassSectionId equals cs.Id

                                    where u.UserTypeId == (int)Enumm.UserType.Teacher
                                    && u.SchoolBranchId == _LoggedIn_BranchID
                                    && ClassSections.Select(m => m.ClassSectionId).Contains(csU.ClassSectionId)
                                    && u.Id != _LoggedIn_UserID
                                    select new ChatUserForListDto
                                    {
                                        UserIds = new List<int>() { u.Id },
                                        Names = u.FullName,
                                        UserName = u.Username,
                                        Description = cs.Class.Name + " " + cs.Section.SectionName,
                                        Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Url = _fileRepo.AppendImagePath(x.Name)
                                        }).ToList(),
                                    }).Distinct().ToList());

                    Users.AddRange((from u in _context.Users
                                    where u.UserTypeId == (int)Enumm.UserType.Admin
                                    && u.SchoolBranchId == _LoggedIn_BranchID
                                    select new ChatUserForListDto
                                    {
                                        UserIds = new List<int>() { u.Id },
                                        Names = u.FullName,
                                        UserName = u.Username,
                                        Description = "",
                                        Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Url = _fileRepo.AppendImagePath(x.Name)
                                        }).ToList(),
                                    }).Distinct().ToList());

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
                                                   Classs = cs.Class.Name,
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
                                 UserIds = new List<int>() { u.Id },
                                 Names = u.FullName,
                                 UserName = u.Username,
                                 Description = cs.Class.Name + " " + cs.Section.SectionName,
                                 Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Url = _fileRepo.AppendImagePath(x.Name)
                                 }).ToList(),
                             }).Distinct().ToList();

                    Users.AddRange((from u in _context.Users
                                    join cla in _context.ClassLectureAssignment
                                    on u.Id equals cla.TeacherId

                                    join cs in _context.ClassSections
                                    on cla.ClassSectionId equals cs.Id

                                    where u.UserTypeId == (int)Enumm.UserType.Student
                                    && ClassSections.Select(m => m.ClassSectionId).Contains(cla.ClassSectionId)
                                    && u.SchoolBranchId == _LoggedIn_BranchID
                                    && u.Id != _LoggedIn_UserID
                                    select new ChatUserForListDto
                                    {
                                        UserIds = new List<int>() { u.Id },
                                        UserName = u.Username,
                                        Names = u.FullName,
                                        Description = cs.Class.Name + " " + cs.Section.SectionName,
                                        Photos = _context.Photos.Where(m => m.UserId == u.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Url = _fileRepo.AppendImagePath(x.Name)
                                        }).ToList(),
                                    }).Distinct().ToList());
                }
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.UserNotLoggedIn;
                return _serviceResponse;
            }
            _serviceResponse.Success = true;
            _serviceResponse.Data = Users;
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetChatMessages(List<string> userIds, bool forSignal = false)
        {
            int UserId = Convert.ToInt32(userIds.First());
            List<GroupMessageForListByTimeDto> Messages = new List<GroupMessageForListByTimeDto>();
            if (!forSignal)
            {
                var Users = await _context.Users.Where(m => userIds.Contains(m.Id.ToString())).ToListAsync();
                var UserToDetails = Users.Count > 0 ? _mapper.Map<List<UserForDetailedDto>>(Users) : new List<UserForDetailedDto>();
                var SentMessages = _context.Messages.Where(m => m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserId.Equals(m.MessageToUserId))
                || (UserId.Equals(m.MessageFromUserId) && m.MessageToUserId.Equals(_LoggedIn_UserID))))
                    .Select(o => new GroupMessageForListDto
                    {
                        Id = o.Id,
                        Time = o.CreatedDateTime,
                        TimeToDisplay = DateFormat.ToTime(o.CreatedDateTime.TimeOfDay),
                        MessageFromUserId = o.MessageFromUserId,
                        MessageFromUser = o.MessageFromUser != null ? o.MessageFromUser.FullName : "",
                        Comment = o.Comment,
                        MessageToUserIdsStr = o.MessageToUserId.ToString(),
                        Type = o.MessageFromUserId == _LoggedIn_UserID ? "1" : "2" // 1=Message, 2=Reply
                    }).ToList();

                var DateTimes = _context.Messages.Where(m => m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserId.Equals(m.MessageToUserId))
                || (UserId.Equals(m.MessageFromUserId) && m.MessageToUserId.Equals(_LoggedIn_UserID))))
                    .OrderBy(m => m.CreatedDateTime)
                    .Select(m => DateFormat.ToDateTime(m.CreatedDateTime)).ToList();
                DateTimes = DateTimes.Distinct().ToList();
                for (var i = 0; i < DateTimes.Count(); i++)
                {
                    var item = DateTimes[i];
                    var ToAdd = new GroupMessageForListByTimeDto();
                    DateTime dt = Convert.ToDateTime(item, CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                    if (dt.Date == DateTime.Now.Date)
                        ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == "Today") ? "" : "Today";
                    else if (dt.Date == DateTime.Now.AddDays(-1).Date)
                        ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == "Yesterday") ? "" : "Yesterday";
                    else
                        ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == item) ? "" : item;
                    ToAdd.Messages = SentMessages.Where(m => m.Time.Date == dt.Date && m.Time.Hour == dt.Hour && m.Time.Minute == dt.Minute).OrderBy(m => m.Time).ToList();
                    Messages.Add(ToAdd);
                }

                _serviceResponse.Data = new { UserToDetails, Messages };
            }
            else
            {
                var SingleSentMessage = _context.Messages.Where(m => m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserId.Equals(m.MessageToUserId))
                || (UserId.Equals(m.MessageFromUserId) && m.MessageToUserId.Equals(_LoggedIn_UserID))))
                    .Select(o => new GroupMessageForListDto
                    {
                        Id = o.Id,
                        Time = o.CreatedDateTime,
                        TimeToDisplay = DateFormat.ToTime(o.CreatedDateTime.TimeOfDay),
                        MessageFromUserId = o.MessageFromUserId,
                        MessageFromUser = o.MessageFromUser != null ? o.MessageFromUser.FullName : "",
                        Comment = o.Comment,
                        MessageToUserIdsStr = o.MessageToUserId.ToString(),
                        Type = o.MessageFromUserId == _LoggedIn_UserID ? "1" : "2" // 1=Message, 2=Reply
                    }).OrderByDescending(m => m.Id).FirstOrDefault();

                var ToAdd = new GroupMessageForListByTimeDto();

                ToAdd.Messages.Add(SingleSentMessage);
                Messages.Add(ToAdd);

                _serviceResponse.Data = Messages.FirstOrDefault();

            }
            return _serviceResponse;
        }
        public async Task<ServiceResponse<object>> GetGroupChatMessages(List<string> userIds, int groupId, bool forSignal = false)
        {
            string UserIdds = string.Join(',', userIds);
            List<GroupMessageForListByTimeDto> Messages = new List<GroupMessageForListByTimeDto>();

            if (!forSignal)
            {
                List<UserForDetailedDto> UserToDetails = new List<UserForDetailedDto>();
                var Users = await _context.Users.Where(m => userIds.Contains(m.Id.ToString())).ToListAsync();
                UserToDetails = Users.Count > 0 ? _mapper.Map<List<UserForDetailedDto>>(Users) : new List<UserForDetailedDto>();
                var SentMessages = _context.GroupMessages.Where(m => m.GroupId == groupId && m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserIdds.Contains(m.MessageToUserIds))
                || (UserIdds.Contains(m.MessageFromUserId.ToString()) && m.MessageToUserIds.Contains(_LoggedIn_UserID.ToString()))))
                    .Select(o => new GroupMessageForListDto
                    {
                        Id = o.Id,
                        Time = o.CreatedDateTime,
                        TimeToDisplay = DateFormat.ToTime(o.CreatedDateTime.TimeOfDay),
                        MessageFromUserId = o.MessageFromUserId,
                        MessageFromUser = o.MessageFromUser != null ? o.MessageFromUser.FullName : "",
                        Comment = o.Comment,
                        MessageToUserIdsStr = o.MessageToUserIds,
                        Type = o.MessageFromUserId == _LoggedIn_UserID ? "1" : "2", // 1=Message, 2=Reply
                        GroupId = o.GroupId,
                    }).ToList();

                var DateTimes = _context.GroupMessages.Where(m => m.GroupId == groupId && m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserIdds.Contains(m.MessageToUserIds))
                || (UserIdds.Contains(m.MessageFromUserId.ToString()) && m.MessageToUserIds.Contains(_LoggedIn_UserID.ToString()))))
                    .OrderBy(m => m.CreatedDateTime)
                    .Select(m => DateFormat.ToDateTime(m.CreatedDateTime)).ToList();
                DateTimes = DateTimes.Distinct().ToList();
                for (var i = 0; i < DateTimes.Count(); i++)
                {
                    var item = DateTimes[i];
                    var ToAdd = new GroupMessageForListByTimeDto();
                    DateTime dt = Convert.ToDateTime(item, CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                    if (dt.Date == DateTime.Now.Date)
                        ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == "Today") ? "" : "Today";
                    else if (dt.Date == DateTime.Now.AddDays(-1).Date)
                        ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == "Yesterday") ? "" : "Yesterday";
                    else
                        ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == item) ? "" : item;
                    ToAdd.Messages = SentMessages.Where(m => m.Time.Date == dt.Date && m.Time.Hour == dt.Hour && m.Time.Minute == dt.Minute).OrderBy(m => m.Time).ToList();
                    Messages.Add(ToAdd);
                }

                _serviceResponse.Data = new { UserToDetails, Messages };
            }
            else
            {
                var SingleSentMessage = _context.GroupMessages.Where(m => m.GroupId == groupId && m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserIdds.Contains(m.MessageToUserIds))
                || (UserIdds.Contains(m.MessageFromUserId.ToString()) && m.MessageToUserIds.Contains(_LoggedIn_UserID.ToString()))))
                    .Select(o => new GroupMessageForListDto
                    {
                        Id = o.Id,
                        Time = o.CreatedDateTime,
                        TimeToDisplay = DateFormat.ToTime(o.CreatedDateTime.TimeOfDay),
                        MessageFromUserId = o.MessageFromUserId,
                        MessageFromUser = o.MessageFromUser != null ? o.MessageFromUser.FullName : "",
                        Comment = o.Comment,
                        MessageToUserIdsStr = o.MessageToUserIds,
                        Type = o.MessageFromUserId == _LoggedIn_UserID ? "1" : "2", // 1=Message, 2=Reply
                        GroupId = o.GroupId,
                    }).OrderByDescending(m => m.Id).FirstOrDefault();

                //var SingleDateTime = _context.GroupMessages.Where(m => m.GroupId == groupId && m.CreatedDateTime.Date >= DateTime.Now.AddDays(-7).Date && ((m.MessageFromUserId == _LoggedIn_UserID && UserIdds.Contains(m.MessageToUserIds))
                //|| (UserIdds.Contains(m.MessageFromUserId.ToString()) && m.MessageToUserIds.Contains(_LoggedIn_UserID.ToString()))))
                //    .OrderByDescending(m => m.CreatedDateTime)
                //    .Select(m => DateFormat.ToDateTime(m.CreatedDateTime)).FirstOrDefault();
                //DateTimes = DateTimes.Distinct().ToList();

                //var item = SingalDateTime;
                var ToAdd = new GroupMessageForListByTimeDto();
                //DateTime dt = Convert.ToDateTime(item, CultureInfo.GetCultureInfo("ur-PK").DateTimeFormat);
                //if (dt.Date == DateTime.Now.Date)
                //    ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == "Today") ? "" : "Today";
                //else if (dt.Date == DateTime.Now.AddDays(-1).Date)
                //    ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == "Yesterday") ? "" : "Yesterday";
                //else
                //    ToAdd.TimeToDisplay = Messages.Any(m => m.TimeToDisplay == item) ? "" : item;
                ToAdd.Messages.Add(SingleSentMessage);
                Messages.Add(ToAdd);

                _serviceResponse.Data = Messages.FirstOrDefault();

            }
            _serviceResponse.Success = true;
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
        public async Task<ServiceResponse<object>> SendGroupMessage(GroupMessageForAddDto model)
        {
            var ToAdd = new GroupMessage
            {
                Comment = model.Comment,
                MessageToUserIds = string.Join(',', model.MessageToUserIds),
                IsRead = false,
                CreatedDateTime = DateTime.Now,
                MessageFromUserId = _LoggedIn_UserID,
                MessageReplyId = model.MessageReplyId,
                GroupId = model.GroupId,
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
            await _context.GroupMessages.AddAsync(ToAdd);
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

        public async Task<ServiceResponse<object>> AddChatGroup(ChatGroupForAddDto model)
        {
            model.UserIds.Add(_LoggedIn_UserID);
            var ToAdd = new ChatGroup
            {
                GroupName = model.GroupName,
                UserIds = string.Join(',', model.UserIds),
                CreatedDateTime = DateTime.Now,
                CreatedById = _LoggedIn_UserID
            };
            await _context.ChatGroups.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetChatGroups()
        {
            var ToReturn = await _context.ChatGroups.Where(m => m.UserIds.Contains(_LoggedIn_UserID.ToString())).Select(o => new ChatGroupForListDto //m.CreatedById == _LoggedIn_UserID || 
            {
                Id = o.Id,
                GroupName = o.GroupName,
                UserIdsStr = o.UserIds,
            }).ToListAsync();
            foreach (var item in ToReturn)
            {
                item.UserIds = item.UserIdsStr.Split(',').ToList();
                var UserNames = item.UserIds.Select(o => new
                {
                    Name = _context.Users.FirstOrDefault(m => m.Id == Convert.ToInt32(o)).FullName,
                }).Select(m => m.Name).ToList();
                item.Names = string.Join(',', UserNames);
            }
            _serviceResponse.Success = true;
            _serviceResponse.Data = ToReturn;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> SendTextMessage()
        {
            string MyUsername = "923328316624"; //Your Username At Sendpk.com 
            string MyPassword = "123456"; //Your Password At Sendpk.com 
            string toNumber = "923328316624"; //Recepient cell phone number with country code 
            string Masking = "Hacker"; //Your Company Brand Name 
            string MessageText = "hello paa g. Ki hal a";
            string jsonResponse = SendSMS(Masking, toNumber, MessageText, MyUsername, MyPassword);
            Console.Write(jsonResponse);
            //Console.Read(); //to keep console window open if trying in visual studio 
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
        public static string SendSMS(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            String URI = "http://sendpk.com" +
            "/api/sms.php?" +
            "username=" + MyUsername +
            "&password=" + MyPassword +
            "&sender=" + Masking +
            "&mobile=" + toNumber +
            "&message=" + Uri.UnescapeDataString(MessageText); // Visual Studio 10-15 
            //"//&message=" + System.Net.WebUtility.UrlEncode(MessageText);// Visual Studio 12 
            try
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return "404:URL not found :" + URI;
                            break;
                        case HttpStatusCode.BadRequest:
                            return "400:Bad Request";
                            break;
                        default:
                            return httpWebResponse.StatusCode.ToString();
                    }
                }
            }
            return null;
        }
    }
}

