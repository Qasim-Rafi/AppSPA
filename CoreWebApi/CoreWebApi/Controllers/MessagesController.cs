﻿using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Hubs;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MessagesController(IMessageRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] MessageForAddDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.SendMessage(model);
            var msgs = await _repo.GetChatMessages(model.MessageToUserId);
            if (_response.Success)
            {
                await _hubContext.Clients.All.SendAsync("MessageNotificationAlert", msgs.Data);
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

            _response = await _repo.GetChatMessages(userId);

            return Ok(_response);

        }
    }
}
