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
        public string Attachment { get; set; }
    }
    public class ReplyForAddDto
    {
        public int MessageId { get; set; }
        public int ReplyToUserId { get; set; }
        public string Reply { get; set; }
        public int ReplyFromUserId { get; set; }
        public string Attachment { get; set; }
    }
}
