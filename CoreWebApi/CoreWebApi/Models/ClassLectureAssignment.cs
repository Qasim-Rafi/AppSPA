using CoreWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Models
{
    public class ClassLectureAssignment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Key, Column(Order = 0)]
        public int LectureId { get; set; }
        [Key, Column(Order = 1)] 
        public int TeacherId { get; set; }
        public int ClassSectionId { get; set; }
        public int SubjectId { get; set; }       
        public DateTime Date { get; set; }

        [ForeignKey("LectureId")]
        public virtual LectureTiming LectureTiming { get; set; }
        [ForeignKey("TeacherId")]
        public virtual User User { get; set; }
        [ForeignKey("ClassSectionId")]
        public virtual ClassSection ClassSection { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
    
}
