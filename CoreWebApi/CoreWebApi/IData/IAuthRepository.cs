﻿using CoreWebApi.Dtos;
using CoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.IData
{
   public interface IAuthRepository
    {
        Task<ServiceResponse<object>> Register(UserForRegisterDto user, string regNo);

        Task<User> Login(string username, string password, int schoolBranchId);
        Task<object> GetSchoolDetails(string regNo, int branchId);
        Task<bool> UserExists(string  userName, string  schoolName);

    }
}

