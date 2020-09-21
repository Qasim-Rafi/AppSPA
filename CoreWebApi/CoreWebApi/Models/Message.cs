﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int MessageToUserId { get; set; }
        public string  Comment { get; set; }
        public int MessageFromUserId  { get; set; }
        public int ReplyMessageId { get; set; }
        public string AttachmentPath { get; set; }

        public virtual User User { get; set; } 

    }
}
