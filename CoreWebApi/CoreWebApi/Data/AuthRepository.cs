using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
            if (user == null)
                return null;

            if (!Seed.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;


        }
        public async Task<object> GetSchoolDetails(string regNo)
        {
            var schoolDetails = await (from school in _context.SchoolAcademy
                                       join branch in _context.SchoolBranch
                                       on school.Id equals branch.SchoolAcademyID
                                       where branch.RegistrationNumber == regNo
                                       select new
                                       {
                                           school,
                                           branch
                                       }
                                ).FirstOrDefaultAsync();
            if (schoolDetails == null)
                return null;

            return schoolDetails;

        }

        //private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        //{
        //    using (HMACSHA512 hmac = new HMACSHA512(passwordSalt))
        //    {
        //       var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        for (int i=0;i<computedHash.Length; i++)
        //        {
        //            if (computedHash[i] != passwordHash[i])
        //                return false;
        //        }

        //        return true;

        //    }
        //}

        public async Task<User> Register(UserForRegisterDto model, string regNo)
        {


            var branch = await _context.SchoolBranch.Where(m => m.RegistrationNumber == regNo).FirstOrDefaultAsync();

            var userToCreate = new User
            {
                Username = model.Username,
                FullName = model.Username,
                UserTypeId = model.UserTypeId,
                Email = model.Email,
                SchoolBranchId = branch.Id,
                Gender = "male",
                Active = true,
                CreatedDateTime = DateTime.Now,
            };
            byte[] passwordHash, passwordSalt;
            Seed.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

            userToCreate.PasswordHash = passwordHash;
            userToCreate.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(userToCreate);
            await _context.SaveChangesAsync();

            return userToCreate;

        }

        //private void CreatePasswordHash(string password,out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    byte[] key= new Byte[64];
        //    using (HMACSHA512 hmac = new HMACSHA512(key))
        //    {
        //        passwordSalt = hmac.Key;
        //        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


        //       // var hmac = System.Security.Cryptography.HMACSHA512()
        //    }

        //}

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
                return true;
            return false;
        }
    }
}
