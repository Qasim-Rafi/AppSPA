using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreWebApi.Hubs
{
    public class ChatHub : Hub
    {
        //private readonly IFilesRepository _fileRepo;
        //private readonly IMapper _mapper;
        //private int _LoggedIn_UserID = 0;
        //private DataContext _context;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public ChatHub(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IFilesRepository filesRepository)
        //{
        //    _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
        //    _fileRepo = filesRepository;
        //    _context = context;
        //    _mapper = mapper;
        //    _httpContextAccessor = httpContextAccessor;
        //}       
        //public async Task SendMessage(int messageToUserId, string comment, int messageReplyId, string file, IFormFileCollection files)
        //{
        //    var ToAdd = new Message
        //    {
        //        Comment = comment,
        //        MessageToUserId = messageToUserId,
        //        IsRead = false,
        //        CreatedDateTime = DateTime.Now,
        //        MessageFromUserId = _LoggedIn_UserID,
        //        MessageReplyId = messageReplyId,
        //    };
        //    if (files != null && files.Count() > 0)
        //    {
        //        for (int i = 0; i < files.Count(); i++)
        //        {
        //            var dbPath = _fileRepo.SaveFile(files[i]);
        //            if (string.IsNullOrEmpty(ToAdd.Attachment))
        //                ToAdd.Attachment += dbPath;
        //            else
        //                ToAdd.Attachment = ToAdd.Attachment + "||" + dbPath;
        //        }
        //    }
        //    await _context.Messages.AddAsync(ToAdd);
        //    var ok = await _context.SaveChangesAsync();
        //    MessageRepository _repo = new MessageRepository(_context, _httpContextAccessor, _mapper, _fileRepo);
        //    var userToIds = new List<string>() { messageToUserId.ToString() };
        //    var response = await _repo.GetChatMessages(userToIds, true);
        //    if (ok == 1)
        //    {
        //        var lastMessageStr = JsonConvert.SerializeObject(response.Data);
        //        var lastMessage = JsonConvert.DeserializeObject<GroupMessageForListByTimeDto>(lastMessageStr);
        //        var ToReturn = new GroupSignalRMessageForListDto
        //        {
        //            Id = lastMessage.Messages[0].Id,
        //            Type = lastMessage.Messages[0].Type,
        //            DateTimeToDisplay = lastMessage.TimeToDisplay,
        //            TimeToDisplay = lastMessage.Messages[0].TimeToDisplay,
        //            Comment = lastMessage.Messages[0].Comment,
        //            MessageFromUserId = lastMessage.Messages[0].MessageFromUserId,
        //            MessageFromUser = lastMessage.Messages[0].MessageFromUser,
        //            MessageToUserIdsStr = lastMessage.Messages[0].MessageToUserIdsStr,
        //            GroupId = 0,
        //            //MessageToUser = lastMessage.Messages[0].MessageToUser,
        //        };

        //        // List<MessageForListByTimeDto> collection = new List<MessageForListByTimeDto>((IEnumerable<MessageForListByTimeDto>)lastMessage.Data);

        //        await Clients.Others.SendAsync("MessageNotificationAlert", ToReturn);
        //        //_hubContext.Clients.Clients(ReceiverConnectionids)
        //    }
        //}
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                try
                {
                    //Add Logged User
                    var userName = httpContext.Request.Query["user"].ToString();
                    //var UserAgent = httpContext.Request.Headers["User-Agent"].FirstOrDefault().ToString();
                    var connId = Context.ConnectionId.ToString();
                    ConnectionMapping<string>.Add(userName, connId);

                }
                catch (Exception) { }
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                //Remove Logged User
                var username = httpContext.Request.Query["user"];
                ConnectionMapping<string>.Remove(username, Context.ConnectionId);

            }

            //return base.OnDisconnectedAsync(exception);
        }
    }
}
