using System;

namespace CoreWebApi.Models
{
    public class TutorStudentMapping
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public bool Active { get; set; }
    }
}
