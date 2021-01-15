using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class MessageReply
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int ReplyToUserId { get; set; }
        public string Reply { get; set; }
        public int ReplyFromUserId { get; set; }
        public string Attachment { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("ReplyFromUserId")]
        public virtual User ReplyFromUser { get; set; }
        [ForeignKey("ReplyToUserId")]
        public virtual User ReplyToUser { get; set; }
        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; }
    }
}
