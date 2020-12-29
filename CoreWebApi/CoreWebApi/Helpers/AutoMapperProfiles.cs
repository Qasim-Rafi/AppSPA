using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // user
            CreateMap<User, UserForAddDto>().ReverseMap();
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailedDto>();
            CreateMap<User, UserForAddDto>();
            CreateMap<User, UserForLoginDto>();
            // class
            CreateMap<Class, ClassDtoForAdd>();
            CreateMap<Class, ClassDtoForEdit>();
            CreateMap<Class, ClassDtoForDetail>();
            CreateMap<Class, ClassDtoForList>();
            // section
            CreateMap<Section, SectionDtoForAdd>();
            CreateMap<Section, SectionDtoForEdit>();
            CreateMap<Section, SectionDtoForDetail>();
            CreateMap<Section, SectionDtoForList>();
            // quiz
            CreateMap<Quizzes, QuizDtoForAdd>();
            CreateMap<QuizQuestions, QuizQuestionDtoForAdd>();
            CreateMap<QuizAnswers, QuizAnswerDtoForAdd>();
            CreateMap<Quizzes, QuizDtoForAdd>();
            CreateMap<Quizzes, QuizDtoForLookupList>();
            // subject
            CreateMap<Subject, SubjectDtoForAdd>();
            CreateMap<Subject, SubjectDtoForEdit>();
            CreateMap<Subject, SubjectDtoForDetail>();
            CreateMap<Subject, SubjectDtoForList>();           
            // assginment
            CreateMap<Assignment, AssignmentDtoForAdd>();
            CreateMap<Assignment, AssignmentDtoForEdit>();
            CreateMap<Assignment, AssignmentDtoForDetail>();
            CreateMap<Assignment, AssignmentDtoForList>();        
            CreateMap<Assignment, AssignmentDtoForLookupList>();        
            // attendance
            CreateMap<Attendance, AttendanceDtoForAdd>();
            CreateMap<Attendance, AttendanceDtoForEdit>();
            CreateMap<Attendance, AttendanceDtoForDetail>();
            CreateMap<Attendance, AttendanceDtoForList>();   
            // leave
            CreateMap<Leave, LeaveDtoForAdd>();
            CreateMap<Leave, LeaveDtoForEdit>();
            CreateMap<Leave, LeaveDtoForDetail>();
            CreateMap<Leave, LeaveDtoForList>();
           
        }
    }
}
