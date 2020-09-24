using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class LeaveDto
    {
    }
    public class LeaveDtoForAdd
    {
        [Required]
        [StringLength(250, ErrorMessage = "Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        [DateValidation] 
        public DateTime FromDate { get; set; }
        [DateValidation]
        public DateTime ToDate { get; set; }
    }
    public class LeaveDtoForEdit
    {
        [Required]
        [StringLength(250, ErrorMessage = "Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        [DateValidation]
        public DateTime FromDate { get; set; }
        [DateValidation]
        public DateTime ToDate { get; set; }
    }
    public class LeaveDtoForList
    {
    }
    public class LeaveDtoForDetail
    {
    }
}
