﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class LookupsController : BaseController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILookupRepository _repo;
        public LookupsController(DataContext context, IMapper mapper, ILookupRepository lookupRepository)
        {
            _context = context;
            _mapper = mapper;
            _repo = lookupRepository;
        }
        [HttpGet("UserTypes")]
        public async Task<IActionResult> GetUserTypes()
        {
            _response = await _repo.GetUserTypes();

            return Ok(_response);

        }
        [HttpGet("QuestionTypes")]
        public async Task<IActionResult> GetQuestionTypes()
        {
            _response = await _repo.GetQuestionTypes();

            return Ok(_response);

        }
        [HttpGet("ClassSections")]
        public async Task<IActionResult> GetClassSections()
        {
            _response = await _repo.GetClassSections();


            return Ok(_response);

        }
        [AllowAnonymous]
        [HttpGet("Classes")]
        public async Task<IActionResult> GetClasses()
        {
            _response = await _repo.GetClasses();


            return Ok(_response);

        }
        [HttpGet("Sections")]
        public async Task<IActionResult> GetSections()
        {
            _response = await _repo.GetSections();


            return Ok(_response);

        }
        [HttpGet("Subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            _response = await _repo.GetSubjects();


            return Ok(_response);

        }
        [AllowAnonymous]
        [HttpGet("Cities/{stateId}")]
        public async Task<IActionResult> Cities(int stateId)
        {
            _response = await _repo.GetCities(stateId);
            return Ok(_response);

        }
        [AllowAnonymous]
        [HttpGet("States/{countryId}")]
        public async Task<IActionResult> GetStates(int countryId)
        {
            _response = await _repo.GetStates(countryId);
            return Ok(_response);

        }
        [AllowAnonymous]
        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            _response = await _repo.GetCountries();
            return Ok(_response);

        }
        [HttpGet("Users/{csId}")] // for students
        public async Task<IActionResult> GetUsersByClassSection(int csId)
        {
            _response = await _repo.GetUsersByClassSection(csId);

            return Ok(_response);

        }
        [HttpGet("Teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            _response = await _repo.GetTeachers();

            return Ok(_response);

        }


        [HttpGet("SchoolAcademies")]
        public IActionResult GetSchoolAcademies()
        {


            _response = _repo.GetSchoolAcademies();


            return Ok(_response);


        }
        [HttpGet("SchoolBranches")]
        public IActionResult SchoolBranches()
        {
            _response = _repo.SchoolBranches();
            return Ok(_response);
        }

        [HttpGet("Assignments")]
        public IActionResult Assignments()
        {
            _response = _repo.Assignments();
            return Ok(_response);
        }

        [HttpGet("Quiz")]
        public IActionResult Quiz()
        {
            _response = _repo.Quizzes();
            return Ok(_response);
        }
        [HttpGet("SubjectsByClassSection/{classSectionId}")]
        public async Task<IActionResult> GetSubjectsByClassSection(int classSectionId = 0)
        {
            _response = await _repo.GetSubjectsByClassSection(classSectionId);


            return Ok(_response);

        }
        [HttpGet("TeachersByClassSection/{classSectionId}/{subjectId?}")]
        public async Task<IActionResult> GetTeachersByClassSection(int classSectionId, int subjectId = 0)
        {
            _response = await _repo.GetTeachersByClassSection(classSectionId, subjectId);

            return Ok(_response);

        }
        [HttpGet("SubjectsByClass/{classId}")]
        public async Task<IActionResult> GetSubjectsByClass(int classId)
        {
            _response = await _repo.GetSubjectsByClass(classId);

            return Ok(_response);

        }
        [HttpGet("LeaveTypes")]
        public async Task<IActionResult> GetLeaveTypes()
        {
            _response = await _repo.GetLeaveTypes();

            return Ok(_response);
        }
        [HttpGet("Employees")]
        public async Task<IActionResult> GetEmployees()
        {
            _response = await _repo.GetEmployees();

            return Ok(_response);
        }
        [HttpGet("Semesters")]
        public async Task<IActionResult> GetSemesters()
        {
            _response = await _repo.GetSemesters();

            return Ok(_response);
        }
        [HttpGet("ExamTypes")]
        public async Task<IActionResult> GetExamTypes()
        {
            _response = await _repo.GetExamTypes();

            return Ok(_response);
        }
        [HttpGet("SemesterSections")]
        public async Task<IActionResult> GetSemesterSections()
        {
            _response = await _repo.GetSemesterSections();


            return Ok(_response);

        }
        [HttpGet("SubjectsBySemesterSection/{classSectionId}")]
        public async Task<IActionResult> GetSubjectsBySemesterSection(int classSectionId = 0)
        {
            _response = await _repo.GetSubjectsBySemesterSection(classSectionId);


            return Ok(_response);

        }
        [HttpGet("TeachersBySemesterSection/{classSectionId}/{subjectId?}")]
        public async Task<IActionResult> GetTeachersBySemesterSection(int classSectionId, int subjectId = 0)
        {
            _response = await _repo.GetTeachersBySemesterSection(classSectionId, subjectId);

            return Ok(_response);

        }
        [HttpGet("SubjectsBySemester/{semesterId}")]
        public async Task<IActionResult> GetGetSubjectsBySemester(int semesterId)
        {
            _response = await _repo.GetSubjectsBySemester(semesterId);

            return Ok(_response);

        }
        [HttpGet("GetTeacherSemestersAndSubjectsBySemester/{semesterSectionId?}")]
        public async Task<IActionResult> GetTeacherSemestersAndSubjectsBySemester(int semesterSectionId = 0)
        {
            _response = await _repo.GetTeacherSemestersAndSubjectsBySemester(semesterSectionId);

            return Ok(_response);

        }
        [HttpGet("GetTutorClassesAndSubjects")]
        public async Task<IActionResult> GetTutorClassesAndSubjects()
        {
            _response = await _repo.GetTutorClassesAndSubjects();
            return Ok(_response);
        }
        [HttpGet("GetTutorStudents/{subjectId?}")]
        public async Task<IActionResult> GetTutorStudents(int subjectId = 0)
        {
            _response = await _repo.GetTutorStudents(subjectId);
            return Ok(_response);
        }

        [HttpGet("GetNotifyToTypes")]
        public IActionResult GetNotifyToTypes()
        {
            _response = _repo.GetNotifyToTypes();
            return Ok(_response);
        }
    }
}
