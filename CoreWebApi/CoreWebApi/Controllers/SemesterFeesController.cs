﻿using AutoMapper;
using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CoreWebApi.Controllers
{
    [Authorize(Roles = "Admin,Teacher,Student")]
    public class SemesterFeesController : BaseController
    {
        private readonly ISemesterFeeRepository _repo;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        private int _LoggedIn_UserID = 0;
        private int _LoggedIn_BranchID = 0;
        private string _LoggedIn_UserName = "";
        public SemesterFeesController(ISemesterFeeRepository repo, IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repo = repo;
            _context = context;
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
        }


        [HttpPost("AddSemester")]
        public async Task<IActionResult> AddSemester(SemesterDtoForAdd model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.SemesterExists(model.Name))
                return BadRequest(new { message = "This semester is already exist" });

            _response = await _repo.AddSemester(model);
            return Ok(_response);

        }
        [HttpPut("UpdateSemester")]
        public async Task<IActionResult> UpdateSemester(SemesterDtoForEdit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSemester(model);
            return Ok(_response);

        }
        [HttpGet("GetSemester")]
        public async Task<IActionResult> GetSemester()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSemester();
            return Ok(_response);

        }
        [HttpGet("GetSemesterById/{id}")]
        public async Task<IActionResult> GetSemesterById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSemesterById(id);
            return Ok(_response);

        }
        [HttpPut("PostSemester")]
        public async Task<IActionResult> PostSemester(SemesterDtoForPost model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.PostSemester(model);
            return Ok(_response);

        }

        [HttpPost("AddSemesterFeeMapping")]
        public async Task<IActionResult> AddSemesterFeeMapping(SemesterFeeMappingDtoForAdd model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddSemesterFeeMapping(model);
            return Ok(_response);

        }
        [HttpPut("UpdateSemesterFeeMapping")]
        public async Task<IActionResult> UpdateSemesterFeeMapping(SemesterFeeMappingDtoForEdit model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.UpdateSemesterFeeMapping(model);
            return Ok(_response);

        }
        [HttpGet("GetSemesterFeeMapping")]
        public async Task<IActionResult> GetSemesterFeeMapping()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSemesterFeeMapping();
            return Ok(_response);

        }
        [HttpGet("GetSemesterFeeMappingById/{id}")]
        public async Task<IActionResult> GetSemesterFeeMappingById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.GetSemesterFeeMappingById(id);
            return Ok(_response);

        }
        //[HttpGet("SearchStudentsBySemesterClassId/{semId}/{classId}")]
        //public async Task<IActionResult> SearchStudentsBySemesterClassId(int semId, int classId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    _response = await _repo.SearchStudentsBySemesterClassId(semId,classId);
        //    return Ok(_response);

        //}
        [HttpPost("AddFeeVoucherDetails")]
        public async Task<IActionResult> AddFeeVoucherDetails(FeeVoucherDetailForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddFeeVoucherDetails(model);

            return Ok(_response);
        }
        [HttpPut("UpdateFeeVoucherDetails")]
        public async Task<IActionResult> UpdateFeeVoucherDetails(FeeVoucherDetailForUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.UpdateFeeVoucherDetails(model);

            return Ok(_response);
        }
        [HttpGet("GetFeeVoucherDetails")]
        public async Task<IActionResult> GetFeeVoucherDetails()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetFeeVoucherDetails();

            return Ok(_response);
        }
        [HttpGet("GetFeeVoucherDetailsById/{id}")]
        public async Task<IActionResult> GetFeeVoucherDetailsById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetFeeVoucherDetailsById(id);

            return Ok(_response);
        }

        [HttpPut("GenerateFeeVoucher/{bankAccountId}/{semesterId}")]
        public async Task<IActionResult> GenerateFeeVoucher(int bankAccountId, int semesterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GenerateFeeVoucher(bankAccountId, semesterId);

            return Ok(_response);
        }
        [HttpGet("GetStudentsBySemester/{id}")]
        public async Task<IActionResult> GetStudentsBySemester(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetStudentsBySemester(id);

            return Ok(_response);
        }

        [HttpGet("GetAllBankAccount")]
        public async Task<IActionResult> GetAllBankAccount()
        {
            _response = await _repo.GetAllBankAccount();
            return Ok(_response);
        }

        [HttpGet("GetBankAccountById/{id}")]
        public async Task<IActionResult> GetBankAccountById(int id)
        {
            _response = await _repo.GetBankAccountById(id);
            return Ok(_response);
        }

        [HttpPost("AddBankAccount")]
        public async Task<IActionResult> AddBankAccount(BankAccountForAddDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.AddBankAccount(model);

            return Ok(_response);

        }

        [HttpPut("UpdateBankAccount/{id}")]
        public async Task<IActionResult> UpdateBankAccount(int id, BankAccountForUpdateDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.UpdateBankAccount(id, model);

            return Ok(_response);

        }
        [HttpDelete("DeleteBankAccount/{id}")]
        public async Task<IActionResult> DeleteBankAccount(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _response = await _repo.DeleteBankAccount(id);
            return Ok(_response);
        }
        [HttpGet("GetGeneratedFeeVouchers")]
        public async Task<IActionResult> GetGeneratedFeeVouchers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetGeneratedFeeVouchers();

            return Ok(_response);
        }
        [HttpGet("GetGeneratedFeeVoucherById/{id}")]
        public async Task<IActionResult> GetGeneratedFeeVoucherById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _response = await _repo.GetGeneratedFeeVoucherById(id);

            return Ok(_response);
        }
    }
}
