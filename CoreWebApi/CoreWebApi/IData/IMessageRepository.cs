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
        Task<ServiceResponse<object>> GetChatMessages(List<string> userIds, bool forSignal);

        Task<ServiceResponse<object>> GetGroupChatMessages(List<string> userIds, int groupId, bool forSignal);
        Task<ServiceResponse<object>> SendMessage(MessageForAddDto model);
        Task<ServiceResponse<object>> SendGroupMessage(GroupMessageForAddDto model);
        Task<ServiceResponse<object>> SendReply(ReplyForAddDto model);
        Task<ServiceResponse<object>> AddChatGroup(ChatGroupForAddDto model);
        Task<ServiceResponse<object>> GetChatGroups();
    }
}
