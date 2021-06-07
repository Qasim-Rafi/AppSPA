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
        // public int MessageFromUserId { get; set; }
        public int? MessageReplyId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class GroupMessageForAddDto
    {
        public List<string> MessageToUserIds { get; set; }
        public string Comment { get; set; }
        public int MessageFromUserId { get; set; }
        public int GroupId { get; set; }
        public int? MessageReplyId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class ReplyForAddDto
    {
        public int MessageId { get; set; }
        public int ReplyToUserId { get; set; }
        public string Reply { get; set; }
        // public int ReplyFromUserId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class MessageForListDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string TimeToDisplay { get; set; }
        public string Type { get; set; }
        public int MessageToUserId { get; set; }
        public string MessageToUser { get; set; }
        public string Comment { get; set; }
        public int MessageFromUserId { get; set; }
        public string MessageFromUser { get; set; }
        public int? MessageReplyId { get; set; }
        public IFormFileCollection files { get; set; }
    }
    public class GroupMessageForListDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string TimeToDisplay { get; set; }
        public string Type { get; set; }
        public string MessageToUserIdsStr { get; set; }
        public List<string> MessageToUserIds { get; set; }
        public string MessageToUser { get; set; }
        public string Comment { get; set; }
        public int MessageFromUserId { get; set; }
        public string MessageFromUser { get; set; }
        public int? MessageReplyId { get; set; }
        public string Attachment { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int GroupId { get; set; }
    }
    public class MessageForListByTimeDto
    {
        public string TimeToDisplay { get; set; }
        public List<MessageForListDto> Messages { get; set; } = new List<MessageForListDto>();
    }
    public class GroupMessageForListByTimeDto
    {
        public string TimeToDisplay { get; set; } = "";
        public List<GroupMessageForListDto> Messages { get; set; } = new List<GroupMessageForListDto>();
    }
    public class SignalRMessageForListDto
    {
        public int Id { get; set; }
        public string TimeToDisplay { get; set; }
        public string DateTimeToDisplay { get; set; }
        public string Type { get; set; }
        public int MessageToUserId { get; set; }
        public string MessageToUser { get; set; }
        public string Comment { get; set; }
        public string Attachment { get; set; }
        public int MessageFromUserId { get; set; }
        public string MessageFromUser { get; set; }
        public int? MessageReplyId { get; set; }
    }
    public class GroupSignalRMessageForListDto
    {
        public int Id { get; set; }
        public string TimeToDisplay { get; set; }
        public string DateTimeToDisplay { get; set; }
        public string Type { get; set; }
        public string MessageToUserIdsStr { get; set; }
        public string MessageToUser { get; set; }
        public string Comment { get; set; }
        public string Attachment { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int MessageFromUserId { get; set; }
        public string MessageFromUser { get; set; }
        public int? MessageReplyId { get; set; }
        public int GroupId { get; set; }
    }
    public class ChatUserForListDto
    {
        public List<int> UserIds { get; set; }
        public string Names { get; set; }
        public string UserName { get; set; }       
        public bool IsOnline { get; set; }
        public string Description { get; set; }
        public List<PhotoDto> Photos { get; set; }
    }

    public class ChatGroupForAddDto
    {
        public string GroupName { get; set; }
        public List<int> UserIds { get; set; }
    }
    public class ChatGroupForListDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string UserIdsStr { get; set; }
        public List<string> UserIds { get; set; }
        public string Names { get; set; }
    }
    //public class SingalUserMessageForAddDto
    //{
    //    public List<int> UserIds { get; set; }
    //} 
    public class GroupMessageForParamDto
    {
        public int groupId { get; set; } = 0;
        public List<string> userIds { get; set; }
    }
}
