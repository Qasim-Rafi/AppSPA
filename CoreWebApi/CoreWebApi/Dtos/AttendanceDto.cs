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
        public class AttendanceDtoForAdd
        {
            
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
        }
        public class AttendanceDtoForDetail
        {
        }
    }
}
