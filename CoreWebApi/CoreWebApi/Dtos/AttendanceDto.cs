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
            public bool Present { get; set; }
            public bool Absent { get; set; }
            public bool Late { get; set; }
            public string Comments { get; set; }
        }
        public class AttendanceDtoForEdit
        {
            public bool Present { get; set; }
            public bool Absent { get; set; }
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
