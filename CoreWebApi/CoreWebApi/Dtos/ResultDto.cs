using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Dtos
{
    public class ResultDto
    {
    }
    public class ResultForAddDto
    {
        public int ClassSectionId { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public int ReferenceId { get; set; }
        public string Remarks { get; set; }
        public double TotalMarks { get; set; }
        public double ObtainedMarks { get; set; }
    }
    public class ResultForListDto
    {
        public int ClassSectionId { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public int ReferenceId { get; set; }
        public string Remarks { get; set; }
        public double TotalMarks { get; set; }
        public double ObtainedMarks { get; set; }
    }
    public class ClassSectionForResultListDto
    {
        public int ClassSectionId { get; set; }
        public string Classs { get; set; }
        public string Section { get; set; }
    }
    public class ExamForResultListDto
    {
        public int RefId { get; set; }
        public string RefName { get; set; }
    }
    public class StudentForResultListDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int ClassSectionId { get; set; }
    }
    public class SubjectForResultListDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassSectionId { get; set; }
    }
}
