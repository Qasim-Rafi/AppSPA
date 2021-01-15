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

        public async Task<ServiceResponse<object>> SendMessage(MessageForAddDto model)
        {
            var ToAdd = new Message
            {
                Comment = model.Comment,
                MessageToUserId = model.MessageToUserId,
                IsRead = false,
                CreatedDateTime = DateTime.Now,
                MessageFromUserId = _LoggedIn_UserID
            };

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
                ReplyFromUserId = _LoggedIn_UserID
            };

            await _context.MessageReplies.AddAsync(ToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;
        }
    }
}
