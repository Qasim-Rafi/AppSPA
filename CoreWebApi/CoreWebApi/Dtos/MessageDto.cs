using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class MessageDto
    {
    }
    public class MessageForAddDto
    {
        public int MessageToUserId { get; set; }
        public string Comment { get; set; }
        public int MessageFromUserId { get; set; }
        public int? MessageReplyId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class ReplyForAddDto
    {
        public int MessageId { get; set; }
        public int ReplyToUserId { get; set; }
        public string Reply { get; set; }
        public int ReplyFromUserId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class MessageForListDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public int MessageToUserId { get; set; }
        public string MessageToUser { get; set; }
        public string Comment { get; set; }
        public int MessageFromUserId { get; set; }
        public string MessageFromUser { get; set; }
        public int? MessageReplyId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class MessageForListByTimeDto
    {
        public DateTime Time { get; set; }
        public string TimeToDisplay { get; set; }
        public List<MessageForListDto> Messages { get; set; } = new List<MessageForListDto>();
    }
    public class ChatUserForListDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }
}
