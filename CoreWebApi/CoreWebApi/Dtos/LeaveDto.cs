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
        [Required]
        [DateValidation] 
        public DateTime FromDate { get; set; }
        [Required]
        [DateValidation]
        public DateTime ToDate { get; set; }
    }
    public class LeaveDtoForEdit
    {
        [Required]
        [StringLength(250, ErrorMessage = "Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        [Required]
        [DateValidation(ErrorMessage = "From Date is not in correct format")]
        public DateTime FromDate { get; set; }
        [DateValidation(ErrorMessage = "To Date is not in correct format")]
        [Required] 
        public DateTime ToDate { get; set; }
    }
    public class LeaveDtoForList
    {
    }
    public class LeaveDtoForDetail
    {
    }
}
