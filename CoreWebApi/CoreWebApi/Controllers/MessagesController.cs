using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.Hubs;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class MessagesController : BaseController
    {
        private readonly IMessageRepository _repo;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;
        private int _LoggedIn_UserID = 0;
        private readonly IFilesRepository _filesRepository;
        //private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        public MessagesController(IMessageRepository repo, IMapper mapper, IHubContext<ChatHub> hubContext, IHttpContextAccessor httpContextAccessor, IFilesRepository filesRepository)
        {
            _mapper = mapper;
            _repo = repo;
            _hubContext = hubContext;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _filesRepository = filesRepository;
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] MessageForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //List<string> ReceiverConnectionids = _connections.GetConnections(model.MessageToUserId.ToString()).ToList<string>();

            _response = await _repo.SendMessage(model);
            var userToIds = new List<string>() { model.MessageToUserId.ToString() };
            var response = await _repo.GetChatMessages(userToIds, true);
            if (_response.Success)
            {
                var lastMessageStr = JsonConvert.SerializeObject(response.Data);
                var lastMessage = JsonConvert.DeserializeObject<GroupMessageForListByTimeDto>(lastMessageStr);
                var ToReturn = new GroupSignalRMessageForListDto
                {
                    Id = lastMessage.Messages[0].Id,
                    Type = lastMessage.Messages[0].Type,
                    DateTimeToDisplay = lastMessage.Messages[0].TimeToDisplay,
                    TimeToDisplay = lastMessage.Messages[0].TimeToDisplay,
                    Comment = lastMessage.Messages[0].Comment,
                    MessageFromUserId = lastMessage.Messages[0].MessageFromUserId,
                    MessageFromUser = lastMessage.Messages[0].MessageFromUser,
                    MessageToUserIdsStr = lastMessage.Messages[0].MessageToUserIdsStr,
                    GroupId = 0,
                    Attachment = lastMessage.Messages[0].Attachment,
                    FileName = lastMessage.Messages[0].FileName,
                    FileType = lastMessage.Messages[0].FileType,

                    //MessageToUser = lastMessage.Messages[0].MessageToUser,
                };

                // List<MessageForListByTimeDto> collection = new List<MessageForListByTimeDto>((IEnumerable<MessageForListByTimeDto>)lastMessage.Data);

                await _hubContext.Clients.All.SendAsync("MessageNotificationAlert", ToReturn);
                //_hubContext.Clients.Clients(ReceiverConnectionids)
            }

            return Ok(_response);

        }
        [HttpPost("SendGroupMessage")]
        public async Task<IActionResult> SendGroupMessage([FromForm] GroupMessageForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SendGroupMessage(model);

            var response = await _repo.GetGroupChatMessages(model.MessageToUserIds, model.GroupId, true);
            if (_response.Success)
            {
                var lastMessageStr = JsonConvert.SerializeObject(response.Data);
                var lastMessage = JsonConvert.DeserializeObject<GroupMessageForListByTimeDto>(lastMessageStr);
                var ToReturn = new GroupSignalRMessageForListDto
                {
                    Id = lastMessage.Messages[0].Id,
                    Type = lastMessage.Messages[0].Type,
                    DateTimeToDisplay = lastMessage.Messages[0].TimeToDisplay,
                    TimeToDisplay = lastMessage.Messages[0].TimeToDisplay,
                    Comment = lastMessage.Messages[0].Comment,
                    MessageFromUserId = lastMessage.Messages[0].MessageFromUserId,
                    MessageFromUser = lastMessage.Messages[0].MessageFromUser,
                    MessageToUserIdsStr = lastMessage.Messages[0].MessageToUserIdsStr,
                    GroupId = lastMessage.Messages[0].GroupId,
                    Attachment = lastMessage.Messages[0].Attachment,
                    FileName = lastMessage.Messages[0].FileName,
                    FileType = lastMessage.Messages[0].FileType,
                    //MessageToUser = lastMessage.Messages[0].MessageToUser,
                };

                // List<MessageForListByTimeDto> collection = new List<MessageForListByTimeDto>((IEnumerable<MessageForListByTimeDto>)lastMessage.Data);

                await _hubContext.Clients.All.SendAsync("MessageNotificationAlert", ToReturn);
            }

            return Ok(_response);

        }
        [HttpPost("SendReply")]
        public async Task<IActionResult> SendReply([FromForm] ReplyForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SendReply(model);

            return Ok(_response);

        }
        [HttpGet("GetUsersForChat")]
        public async Task<IActionResult> GetUsersForChat()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetUsersForChat();

            return Ok(_response);

        }
        //[HttpGet("GetChatMessages/{userId}")] // not in use
        //public async Task<IActionResult> GetChatMessages(int userId)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _response = await _repo.GetChatMessages(userId, false);

        //    return Ok(_response);

        //}
        [HttpPost("GetGroupChatMessages")]
        public async Task<IActionResult> GetGroupChatMessages(GroupMessageForParamDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model.groupId > 0)
                _response = await _repo.GetGroupChatMessages(model.userIds, model.groupId, false);
            else
                _response = await _repo.GetChatMessages(model.userIds, false);
            return Ok(_response);

        }
        [HttpPost("AddChatGroup")]
        public async Task<IActionResult> AddChatGroup(ChatGroupForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddChatGroup(model);

            return Ok(_response);

        }
        [HttpGet("GetChatGroup")]
        public async Task<IActionResult> GetChatGroup()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetChatGroups();

            return Ok(_response);

        }
        [HttpGet("SendTextMessage")]
        public async Task<IActionResult> SendTextMessage()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SendTextMessage();

            return Ok(_response);

        }
    }
}
