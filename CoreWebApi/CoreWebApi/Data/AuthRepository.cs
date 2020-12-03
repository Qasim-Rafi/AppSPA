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
        ServiceResponse<object> _serviceResponse;

        public AuthRepository(DataContext context)
        {
            _context = context;
            _serviceResponse = new ServiceResponse<object>();
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
        public async Task<object> GetSchoolDetails(string regNo, int branchId)
        {
            if (!string.IsNullOrEmpty(regNo))
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
                //regNo = _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefault().RegistrationNumber;
            }
            else
            {
                var schoolDetails = await (from school in _context.SchoolAcademy
                                           join branch in _context.SchoolBranch
                                           on school.Id equals branch.SchoolAcademyID
                                           where branch.Id == branchId
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

        public async Task<ServiceResponse<object>> Register(UserForRegisterDto model, string regNo)
        {
            SchoolBranch branch = null;
            if (model.UserType.ToLower() == "school")
            {
                var schools = _context.SchoolAcademy.OrderByDescending(m => m.Id).ToList();
                var schoolAcademy = new SchoolAcademy
                {
                    Name = string.IsNullOrEmpty(model.SchoolName) ? "School-" + (schools.Count() + 1) : model.SchoolName,
                    PrimaryContactPerson = model.Username,
                    SecondaryContactPerson = model.Username,
                    PrimaryphoneNumber = "0000-0000000",
                    SecondaryphoneNumber = "0000-0000000",
                    Email = model.Email,
                    PrimaryAddress = "---",
                    SecondaryAddress = "---",
                    Active = true
                };

                _context.SchoolAcademy.Add(schoolAcademy);
                _context.SaveChanges();
                int schoolAcademyId = schoolAcademy.Id;

                if (schoolAcademyId > 0)
                {
                    var branches = _context.SchoolBranch.OrderByDescending(m => m.Id).ToList();
                    var schoolBranhes = new SchoolBranch
                    {
                        BranchName = string.IsNullOrEmpty(model.SchoolName) ? "Branch-" + (branches.Count() + 1) : model.SchoolName,
                        SchoolAcademyID = schoolAcademyId,
                        CreatedDateTime = DateTime.Now,
                        Active = true,
                        RegistrationNumber = branches.Count() == 1 ? "10000000" : (Convert.ToInt32(branches.FirstOrDefault().RegistrationNumber) + 1).ToString()
                    };

                    _context.AddRange(schoolBranhes);
                    _context.SaveChanges();
                    branch = await _context.SchoolBranch.OrderByDescending(m => m.Id).FirstOrDefaultAsync();

                }
                var userToCreate = new User
                {
                    Username = model.Username,
                    FullName = model.Username,
                    UserTypeId = (int)Enumm.UserType.Admin,
                    Email = model.Email,
                    SchoolBranchId = branch.Id,
                    Gender = "male",
                    Active = true,
                    CreatedDateTime = DateTime.Now,
                    Role = _context.UserTypes.Where(m => m.Id == (int)Enumm.UserType.Admin).FirstOrDefault()?.Name
                };
                byte[] passwordHash, passwordSalt;
                Seed.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

                userToCreate.PasswordHash = passwordHash;
                userToCreate.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(userToCreate);
                await _context.SaveChangesAsync();
            }
            else if (model.UserType.ToLower() == "tutor")
            {
                branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();
                var userToCreate = new User
                {
                    Username = model.Username,
                    FullName = model.Username,
                    UserTypeId = (int)Enumm.UserType.Tutor,
                    Email = model.Email,
                    SchoolBranchId = branch.Id,
                    Gender = "male",
                    Active = true,
                    CreatedDateTime = DateTime.Now,
                    Role = _context.UserTypes.Where(m => m.Id == (int)Enumm.UserType.Tutor).FirstOrDefault()?.Name
                };
                byte[] passwordHash, passwordSalt;
                Seed.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

                userToCreate.PasswordHash = passwordHash;
                userToCreate.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(userToCreate);
                await _context.SaveChangesAsync();

            }
            else if (model.UserType.ToLower() == "student")
            {
                branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();
                var userToCreate = new User
                {
                    Username = model.Username,
                    FullName = model.Username,
                    UserTypeId = (int)Enumm.UserType.OnlineStudent,
                    Email = model.Email,
                    SchoolBranchId = branch.Id,
                    Gender = "male",
                    Active = true,
                    CreatedDateTime = DateTime.Now,
                    Role = _context.UserTypes.Where(m => m.Id == (int)Enumm.UserType.OnlineStudent).FirstOrDefault()?.Name
                };
                byte[] passwordHash, passwordSalt;
                Seed.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);

                userToCreate.PasswordHash = passwordHash;
                userToCreate.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(userToCreate);
                await _context.SaveChangesAsync();

            }
            //var UserTypes = _context.UserTypes.ToList();

            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;


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

        public async Task<bool> UserExists(string userName, string schoolName)
        {
            if (!string.IsNullOrEmpty(schoolName))
            {
                var schoolNames = _context.SchoolAcademy.Select(m => m.Name.ToLower()).ToList();
                if (await _context.Users.AnyAsync(x => x.Username.ToLower() == userName.ToLower() && schoolNames.Contains(schoolName.ToLower())))
                    return true;
            }
            else if (await _context.Users.AnyAsync(x => x.Username.ToLower() == userName.ToLower()))
                return true;
            return false;
        }
    }
}
