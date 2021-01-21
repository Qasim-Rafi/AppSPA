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
            var response = await _repo.GetChatMessages(model.MessageToUserId, true);
            if (_response.Success)
            {
                var lastMessageStr = JsonConvert.SerializeObject(response.Data);
                var lastMessage = JsonConvert.DeserializeObject<MessageForListByTimeDto>(lastMessageStr);
                var ToReturn = new SignalRMessageForListDto
                {
                    Id = lastMessage.Messages[0].Id,
                    Type = lastMessage.Messages[0].Type,
                    DateTimeToDisplay = lastMessage.TimeToDisplay,
                    TimeToDisplay = lastMessage.Messages[0].TimeToDisplay,
                    Comment = lastMessage.Messages[0].Comment,
                    MessageFromUserId = lastMessage.Messages[0].MessageFromUserId,
                    MessageFromUser = lastMessage.Messages[0].MessageFromUser,
                    MessageToUserId = lastMessage.Messages[0].MessageToUserId,
                    MessageToUser = lastMessage.Messages[0].MessageToUser,
                    IsReceived = false
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
        [HttpGet("GetChatMessages/{userId}")]
        public async Task<IActionResult> GetChatMessages(int userId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetChatMessages(userId, false);

            return Ok(_response);

        }
    }
}
