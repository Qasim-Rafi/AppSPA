using CoreWebApi.Data;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi_Tests
{
    public class AuthRepositoryFake : BaseRepository, IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFilesRepository _fileRepo;
        public AuthRepositoryFake(DataContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, EmailSettings emailSettings, IWebHostEnvironment webHostEnvironment, IFilesRepository filesRepository)
         : base(context, httpContextAccessor)
        {
            _configuration = configuration;
            _emailSettings = emailSettings;
            _webHostEnvironment = webHostEnvironment;
            _fileRepo = filesRepository;
        }

        public async Task<User> Login(string username, string password, int schoolBranchId)
        {
            if (schoolBranchId > 0 && !string.IsNullOrEmpty(schoolBranchId.ToString()))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower() && x.Active == true && x.SchoolBranchId == schoolBranchId);
                if (user == null)
                    return null;

                if (!Seed.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                return user;
            }
            else
            {
                var branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower() && x.Active == true && x.SchoolBranchId == branch.Id);
                if (user == null)
                    return null;

                if (!Seed.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    return null;

                return user;
            }

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
                                               schoolExamType = branch.ExamType,
                                               logo = _fileRepo.AppendMultiDocPath(school.Logo),
                                               branch
                                           }).FirstOrDefaultAsync();
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
                                               schoolExamType = branch.ExamType,
                                               logo = _fileRepo.AppendMultiDocPath(school.Logo),
                                               branch
                                           }).FirstOrDefaultAsync();
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

            if (model.UserTypeSignUp != null && model.UserTypeSignUp.ToLower() == "school")
            {
                if (!string.IsNullOrEmpty(model.SchoolName))
                {
                    var SchoolExist = _context.SchoolAcademy.Where(m => m.Name.ToLower() == model.SchoolName.ToLower()).FirstOrDefault();
                    if (SchoolExist != null)
                    {
                        _serviceResponse.Success = false;
                        _serviceResponse.Message = CustomMessage.RecordAlreadyExist;
                        return _serviceResponse;
                    }
                }
                var schools = _context.SchoolAcademy.OrderByDescending(m => m.Id).ToList();
                var schoolAcademy = new SchoolAcademy
                {
                    Name = string.IsNullOrEmpty(model.SchoolName) ? "School-" + (schools.Count() + 1) : model.SchoolName,
                    PrimaryContactPerson = model.Username,
                    SecondaryContactPerson = model.Username,
                    PrimaryphoneNumber = "---",
                    SecondaryphoneNumber = "---",
                    Email = model.Email,
                    PrimaryAddress = "---",
                    SecondaryAddress = "---",
                    Active = true
                };
                if (model.files != null && model.files.Count() > 0)
                {
                    for (int i = 0; i < model.files.Count(); i++)
                    {
                        var dbPath = _fileRepo.SaveFile(model.files[i]);

                        if (string.IsNullOrEmpty(schoolAcademy.Logo))
                            schoolAcademy.Logo += dbPath;
                        else
                            schoolAcademy.Logo = schoolAcademy.Logo + "||" + dbPath;
                    }
                }
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
                        RegistrationNumber = branches.Count() == 1 ? "10000000" : (Convert.ToInt32(branches.FirstOrDefault().RegistrationNumber) + 1).ToString(),
                        ExamType = model.ExamType,
                    };

                    _context.AddRange(schoolBranhes);
                    _context.SaveChanges();
                    branch = await _context.SchoolBranch.OrderByDescending(m => m.Id).FirstOrDefaultAsync();

                }
                var userToCreate = new User
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    UserTypeId = (int)Enumm.UserType.Admin,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
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
            else if (model.UserTypeSignUp != null && model.UserTypeSignUp.ToLower() == "tutor")
            {
                branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();
                var userToCreate = new User
                {
                    Username = model.Username,
                    FullName = model.Username,
                    UserTypeId = (int)Enumm.UserType.Tutor,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    SchoolBranchId = branch.Id,
                    Gender = model.Gender,
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
            else if (model.UserTypeSignUp != null && model.UserTypeSignUp.ToLower() == "student")
            {
                branch = await _context.SchoolBranch.Where(m => m.BranchName == "ONLINE ACADEMY").FirstOrDefaultAsync();
                var userToCreate = new User
                {
                    Username = model.Username,
                    FullName = model.Username,
                    UserTypeId = (int)Enumm.UserType.OnlineStudent,
                    Email = model.Email,
                    ContactNumber = model.ContactNumber,
                    SchoolBranchId = branch.Id,
                    Gender = model.Gender,
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

        public async Task<ServiceResponse<object>> UploadFile(UploadFileDto model)
        {
            try
            {
                //string contentRootPath = _HostEnvironment.ContentRootPath;


                var pathToSave = "http://localhost:8000/images";


                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                //var fullPath = Path.Combine(pathToSave);
                //var dbPath = Path.Combine(pathToSave, fileName); //you can add this path to a list and then return all dbPaths to the client if require
                //if (!Directory.Exists(fullPath))
                //{
                //    //If Directory (Folder) does not exists. Create it.
                //    //Directory.CreateDirectory(fullPath);
                //}
                //var filePath = Path.Combine(pathToSave, fileName);
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                var filePath = Path.Combine(contentRootPath, "SchoolDocuments", fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }
                return _serviceResponse;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ServiceResponse<object>> ForgotPassword(ForgotPasswordDto user)
        {
            var User = _context.Users.Where(m => m.Email == user.Email && m.Active == true).FirstOrDefault();
            if (User != null)
            {
                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailSettings.Sender));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Forgot Password?";
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var isDevelopment = environment == Environments.Development;
                string resetLink = "";
                if (isDevelopment)
                    resetLink = "http://localhost:4200/resetpassword";
                else
                    resetLink = "https://e-learningbox.com/resetpassword";
                email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>You have requested reset password.</h1> Here is the link is to reset your password: {resetLink}" };

                // send email
                using var smtp = new SmtpClient();
                // gmail
                smtp.Connect(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls); //587           
                //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettings.UserName, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                _serviceResponse.Message = CustomMessage.ResetPasswordReqSent;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> ResetPassword(ResetPasswordDto model)
        {
            var User = _context.Users.Where(m => m.Email == model.Email && m.Active == true).FirstOrDefault();
            if (User != null)
            {
                byte[] passwordHash, passwordSalt;
                Seed.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
                User.PasswordHash = passwordHash;
                User.PasswordSalt = passwordSalt;
                _context.Users.Update(User);
                await _context.SaveChangesAsync();

                _serviceResponse.Message = CustomMessage.Updated;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
                return _serviceResponse;
            }
        }

        public Task<User> ExStudentLogin(string username, string password, int tutorId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<object>> ExStudentRegister(ExStudentForRegisterDto model)
        {
            throw new NotImplementedException();
        }
    }
}

