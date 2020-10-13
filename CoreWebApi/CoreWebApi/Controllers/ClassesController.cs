﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CoreWebApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public ClassesController(IClassRepository repo, IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _repo.GetClasses();
            var ToReturn = _mapper.Map<IEnumerable<Class>>(classes);
            return Ok(ToReturn);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            var @class = await _repo.GetClass(id);
            var ToReturn = _mapper.Map<Class>(@class);
            return Ok(ToReturn);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post(ClassDtoForAdd @class)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _repo.ClassExists(@class.Name))
                    return BadRequest(new { message = "Class Already Exist" });

                var createdObj = await _repo.AddClass(@class);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ClassDtoForEdit @class)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var updatedObj = await _repo.EditClass(id, @class);

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });

            }
        }

        [HttpGet("GetClassSectionMapping")]
        public async Task<IActionResult> GetClassSectionMapping()
        {
            var list = await _repo.GetClassSectionMapping();

            var ToReturn = list.Select(o => new
            {
                ClassSectionId = o.Id,
                o.ClassId,
                ClassName = _context.Class.FirstOrDefault(m => m.Id == o.ClassId)?.Name,
                o.SectionId,
                SectionName = _context.Sections.FirstOrDefault(m => m.Id == o.SectionId)?.SectionName,
            });
            return Ok(ToReturn);

        }
        [HttpPost("AddClassSectionMapping")]
        public async Task<IActionResult> AddClassSection(ClassSectionDtoForAdd classSection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
                //    return BadRequest(new { message = "Class Section Already Exist" });

                var createdObj = await _repo.AddClassSectionMapping(classSection);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }

        //[HttpPut("UpdateClassSectionMapping")]
        //public async Task<IActionResult> UpdateClassSectionMapping(ClassSectionDtoForUpdate classSection)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
        //        //    return BadRequest(new { message = "Class Section Already Exist" });

        //        var createdObj = await _repo.UpdateClassSectionMapping(classSection);

        //        return StatusCode(StatusCodes.Status201Created);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(new
        //        {
        //            message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
        //        });
        //    }
        //}

        [HttpPost("AddClassSectionUserMappingInBulk")]
        public async Task<IActionResult> AddClassSectionUserMappingInBulk(ClassSectionUserDtoForAddBulk classSectionUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
                //    return BadRequest(new { message = "Class Section Already Exist" });

                var createdObj = await _repo.AddClassSectionUserMappingBulk(classSectionUser);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpPost("AddClassSectionUserMapping")]
        public async Task<IActionResult> AddClassSectionUser(ClassSectionUserDtoForAdd classSectionUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
                //    return BadRequest(new { message = "Class Section Already Exist" });

                var createdObj = await _repo.AddClassSectionUserMapping(classSectionUser);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
                });
            }
        }
        [HttpGet("GetClassSectionUserMapping/{csId}/{userId}")]
        public async Task<IActionResult> GetClassSectionUserMappingById(int csId, int userId)
        {
            var ToReturn = await _repo.GetClassSectionUserMappingById(csId, userId);


            return Ok(ToReturn);

        }
        //[HttpPut("UpdateClassSectionUserMapping")]
        //public async Task<IActionResult> UpdateClassSectionUserMapping(ClassSectionUserDtoForAdd classSectionUser)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        //if (await _repo.ClassSectionExists(classSection.ClassId, classSection.SectionId))
        //        //    return BadRequest(new { message = "Class Section Already Exist" });

        //        var createdObj = await _repo.UpdateClassSectionUserMapping(classSectionUser);

        //        return StatusCode(StatusCodes.Status201Created);
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest(new
        //        {
        //            message = ex.Message == "" ? ex.InnerException.ToString() : ex.Message
        //        });
        //    }
        //}
    }
}
