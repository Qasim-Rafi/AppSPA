using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string MessageToUserIds { get; set; }
        public string Comment { get; set; }
        public int MessageFromUserId { get; set; }
        public int? MessageReplyId { get; set; }
        public string Attachment { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("MessageFromUserId")]
        public virtual User MessageFromUser { get; set; }
        //[ForeignKey("MessageToUserId")]
        //public virtual User MessageToUser { get; set; }
       
    }
}
