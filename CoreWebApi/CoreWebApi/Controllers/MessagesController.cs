﻿using AutoMapper;
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
            var response = await _repo.GetChatMessages(model.MessageToUserIds, true);
            if (_response.Success)
            {
                var lastMessageStr = JsonConvert.SerializeObject(response.Data);
                var lastMessage = JsonConvert.DeserializeObject<MessageForListByTimeDto>(lastMessageStr);
                var ToReturn = new SignalRMessageForListDto
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
        [HttpPost("GetChatMessages")]
        public async Task<IActionResult> GetChatMessages(List<string> userIds)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetChatMessages(userIds, false);

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

            _response = await _repo.GetChatGroup();

            return Ok(_response);

        }
    }
}
