using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace CoreWebApi.Controllers
{
    //[Authorize(Roles = "Admin,Teacher,Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : BaseController
    {
        private readonly IAssignmentRepository _repo;
        private readonly IMapper _mapper;
        ServiceResponse<object> _response;
        private IFileProvider _fileProvider;
        public AssignmentsController(IAssignmentRepository repo, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFileProvider fileProvider)
            : base(httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _response = new ServiceResponse<object>();
            _fileProvider = fileProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignmentes()
        {
            _response = await _repo.GetAssignments();

            return Ok(_response);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignment(int id)
        {
            _response = await _repo.GetAssignment(id);
            return Ok(_response);
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromForm] AssignmentDtoForAdd assignment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (await _repo.AssignmentExists(assignment.AssignmentName))
            //    return BadRequest(new { message = "Assignment Already Exist" });


            _response = await _repo.AddAssignment(assignment);

            return Ok(_response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AssignmentDtoForEdit assignment)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.EditAssignment(id, assignment);

            return Ok(_response);

        }
        [HttpDelete("DeleteDoc/{docName}")]
        public async Task<IActionResult> DeleteDoc(string docName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var file = _fileProvider.GetFileInfo(docName);
            if (file.Exists)
            {
                var result = ReadTxtContent(file.PhysicalPath, docName);
                if (result == null)
                {
                    _response.Success = false;
                    _response.Message = CustomMessage.RecordNotFound;
                    return Ok(_response);
                }
                _response = await _repo.DeleteDoc(file.PhysicalPath, docName);
                return Ok(_response);
            }
            _response.Success = false;
            _response.Message = CustomMessage.RecordNotFound;
            return Ok(_response);
        }
        [NonAction]
        private FileStreamResult ReadTxtContent(string Path, string fileName)
        {
            if (!System.IO.File.Exists(Path))
            {
                return null;
            }
            var memory = new MemoryStream();
            using (var stream = new FileStream(Path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(Path), fileName);
        }
        [NonAction]
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        [NonAction]
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
