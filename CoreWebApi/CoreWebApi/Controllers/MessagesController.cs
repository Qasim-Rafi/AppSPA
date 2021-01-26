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

namespace CoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        private readonly IHubContext<MessageNotificationHub> _hubContext;
        private int _LoggedIn_UserID = 0;
        public MessagesController(IMessageRepository repo, IMapper mapper, IHubContext<MessageNotificationHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
            _hubContext = hubContext;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
        }
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] MessageForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SendMessage(model);
            var userToIds = new List<string>() { model.MessageToUserId.ToString() };
            var response = await _repo.GetChatMessages(userToIds, true);
            if (_response.Success)
            {
                var lastMessageStr = JsonConvert.SerializeObject(response.Data);
                var lastMessage = JsonConvert.DeserializeObject<GroupMessageForListByTimeDto>(lastMessageStr);
                var ToReturn = new GroupSignalRMessageForListDto
                {
                    Id = lastMessage.Messages.Last().Id,
                    Type = lastMessage.Messages.Last().Type,
                    DateTimeToDisplay = lastMessage.TimeToDisplay,
                    TimeToDisplay = lastMessage.Messages.Last().TimeToDisplay,
                    Comment = lastMessage.Messages.Last().Comment,
                    MessageFromUserId = lastMessage.Messages.Last().MessageFromUserId,
                    MessageFromUser = lastMessage.Messages.Last().MessageFromUser,
                    MessageToUserIdsStr = lastMessage.Messages.Last().MessageToUserIdsStr,
                    //MessageToUser = lastMessage.Messages.Last().MessageToUser,
                };

                // List<MessageForListByTimeDto> collection = new List<MessageForListByTimeDto>((IEnumerable<MessageForListByTimeDto>)lastMessage.Data);

                await _hubContext.Clients.All.SendAsync("MessageNotificationAlert", ToReturn);
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
                    Id = lastMessage.Messages.Last().Id,
                    Type = lastMessage.Messages.Last().Type,
                    DateTimeToDisplay = lastMessage.TimeToDisplay,
                    TimeToDisplay = lastMessage.Messages.Last().TimeToDisplay,
                    Comment = lastMessage.Messages.Last().Comment,
                    MessageFromUserId = lastMessage.Messages.Last().MessageFromUserId,
                    MessageFromUser = lastMessage.Messages.Last().MessageFromUser,
                    MessageToUserIdsStr = lastMessage.Messages.Last().MessageToUserIdsStr,
                    //MessageToUser = lastMessage.Messages.Last().MessageToUser,
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
    }
}
