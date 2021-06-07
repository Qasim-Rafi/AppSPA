using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ChatMessageAttachment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string AttachmentPath { get; set; }

        [ForeignKey("MessageId")]
        public virtual Message MessageObj { get; set; }
    }
}
