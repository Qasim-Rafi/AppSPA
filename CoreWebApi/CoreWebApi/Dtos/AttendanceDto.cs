using CoreWebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class AttendanceDto
    {
    }
    public class AttendanceDtoForAdd
    {
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }

        //[Range(typeof(bool), "true", "false", ErrorMessage = "The field must be true of false")]
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Present { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Absent { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Late { get; set; }
        public string Comments { get; set; }
    }
    public class AttendanceDtoForEdit
    {
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Present { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Absent { get; set; }
        [BoolValidation(ErrorMessage = "The field must be true of false")]
        public bool Late { get; set; }
        public string Comments { get; set; }
    }
    public class AttendanceDtoForList
    {
        public int Id { get; set; }
        public int ClassSectionId { get; set; }
        public int UserId { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool Late { get; set; }
        public string Comments { get; set; }
        //public string LeavePurpose { get; set; }
        //public string LeaveType { get; set; }
        public DateTime CreatedDatetime { get; set; }
        //public DateTime? LeaveFrom { get; set; }
        //public DateTime? LeaveTo { get; set; }
        public int LateCount { get; internal set; }
        public int AbsentCount { get; internal set; }
        public int LeaveCount { get; internal set; }
        public int PresentCount { get; internal set; }
    }
    public class AttendanceDtoForDetail
    {
    }
}
