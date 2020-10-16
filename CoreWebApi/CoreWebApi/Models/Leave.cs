using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        [DateValidation(ErrorMessage = "From Date is not in correct format")]
        public DateTime FromDate { get; set; }
        [DateValidation(ErrorMessage = "To Date is not in correct format")]
        public DateTime ToDate { get; set; }

        public int LeaveApprovalTypeId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("LeaveTypeId")]
        public virtual LeaveType LeaveType { get; set; }

        [ForeignKey("LeaveApprovalTypeId")]
        public virtual  LeaveApprovalType LeaveApprovalType { get; set; }
    }
}
