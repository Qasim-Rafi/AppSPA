using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
    public interface IMessageRepository
    {
        Task<ServiceResponse<object>> GetUsersForChat();
        Task<ServiceResponse<object>> GetChatMessages(int userId);
        Task<ServiceResponse<object>> SendMessage(MessageForAddDto model);
        Task<ServiceResponse<object>> SendReply(ReplyForAddDto model);
    }
}
