using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreWebApi.Models
{
    public class Photo
    {
        public int Id { get; set; }
        //[NotMapped]
        public string Url { get; set; }
        public string  Description { get; set; }
        public DateTime CreatedDatetime { get; set; }
        //[Column("IsMain")]
        public bool IsPrimary { get; set; }
        //public User User{ get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}