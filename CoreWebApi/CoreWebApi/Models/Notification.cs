using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ClassId{ get; set; }
        [StringLength(200,ErrorMessage ="Message length cannot be longer then 200 characters.")]
        public string message { get; set; }


        public virtual Class Class { get; set; }
    }
}
