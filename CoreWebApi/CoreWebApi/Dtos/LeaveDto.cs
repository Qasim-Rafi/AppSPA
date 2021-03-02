using CoreWebApi.Helpers;
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
    public class LeaveDtoForAdd : BaseDto
    {
        [StringLength(250, ErrorMessage = "Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LeaveTypeId { get; set; }
        //public string UserId { get; set; }
    }
    public class LeaveDtoForEdit
    {
        public int Id { get; set; }
        [StringLength(250, ErrorMessage = "Details cannot be longer then 250 characters")]
        public string Details { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LeaveTypeId { get; set; }
        //public string UserId { get; set; }
    }
    public class LeaveDtoForList
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
    }
    public class LeaveDtoForDetail
    {
        public string Details { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
    }
    public class LeaveDtoForApprove
    {
        public int LeaveId { get; set; }
        public string Status { get; set; }
    }
}
