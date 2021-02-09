using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class UserRepository : IUserRepository
    {

        protected readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly IFilesRepository _File;
        ServiceResponse<object> _serviceResponse;
        public int _LoggedIn_UserID = 0;
        public int _LoggedIn_BranchID = 0;
        public string _LoggedIn_UserName = "";
        private string _LoggedIn_UserRole = "";
        private readonly ITeacherRepository _TeacherRepository;
        public UserRepository(DataContext context, IMapper mapper, IWebHostEnvironment HostEnvironment, IFilesRepository file, IHttpContextAccessor httpContextAccessor, ITeacherRepository TeacherRepository, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
            _File = file;
            _serviceResponse = new ServiceResponse<object>();
            _LoggedIn_UserID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.NameIdentifier.ToString()));
            _LoggedIn_BranchID = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.BranchIdentifier.ToString()));
            _LoggedIn_UserName = httpContextAccessor.HttpContext.User.FindFirstValue(Enumm.ClaimType.Name.ToString())?.ToString();
            _LoggedIn_UserRole = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            _TeacherRepository = TeacherRepository;
            _configuration = configuration;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(Task entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<ServiceResponse<UserForDetailedDto>> GetUser(GetByIdFlagDto model)
        {
            ServiceResponse<UserForDetailedDto> serviceResponse = new ServiceResponse<UserForDetailedDto>();
            var GetUser = await (from user in _context.Users
                                 where user.Id == model.Id
                                 select new UserForDetailedDto
                                 {
                                     Id = user.Id,
                                     FullName = user.FullName,
                                     DateofBirth = user.DateofBirth != null ? DateFormat.ToDate(user.DateofBirth.ToString()) : "",
                                     Email = user.Email,
                                     Gender = user.Gender,
                                     Username = user.Username,
                                     CountryId = user.CountryId,
                                     StateId = user.StateId,
                                     CityId = user.CityId,
                                     CountryName = user.Country.Name,
                                     StateName = user.State.Name,
                                     OtherState = user.OtherState,
                                     UserTypeId = user.UserTypeId,
                                     UserType = user.Usertypes.Name,
                                     Active = user.Active,
                                     RollNumber = user.RollNumber,
                                     MemberSince = DateFormat.ToDate(user.CreatedDateTime.ToString()),
                                     ParentEmail = user.ParentEmail,
                                     ParentContactNumber = user.ParentContactNumber,
                                     LevelFrom = _context.TeacherExperties.FirstOrDefault(m => m.TeacherId == user.Id) != null ? _context.TeacherExperties.FirstOrDefault(m => m.TeacherId == user.Id).LevelFrom : 0,
                                     LevelTo = _context.TeacherExperties.FirstOrDefault(m => m.TeacherId == user.Id) != null ? _context.TeacherExperties.FirstOrDefault(m => m.TeacherId == user.Id).LevelTo : 0,
                                     Experties = _context.TeacherExperties.Where(m => m.TeacherId == user.Id).Select(o => new TeacherExpertiesDtoForList
                                     {
                                         Id = o.SubjectId,
                                         Name = _context.Subjects.FirstOrDefault(m => m.Id == o.SubjectId) != null ? _context.Subjects.FirstOrDefault(m => m.Id == o.SubjectId).Name : "",
                                     }).ToList(),
                                     Photos = _context.Photos.Where(m => m.UserId == user.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         IsPrimary = x.IsPrimary,
                                         Url = _File.AppendImagePath(x.Name)
                                     }).ToList(),
                                 }).FirstOrDefaultAsync();

            if (GetUser != null && GetUser.Active == false && model.IsEditable == false)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = CustomMessage.URDeactivated;
                return serviceResponse;
            }
            else if (GetUser != null)
            {
                serviceResponse.Success = true;
                serviceResponse.Data = GetUser;
                return serviceResponse;
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = CustomMessage.RecordNotFound;
                return serviceResponse;
            }

        }


        public async Task<ServiceResponse<UserForDetailedDto>> GetUserRole(int id) // not in use
        {
            ServiceResponse<UserForDetailedDto> serviceResponse = new ServiceResponse<UserForDetailedDto>();

            var user = await _context.Users.Where(u => u.Id == id && u.Active == true).Select(s => new UserForDetailedDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                DateofBirth = s.DateofBirth != null ? DateFormat.ToDate(s.DateofBirth.ToString()) : "",
                //Photos = _context.Photos.Where(m => m.UserId == s.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).ToList()
            }).FirstOrDefaultAsync();

            //foreach (var item in serviceResponse.Data?.Photos)
            //{
            //    item.Url = _File.AppendImagePath(item.Url);
            //}
            serviceResponse.Data = user;
            serviceResponse.Success = true;
            return serviceResponse;


        }

        //public async Task<ServiceResponse<User>> GetUser(int id)
        //{
        //    ServiceResponse<User> serviceResponse = new ServiceResponse<User>();

        //    var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id && u.Active == true);

        //    foreach (var item in user?.Photos)
        //    {
        //        item.Url = _File.AppendImagePath(item.Url);
        //    }
        //    serviceResponse.Data = user;
        //    return serviceResponse;

        //}

        public async Task<ServiceResponse<object>> GetUsers(int id)
        {
            if (id > 0)
            {
                var users = await _context.Users.Where(m => m.Active == true && m.UserTypeId == id && m.SchoolBranchId == _LoggedIn_BranchID)
                    .OrderByDescending(m => m.Id).Include(m => m.Country).Include(m => m.State).Include(m => m.City).Select(o => new UserForListDto
                    {
                        Id = o.Id,
                        FullName = o.FullName,
                        DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                        Email = o.Email,
                        Gender = o.Gender,
                        Username = o.Username,
                        CountryId = o.CountryId,
                        StateId = o.StateId,
                        CityId = o.CityId,
                        CountryName = o.Country.Name,
                        StateName = o.State.Name,
                        OtherState = o.OtherState,
                        Active = o.Active,
                        UserTypeId = o.UserTypeId,
                        UserType = o.Usertypes.Name,
                        Photos = _context.Photos.Where(m => m.UserId == o.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsPrimary = x.IsPrimary,
                            Url = _File.AppendImagePath(x.Name)
                        }).ToList(),
                    }).ToListAsync();

                //foreach (var user in users)
                //{
                //    foreach (var item in user?.Photos)
                //    {
                //        item.Url = _File.AppendImagePath(item.Url);
                //    }
                //}
                _serviceResponse.Data = users;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {


                var users = await _context.Users.Where(m => m.Active == true && m.SchoolBranchId == _LoggedIn_BranchID)
                    .OrderByDescending(m => m.Id).Include(m => m.Country).Include(m => m.State).Include(m => m.City).Select(o => new UserForListDto
                    {
                        Id = o.Id,
                        FullName = o.FullName,
                        DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                        Email = o.Email,
                        Gender = o.Gender,
                        Username = o.Username,
                        CountryId = o.CountryId,
                        StateId = o.StateId,
                        CityId = o.CityId,
                        CountryName = o.Country.Name,
                        StateName = o.State.Name,
                        OtherState = o.OtherState,
                        Active = o.Active,
                        UserTypeId = o.UserTypeId,
                        UserType = o.Usertypes.Name,
                        Photos = _context.Photos.Where(m => m.UserId == o.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsPrimary = x.IsPrimary,
                            Url = _File.AppendImagePath(x.Name)
                        }).ToList(),
                    }).ToListAsync();

                //foreach (var user in users)
                //{
                //    foreach (var item in user?.Photos)
                //    {
                //        item.Url = _File.AppendImagePath(item.Url);
                //    }
                //}

                _serviceResponse.Data = users;
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
        }
        public async Task<IEnumerable<UserForListDto>> GetInActiveUsers()
        {
            var users = await _context.Users.Where(m => m.Active == false && m.SchoolBranchId == _LoggedIn_BranchID)
                .OrderByDescending(m => m.Id).Include(m => m.Country).Include(m => m.State).Include(m => m.City).Select(o => new UserForListDto
                {
                    Id = o.Id,
                    FullName = o.FullName,
                    DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                    Email = o.Email,
                    Gender = o.Gender,
                    Username = o.Username,
                    CountryId = o.CountryId,
                    StateId = o.StateId,
                    CityId = o.CityId,
                    CountryName = o.Country.Name,
                    StateName = o.State.Name,
                    OtherState = o.OtherState,
                    Active = o.Active,
                    UserTypeId = o.UserTypeId,
                    UserType = o.Usertypes.Name,
                    //Photos = _context.Photos.Where(m => m.UserId == o.Id).OrderByDescending(m => m.Id).ToList()
                }).ToListAsync();

            //foreach (var user in users)
            //{
            //    foreach (var item in user?.Photos)
            //    {
            //        item.Url = _File.AppendImagePath(item.Url);
            //    }
            //}

            return users;

        }
        public async Task<ServiceResponse<bool>> SaveAll()
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>
            {
                Data = await _context.SaveChangesAsync() > 0
            };
            return serviceResponse;

        }

        public async Task<bool> UserExists(string username)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            bool isExist = false;
            if (await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower() && x.SchoolBranchId == _LoggedIn_BranchID))
            {
                isExist = true;
            }

            return isExist;
        }


        public async Task<ServiceResponse<UserForListDto>> AddUser(UserForAddDto userDto)
        {
            ServiceResponse<UserForListDto> serviceResponse = new ServiceResponse<UserForListDto>();
            try
            {
                string NewRegNo = "";
                string NewRollNo = "";
                DateTime DateOfBirth = DateTime.ParseExact(userDto.DateofBirth, "MM/dd/yyyy", null);

                if (userDto.UserTypeId == (int)Enumm.UserType.Student)
                {
                    
                    SchoolBranch School = _context.SchoolBranch.Where(m => m.Id == _LoggedIn_BranchID).FirstOrDefault();
                    var LastUser = _context.Users.Where(m => m.UserTypeId == (int)Enumm.UserType.Student).ToList().LastOrDefault();
                    var HasRegistrationNumber = Convert.ToBoolean(_configuration.GetSection("AppSettings:SchoolHaveRegistrationNumbers").Value);
                    if (HasRegistrationNumber)
                    {
                        NewRegNo = userDto.RegistrationNumber;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(LastUser?.RegistrationNumber))
                        {
                            string RegNumber = Regex.Replace(LastUser?.RegistrationNumber, @"[^\d]", "");
                            int LastUserRegNo = Convert.ToInt32(RegNumber);
                            int NextRegNo = ++LastUserRegNo;
                            NewRegNo = $"{School.BranchName.Substring(0, 3)}-{NextRegNo:00000}";
                        }
                        else
                        {
                            NewRegNo = $"{School.BranchName.Substring(0, 3)}-{1:00000}";
                        }
                    }
                    if (!string.IsNullOrEmpty(LastUser?.RollNumber))
                    {
                        string RollNumber = Regex.Replace(LastUser?.RollNumber, @"[^\d]", "");
                        int LastUserRollNo = Convert.ToInt32(RollNumber);
                        int NextRollNo = ++LastUserRollNo;
                        NewRollNo = $"R-{School.BranchName.Substring(0, 3)}-{NextRollNo:00000}";
                    }
                    else
                    {
                        NewRollNo = $"R-{School.BranchName.Substring(0, 3)}-{1:00000}";
                    }
                    var TotalDays = (DateTime.Now.Date - DateOfBirth.Date).TotalDays;
                    var Age = Math.Truncate(TotalDays / 365);
                    if (Age < BusinessRules.Student_Min_Age)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = string.Format(CustomMessage.StudentMinAge, BusinessRules.Student_Min_Age.ToString());
                        return serviceResponse;
                    }
                }

                if (userDto.UserTypeId == (int)Enumm.UserType.Teacher)
                {
                    var TotalDays = (DateTime.Now.Date - DateOfBirth.Date).TotalDays;
                    var Age = Math.Truncate(TotalDays / 365);
                    if (Age < BusinessRules.Teacher_Min_Age)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = string.Format(CustomMessage.TeacherMinAge, BusinessRules.Teacher_Min_Age.ToString());
                        return serviceResponse;
                    }
                }

                var userToCreate = new User
                {
                    RegistrationNumber = NewRegNo,
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    UserTypeId = userDto.UserTypeId,
                    CreatedDateTime = DateTime.Now,
                    Gender = userDto.Gender,
                    Active = true,
                    DateofBirth = DateOfBirth,
                    LastActive = DateTime.Now,
                    StateId = userDto.StateId,
                    CityId = userDto.CityId,
                    CountryId = userDto.CountryId,
                    OtherState = userDto.OtherState,
                    Email = userDto.Email,
                    SchoolBranchId = _LoggedIn_BranchID,
                    RollNumber = NewRollNo,
                    Role = _context.UserTypes.Where(m => m.Id == userDto.UserTypeId).FirstOrDefault()?.Name
                };

                if (userDto.UserTypeId == (int)Enumm.UserType.Student)
                {
                    userToCreate.ParentContactNumber = userDto.ParentContactNumber;
                    userToCreate.ParentEmail = userDto.ParentEmail;
                }

                byte[] passwordHash, passwordSalt;
                Seed.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
                userToCreate.PasswordHash = passwordHash;
                userToCreate.PasswordSalt = passwordSalt;

                await _context.Users.AddAsync(userToCreate);
                await _context.SaveChangesAsync();

                if (userDto.UserTypeId == (int)Enumm.UserType.Student)
                {
                    // create parent account
                    
                    var parentToCreate = new User
                    {
                        FullName = userDto.ParentEmail.Split("@")[0],
                        Username = userDto.ParentEmail.Split("@")[0].ToLower(),
                        Email = userDto.ParentEmail,
                        UserTypeId = (int)Enumm.UserType.Parent,
                        CreatedDateTime = DateTime.Now,
                        Gender = "male",
                        Active = true,
                        StateId = userDto.StateId,
                        CityId = userDto.CityId,
                        CountryId = userDto.CountryId,
                        OtherState = userDto.OtherState,                        
                        SchoolBranchId = _LoggedIn_BranchID,
                        Role = _context.UserTypes.Where(m => m.Id == (int)Enumm.UserType.Parent).FirstOrDefault()?.Name
                    };
                   
                    Seed.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
                    parentToCreate.PasswordHash = passwordHash;
                    parentToCreate.PasswordSalt = passwordSalt;

                    await _context.Users.AddAsync(parentToCreate);
                    await _context.SaveChangesAsync();
                }
                if (userDto.UserTypeId == (int)Enumm.UserType.Teacher)
                {
                    if (Convert.ToInt32(userDto.LevelFrom) > Convert.ToInt32(userDto.LevelTo))
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = CustomMessage.LevelFromToCheck;
                        return serviceResponse;
                    }
                    List<TeacherExpertiesDtoForAdd> expertiesToAdd = new List<TeacherExpertiesDtoForAdd>();
                    if (userDto.Experties != null && userDto.Experties.Count() > 0 && userDto.Experties[0] != null)
                    {
                        foreach (var SubjectId in userDto.Experties)
                        {
                            expertiesToAdd.Add(new TeacherExpertiesDtoForAdd
                            {
                                SubjectId = Convert.ToInt32(SubjectId),
                                TeacherId = userToCreate.Id,
                                LevelFrom = Convert.ToInt32(userDto.LevelFrom),
                                LevelTo = Convert.ToInt32(userDto.LevelTo),
                            });
                        }
                        var response = await _TeacherRepository.AddExperties(expertiesToAdd, userToCreate.Id);
                    }
                    else
                    {
                        serviceResponse.Message = CustomMessage.ExpertiesRequired;
                        serviceResponse.Success = false;
                        return serviceResponse;
                    }
                }
                //var createdUser = _mapper.Map<UserForListDto>(userToCreate);

                serviceResponse.Success = true;
                serviceResponse.Message = CustomMessage.Added;
                return serviceResponse;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }
                return serviceResponse;
            }
        }



        public async Task<ServiceResponse<string>> EditUser(int id, UserForUpdateDto user)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                User dbUser = _context.Users.FirstOrDefault(s => s.Id.Equals(id));
                if (dbUser != null)
                {
                    User checkExist = _context.Users.FirstOrDefault(m => m.Username.ToLower() == user.Username.ToLower() && m.SchoolBranchId == _LoggedIn_BranchID);
                    if (checkExist != null && checkExist.Id != id)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = CustomMessage.UserAlreadyExist;
                        return serviceResponse;
                    }
                    DateTime DateOfBirth = DateTime.ParseExact(user.DateofBirth, "MM/dd/yyyy", null);

                    var oldStatus = dbUser.Active;
                    var oldType = dbUser.UserTypeId;
                    if (user.UserTypeId == (int)Enumm.UserType.Student)
                    {

                        var TotalDays = (DateTime.Now.Date - DateOfBirth.Date).TotalDays;
                        var Age = Math.Truncate(TotalDays / 365);
                        if (Age < BusinessRules.Student_Min_Age)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = string.Format(CustomMessage.StudentMinAge, BusinessRules.Student_Min_Age.ToString());
                            return serviceResponse;
                        }
                        if (oldStatus == true && user.Active == false)
                        {
                            var StudentReferences = _context.ClassSectionUsers.Where(m => m.UserId == dbUser.Id).ToList().Count();
                            if (StudentReferences > 0)
                            {
                                serviceResponse.Message = string.Format(CustomMessage.RecordRelationExist, "Student");
                                serviceResponse.Success = false;
                                return serviceResponse;
                            }
                        }
                        if (oldType != user.UserTypeId)
                        {
                            var StudentReferences = _context.ClassSectionUsers.Where(m => m.UserId == dbUser.Id).ToList().Count();
                            if (StudentReferences > 0)
                            {
                                serviceResponse.Message = string.Format(CustomMessage.CantChangeUserType, "Student");
                                serviceResponse.Success = false;
                                return serviceResponse;
                            }
                        }
                    }
                    if (user.UserTypeId == (int)Enumm.UserType.Teacher)
                    {
                        var TotalDays = (DateTime.Now.Date - DateOfBirth.Date).TotalDays;
                        var Age = Math.Truncate(TotalDays / 365);
                        if (Age < BusinessRules.Teacher_Min_Age)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = string.Format(CustomMessage.TeacherMinAge, BusinessRules.Teacher_Min_Age.ToString());
                            return serviceResponse;
                        }
                        if (oldStatus == true && user.Active == false)
                        {
                            var GetReferences = _context.ClassSectionUsers.Where(m => m.UserId == dbUser.Id).ToList().Count();
                            var GetReferences2 = _context.ClassLectureAssignment.Where(m => m.TeacherId == dbUser.Id).ToList().Count();
                            if (GetReferences > 0 || GetReferences2 > 0)
                            {
                                serviceResponse.Message = string.Format(CustomMessage.RecordRelationExist, "Teacher");
                                serviceResponse.Success = false;
                                return serviceResponse;
                            }
                        }
                        if (oldType != user.UserTypeId)
                        {
                            var StudentReferences = _context.ClassSectionUsers.Where(m => m.UserId == dbUser.Id).ToList().Count();
                            if (StudentReferences > 0)
                            {
                                serviceResponse.Message = string.Format(CustomMessage.CantChangeUserType, "Teacher");
                                serviceResponse.Success = false;
                                return serviceResponse;
                            }
                        }
                    }

                    dbUser.FullName = user.FullName;
                    dbUser.Email = user.Email;
                    dbUser.Username = user.Username.ToLower();
                    dbUser.StateId = user.StateId;
                    dbUser.CityId = user.CityId;
                    dbUser.CountryId = user.CountryId;
                    dbUser.OtherState = user.OtherState;
                    dbUser.DateofBirth = DateOfBirth;
                    dbUser.Gender = user.Gender;
                    dbUser.ParentEmail = user.ParentEmail;
                    dbUser.ParentContactNumber = user.ParentContactNumber;
                    dbUser.Active = user.Active;
                    if (user.UserTypeId > 0)
                    {
                        dbUser.Role = _context.UserTypes.Where(m => m.Id == user.UserTypeId).FirstOrDefault()?.Name;
                        dbUser.UserTypeId = user.UserTypeId;
                    }
                    if (!string.IsNullOrEmpty(user.OldPassword))
                    {
                        if (Seed.VerifyPasswordHash(user.OldPassword, dbUser.PasswordHash, dbUser.PasswordSalt))
                        {

                            if (!string.IsNullOrEmpty(user.Password))
                            {
                                byte[] passwordhash, passwordSalt;
                                Seed.CreatePasswordHash(user.Password, out passwordhash, out passwordSalt);
                                dbUser.PasswordHash = passwordhash;
                                dbUser.PasswordSalt = passwordSalt;
                            }
                            else
                            {
                                serviceResponse.Success = false;
                                serviceResponse.Message = CustomMessage.NewPasswordNotGiven;
                                return serviceResponse;
                            }

                        }
                        else
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = CustomMessage.PasswordNotMatched;
                            return serviceResponse;
                        }

                    }


                    await _context.SaveChangesAsync();

                    if (user.UserTypeId == (int)Enumm.UserType.Teacher)
                    {
                        if (Convert.ToInt32(user.LevelFrom) > Convert.ToInt32(user.LevelTo))
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = CustomMessage.LevelFromToCheck;
                            return serviceResponse;
                        }
                        List<TeacherExpertiesDtoForAdd> expertiesToAdd = new List<TeacherExpertiesDtoForAdd>();
                        if (user.Experties != null && user.Experties.Count() > 0 && user.Experties[0] != null)
                        {
                            var expertiesList = user.Experties[0].Split(',').ToList();
                            foreach (var SubjectId in expertiesList)
                            {
                                expertiesToAdd.Add(new TeacherExpertiesDtoForAdd
                                {
                                    SubjectId = Convert.ToInt32(SubjectId),
                                    TeacherId = dbUser.Id,
                                    LevelFrom = Convert.ToInt32(user.LevelFrom),
                                    LevelTo = Convert.ToInt32(user.LevelTo),
                                });
                            }
                            var response = await _TeacherRepository.AddExperties(expertiesToAdd, dbUser.Id);
                        }
                        else
                        {
                            serviceResponse.Message = CustomMessage.ExpertiesRequired;
                            serviceResponse.Success = false;
                            return serviceResponse;
                        }
                    }
                    // saving images

                    if (user.files != null && user.files.Count() > 0)
                    {
                        for (int i = 0; i < user.files.Count(); i++)
                        {
                            var file = user.files[i];
                            string Name = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            if (user.IsPrimaryPhoto)
                            {
                                IQueryable<Photo> updatePrimaryPhotos = _context.Photos.Where(m => m.UserId == dbUser.Id);
                                await updatePrimaryPhotos.ForEachAsync(m => m.IsPrimary = false);
                            }
                            Photo updatePhoto = _context.Photos.Where(m => m.UserId == dbUser.Id).FirstOrDefault();
                            if (updatePhoto == null)
                            {
                                var photo = new Photo
                                {
                                    Name = Name,
                                    Description = "description...",
                                    IsPrimary = user.IsPrimaryPhoto,
                                    UserId = dbUser.Id,
                                    Url = _File.GetBinaryFile(file),
                                    CreatedDatetime = DateTime.Now
                                };
                                await _context.Photos.AddAsync(photo);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                updatePhoto.Name = Name;
                                updatePhoto.IsPrimary = user.IsPrimaryPhoto;
                                updatePhoto.UserId = dbUser.Id;
                                updatePhoto.Url = _File.GetBinaryFile(file);
                                _context.Photos.Update(updatePhoto);
                                await _context.SaveChangesAsync();
                            }

                        }
                    }
                    if (oldStatus == true && user.Active == false)
                    {
                        serviceResponse.Message = CustomMessage.RecordDeActivated;
                        serviceResponse.Success = true;
                    }
                    else
                    {
                        serviceResponse.Message = CustomMessage.Updated;
                        serviceResponse.Success = true;
                    }
                }
                else
                {
                    serviceResponse.Message = CustomMessage.RecordNotFound;
                    serviceResponse.Success = false;
                }

                return serviceResponse;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }

                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<string>> UpdateProfile(int id, UserForUpdateDto user)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                User dbUser = _context.Users.FirstOrDefault(s => s.Id.Equals(id));
                if (dbUser != null)
                {
                    User checkExist = _context.Users.FirstOrDefault(m => m.Username.ToLower() == user.Username.ToLower() && m.SchoolBranchId == _LoggedIn_BranchID);
                    if (checkExist != null && checkExist.Id != id)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = CustomMessage.UserAlreadyExist;
                        return serviceResponse;
                    }
                    DateTime DateOfBirth = DateTime.ParseExact(user.DateofBirth, "MM/dd/yyyy", null);

                    if (user.UserTypeId == (int)Enumm.UserType.Student)
                    {
                        var TotalDays = (DateTime.Now.Date - DateOfBirth.Date).TotalDays;
                        var Age = Math.Truncate(TotalDays / 365);
                        if (Age < BusinessRules.Student_Min_Age)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = string.Format(CustomMessage.StudentMinAge, BusinessRules.Student_Min_Age.ToString());
                            return serviceResponse;
                        }
                    }
                    if (user.UserTypeId == (int)Enumm.UserType.Teacher)
                    {
                        var TotalDays = (DateTime.Now.Date - DateOfBirth.Date).TotalDays;
                        var Age = Math.Truncate(TotalDays / 365);
                        if (Age < BusinessRules.Teacher_Min_Age)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = string.Format(CustomMessage.TeacherMinAge, BusinessRules.Teacher_Min_Age.ToString());
                            return serviceResponse;
                        }

                    }

                    dbUser.FullName = user.FullName;
                    dbUser.Email = user.Email;
                    dbUser.Username = user.Username.ToLower();
                    dbUser.StateId = user.StateId;
                    dbUser.CityId = user.CityId;
                    dbUser.CountryId = user.CountryId;
                    dbUser.OtherState = user.OtherState;
                    dbUser.DateofBirth = DateOfBirth;
                    dbUser.Gender = user.Gender;
                    dbUser.ParentEmail = user.ParentEmail;
                    dbUser.ParentContactNumber = user.ParentContactNumber;
                    dbUser.Active = user.Active;
                    if (user.UserTypeId > 0)
                    {
                        dbUser.Role = _context.UserTypes.Where(m => m.Id == user.UserTypeId).FirstOrDefault()?.Name;
                        dbUser.UserTypeId = user.UserTypeId;
                    }
                    if (!string.IsNullOrEmpty(user.OldPassword))
                    {
                        if (Seed.VerifyPasswordHash(user.OldPassword, dbUser.PasswordHash, dbUser.PasswordSalt))
                        {

                            if (!string.IsNullOrEmpty(user.Password))
                            {
                                byte[] passwordhash, passwordSalt;
                                Seed.CreatePasswordHash(user.Password, out passwordhash, out passwordSalt);
                                dbUser.PasswordHash = passwordhash;
                                dbUser.PasswordSalt = passwordSalt;
                            }
                            else
                            {
                                serviceResponse.Success = false;
                                serviceResponse.Message = CustomMessage.NewPasswordNotGiven;
                                return serviceResponse;
                            }

                        }
                        else
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = CustomMessage.PasswordNotMatched;
                            return serviceResponse;
                        }

                    }


                    await _context.SaveChangesAsync();


                    // saving images

                    if (user.files != null && user.files.Count() > 0)
                    {
                        for (int i = 0; i < user.files.Count(); i++)
                        {
                            var file = user.files[i];
                            string Name = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            if (user.IsPrimaryPhoto)
                            {
                                IQueryable<Photo> updatePrimaryPhotos = _context.Photos.Where(m => m.UserId == dbUser.Id);
                                await updatePrimaryPhotos.ForEachAsync(m => m.IsPrimary = false);
                            }
                            Photo updatePhoto = _context.Photos.Where(m => m.UserId == dbUser.Id).FirstOrDefault();
                            if (updatePhoto == null)
                            {
                                var photo = new Photo
                                {
                                    Name = Name,
                                    Description = "description...",
                                    IsPrimary = user.IsPrimaryPhoto,
                                    UserId = dbUser.Id,
                                    Url = _File.GetBinaryFile(file),
                                    CreatedDatetime = DateTime.Now
                                };
                                await _context.Photos.AddAsync(photo);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                updatePhoto.Name = Name;
                                updatePhoto.IsPrimary = user.IsPrimaryPhoto;
                                updatePhoto.UserId = dbUser.Id;
                                updatePhoto.Url = _File.GetBinaryFile(file);
                                _context.Photos.Update(updatePhoto);
                                await _context.SaveChangesAsync();
                            }

                        }
                    }

                    serviceResponse.Message = CustomMessage.Updated;
                    serviceResponse.Success = true;

                }
                else
                {
                    serviceResponse.Message = CustomMessage.RecordNotFound;
                    serviceResponse.Success = false;
                }

                return serviceResponse;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = CustomMessage.SqlDuplicateRecord;
                }
                else
                {
                    throw ex;
                }

                return serviceResponse;
            }
        }

        public async Task<ServiceResponse<IEnumerable<UserByTypeListDto>>> GetUsersByType(int typeId, int? classSectionId)
        {
            ServiceResponse<IEnumerable<UserByTypeListDto>> serviceResponse = new ServiceResponse<IEnumerable<UserByTypeListDto>>();
            var today = DateTime.Now;
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            if (!string.IsNullOrEmpty(classSectionId.ToString()))
            {
                var users = await (from u in _context.Users
                                   join csU in _context.ClassSectionUsers
                                   on u.Id equals csU.UserId

                                   where u.UserTypeId == typeId
                                   && csU.ClassSectionId == classSectionId
                                   && u.Active == true
                                   && u.SchoolBranchId == _LoggedIn_BranchID
                                   orderby u.Id descending
                                   select u).Select(o => new UserByTypeListDto
                                   {
                                       UserId = o.Id,
                                       FullName = o.FullName,
                                       Present = false,
                                       Absent = false,
                                       Late = false,
                                       Comments = "",
                                       UserTypeId = o.UserTypeId,
                                       ClassSectionId = _context.ClassSectionUsers.Where(m => m.UserId == o.Id).FirstOrDefault().ClassSectionId,
                                       LeaveCount = _context.Leaves.Where(m => m.UserId == o.Id).Count(),
                                       AbsentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Absent == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                                       LateCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Late == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                                       PresentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Present == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                                       Photos = _context.Photos.Where(m => m.UserId == o.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           IsPrimary = x.IsPrimary,
                                           Url = _File.AppendImagePath(x.Name)
                                       }).ToList(),
                                   }).ToListAsync();

                //var ToReturn = users.Select(o => new UserByTypeListDto
                //{
                //    UserId = o.Id,
                //    FullName = o.FullName,
                //    Present = false,
                //    Absent = false,
                //    Late = false,
                //    Comments = "",
                //    UserTypeId = o.UserTypeId,
                //    ClassSectionId = _context.ClassSectionUsers.Where(m => m.UserId == o.Id).FirstOrDefault()?.ClassSectionId,
                //    LeaveCount = _context.Leaves.Where(m => m.UserId == o.Id).Count(),
                //    AbsentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Absent == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                //    LateCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Late == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                //    PresentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Present == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                //    //Photos = _context.Photos.Where(m => m.UserId == o.Id).ToList()
                //}).ToList();
                //foreach (var user in users)
                //{
                //    foreach (var item in user?.Photos)
                //    {
                //        item.Url = _File.AppendImagePath(item.Url);
                //    }
                //}
                serviceResponse.Data = users;
                serviceResponse.Success = true;
            }
            else
            {
                var users = await (from u in _context.Users
                                   where u.UserTypeId == typeId
                                   && u.Active == true
                                   && u.SchoolBranchId == _LoggedIn_BranchID
                                   orderby u.Id descending
                                   select u).Select(o => new UserByTypeListDto
                                   {
                                       UserId = o.Id,
                                       FullName = o.FullName,
                                       Present = false,
                                       Absent = false,
                                       Late = false,
                                       Comments = "",
                                       UserTypeId = o.UserTypeId,
                                       ClassSectionId = _context.ClassSectionUsers.Where(m => m.UserId == o.Id).FirstOrDefault().ClassSectionId,
                                       LeaveCount = _context.Leaves.Where(m => m.UserId == o.Id).Count(),
                                       AbsentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Absent == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                                       LateCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Late == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                                       PresentCount = _context.Attendances.Where(m => m.UserId == o.Id && m.Present == true && m.CreatedDatetime >= thisMonth && m.CreatedDatetime <= today).Count(),
                                       Photos = _context.Photos.Where(m => m.UserId == o.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           IsPrimary = x.IsPrimary,
                                           Url = _File.AppendImagePath(x.Name)
                                       }).ToList(),
                                   }).ToListAsync();

                serviceResponse.Data = users;
                serviceResponse.Success = true;

            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUnmappedStudents()
        {
            var userIds = (from u in _context.Users
                           join csU in _context.ClassSectionUsers
                           on u.Id equals csU.UserId
                           into Details
                           from m in Details.DefaultIfEmpty()
                           where u.SchoolBranchId == _LoggedIn_BranchID
                           && u.Active == true
                           select new
                           {
                               id = u.Id,
                               userId = m.UserId
                           }).Where(x => x.userId == null).ToList();

            List<User> unmappedStudents = await _context.Users.Where(m => userIds.Select(sel => sel.id).Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Student).ToListAsync();
            _serviceResponse.Data = _mapper.Map<List<UserForListDto>>(unmappedStudents);
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetMappedStudents(int csId)
        {
            var userIds = (from u in _context.Users
                           join csU in _context.ClassSectionUsers
                           on u.Id equals csU.UserId
                           into Details
                           from m in Details.DefaultIfEmpty()
                           where u.SchoolBranchId == _LoggedIn_BranchID
                           && u.Active == true
                           && m.ClassSectionId == csId
                           select new
                           {
                               id = u.Id,
                               userId = m.UserId

                           }).Where(x => x.userId != null).ToList();
            List<User> StudentList = await _context.Users.Where(m => userIds.Select(sel => sel.id).Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Student).ToListAsync();
            var mappedStudents = _mapper.Map<List<UserForListDto>>(StudentList);

            User Teacher = await _context.Users.Where(m => userIds.Select(sel => sel.id).Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Teacher).FirstOrDefaultAsync();
            var mappedTeacher = _mapper.Map<UserForListDto>(Teacher);
            _serviceResponse.Data = new { mappedStudents, mappedTeacher };
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddUsersInGroup(UserForAddInGroupDto model)
        {

            var group = new Models.Group
            {
                GroupName = model.GroupName,
                ClassSectionId = model.ClassSectionId,
                Active = true,
                SchoolBranchId = _LoggedIn_BranchID
            };
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            List<GroupUser> listToAdd = new List<GroupUser>();
            foreach (var item in model.UserIds)
            {
                listToAdd.Add(new GroupUser
                {
                    GroupId = group.Id,
                    UserId = item
                });
            }
            await _context.GroupUsers.AddRangeAsync(listToAdd);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Added;
            return _serviceResponse;


        }


        public async Task<ServiceResponse<object>> GetGroupUsers()
        {

            List<GroupListDto> groupList = new List<GroupListDto>();

            //var users = (await _context.GroupUsers
            //    .Include(m => m.Group).Include(m => m.User).ToListAsync())
            //    .GroupBy(m => m.Group.GroupName)
            //    .ToDictionary(e => e.Key, e => e.Select(e2 => new
            //    {
            //        Id = e2.User.Id,
            //        FullName = e2.User.FullName
            //    })).ToList();





            var result = await _context.Groups
               .Join(_context.GroupUsers
               , od => od.Id
               , o => o.GroupId
               , (o, od) => new
               {
                   o.Id,
                   od.Group
               })
                .Where(m => m.Group.SchoolBranchId == _LoggedIn_BranchID && m.Group.Active == true).Select(s => s).Distinct().ToListAsync();


            var users = string.Empty;

            foreach (var item in result)
            {

                groupList.Add(new GroupListDto
                {
                    Id = item.Id,
                    GroupName = item.Group.GroupName,
                });

            }


            foreach (var item in groupList)
            {
                var user = await _context.Groups.Where(x => x.Id == item.Id).
                           Join(_context.GroupUsers, g => g.Id,
                     gu => gu.GroupId, (Group, GroupUser) => new
                     {
                         Group = Group,
                         GroupUser = GroupUser
                     }).Join(_context.Users, gu => gu.GroupUser.UserId, u => u.Id, (GroupUser, User) => new
                     {
                         GroupUser = GroupUser,
                         User = User
                     })
                       .Where(m => m.GroupUser.Group.SchoolBranchId == _LoggedIn_BranchID && m.User.Active == true).Select(p => p).ToListAsync();

                foreach (var item_u in user)
                {
                    if (item.Id == item_u.GroupUser.Group.Id)
                    {

                        item.Children.Add(new GroupUserListDto
                        {
                            Display = item_u.User.FullName,
                            Value = item_u.User.Id
                        });
                    }

                }
            }





            if (groupList.Count > 0)
            {
                _serviceResponse.Data = groupList;// JsonConvert.SerializeObject(groupList);
            }

            _serviceResponse.Success = true;
            return _serviceResponse;


        }

        public async Task<ServiceResponse<object>> UpdateUsersInGroup(UserForAddInGroupDto model)
        {


            var group = await _context.Groups.Where(m => m.Id == model.Id && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).FirstOrDefaultAsync();
            if (group != null)
            {
                group.GroupName = model.GroupName;
                group.Active = model.Active ?? true;
                group.SchoolBranchId = _LoggedIn_BranchID;

                await _context.SaveChangesAsync();
                if (model.UserIds.Count() > 0)
                {
                    List<GroupUser> listToDelete = await _context.GroupUsers.Where(m => m.GroupId == model.Id && m.Group.SchoolBranchId == _LoggedIn_BranchID).ToListAsync();
                    _context.GroupUsers.RemoveRange(listToDelete);
                    await _context.SaveChangesAsync();

                    List<GroupUser> listToAdd = new List<GroupUser>();
                    foreach (var item in model.UserIds)
                    {
                        listToAdd.Add(new GroupUser
                        {
                            GroupId = group.Id,
                            UserId = item
                        });
                    }
                    await _context.GroupUsers.AddRangeAsync(listToAdd);
                    await _context.SaveChangesAsync();
                }
                _serviceResponse.Success = true;
                _serviceResponse.Message = CustomMessage.Updated;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> GetGroupUsersById(int id)
        {

            List<GroupListForEditDto> groupList = new List<GroupListForEditDto>();

            var result = await (from g in _context.Groups
                                join gu in _context.GroupUsers
                                on g.Id equals gu.GroupId
                                where g.Id == id
                                && g.SchoolBranchId == _LoggedIn_BranchID
                                && g.Active == true
                                select new
                                {
                                    g.Id,
                                    g.GroupName,
                                    g.ClassSectionId
                                }).FirstOrDefaultAsync();

            var users = string.Empty;

            if (result != null)
            {

                groupList.Add(new GroupListForEditDto
                {
                    Id = result.Id,
                    GroupName = result.GroupName,
                    ClassSectionId = result.ClassSectionId
                });

            }


            foreach (var item in groupList)
            {
                var user = await (from u in _context.Users
                                  join gu in _context.GroupUsers
                                  on u.Id equals gu.UserId
                                  join g in _context.Groups
                                  on gu.GroupId equals g.Id
                                  where g.SchoolBranchId == _LoggedIn_BranchID
                                  && u.Active == true
                                  && g.Active == true
                                  select new
                                  {
                                      GroupUser = gu,
                                      User = u,
                                      Group = g
                                  }).ToListAsync();

                foreach (var item_u in user)
                {
                    if (item.Id == item_u.GroupUser.Group.Id)
                    {

                        item.Students.Add(new GroupUserListForEditDto
                        {
                            FullName = item_u.User.FullName,
                            Id = item_u.User.Id
                        });
                    }

                }
            }



            if (groupList.Count > 0)
            {
                _serviceResponse.Data = groupList;// JsonConvert.SerializeObject(groupList);
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
            }

            _serviceResponse.Success = true;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> DeleteGroup(int id)
        {

            var group = _context.Groups.Where(m => m.Id == id && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).FirstOrDefault();
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                _serviceResponse.Success = true;
                return _serviceResponse;
            }
            else
            {
                _serviceResponse.Success = false;
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                return _serviceResponse;
            }
        }

        public async Task<ServiceResponse<object>> ActiveInActiveUser(int id, bool status)
        {
            var user = _context.Users.Where(m => m.Id == id && m.SchoolBranchId == _LoggedIn_BranchID && m.Active == true).FirstOrDefault();
            if (user.Active == true && status == false)
            {
                var GetReferences = _context.ClassSectionUsers.Where(m => m.UserId == user.Id).ToList().Count();
                var GetReferences2 = _context.ClassLectureAssignment.Where(m => m.TeacherId == user.Id).ToList().Count();
                if (GetReferences > 0 || GetReferences2 > 0)
                {
                    _serviceResponse.Message = string.Format(CustomMessage.RecordRelationExist, "This");
                    _serviceResponse.Success = false;
                    return _serviceResponse;
                }
            }
            user.Active = status;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.ActiveStatusUpdated;
            return _serviceResponse;

        }

        public async Task<ServiceResponse<object>> GetUsersByClassSection(int classSectionId)
        {
            var users = await (from u in _context.Users
                               join csU in _context.ClassSectionUsers
                               on u.Id equals csU.UserId
                               where u.UserTypeId == (int)Enumm.UserType.Student
                               && csU.ClassSectionId == classSectionId
                               && u.Active == true
                               && u.SchoolBranchId == _LoggedIn_BranchID
                               select u).Include(m => m.Country).Include(m => m.State).Include(m => m.City).Select(o => new UserForListDto
                               {
                                   Id = o.Id,
                                   FullName = o.FullName,
                                   DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                                   Email = o.Email,
                                   Gender = o.Gender,
                                   Username = o.Username,
                                   CountryId = o.CountryId,
                                   StateId = o.StateId,
                                   CityId = o.CityId,
                                   CountryName = o.Country.Name,
                                   StateName = o.State.Name,
                                   OtherState = o.OtherState,
                                   Active = o.Active,
                                   UserTypeId = o.UserTypeId,
                                   UserType = o.Usertypes.Name,
                                   Photos = _context.Photos.Where(m => m.UserId == o.Id && m.IsPrimary == true).OrderByDescending(m => m.Id).Select(x => new PhotoDto
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       IsPrimary = x.IsPrimary,
                                       Url = _File.AppendImagePath(x.Name)
                                   }).ToList(),
                               }).ToListAsync();

            //foreach (var user in users)
            //{
            //    foreach (var item in user?.Photos)
            //    {
            //        item.Url = _File.AppendImagePath(item.Url);
            //    }
            //}


            _serviceResponse.Success = true;
            _serviceResponse.Data = users;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> SearchTutor(SearchTutorDto model)
        {
            var users = await (from user in _context.Users
                               join csUser in _context.ClassSectionUsers
                               on user.Id equals csUser.UserId

                               join cs in _context.ClassSections
                               on csUser.ClassSectionId equals cs.Id

                               join subAssign in _context.SubjectAssignments
                               on cs.ClassId equals subAssign.ClassId

                               join subject in _context.Subjects
                               on subAssign.SubjectId equals subject.Id

                               where csUser.ClassSection.ClassId == model.GradeId
                               //&& user.Gender.ToLower() == model.Gender.ToLower()
                               && user.CityId == model.CityId
                               && subject.Id == model.SubjectId
                               && user.Active == true
                               && user.UserTypeId == (int)Enumm.UserType.Tutor
                               select new { user, csUser, subject }).Select(o => new TutorForListDto
                               {
                                   Id = o.user.Id,
                                   FullName = o.user.FullName,
                                   DateofBirth = o.user.DateofBirth != null ? DateFormat.ToDate(o.user.DateofBirth.ToString()) : "",
                                   Email = o.user.Email,
                                   Gender = o.user.Gender,
                                   Username = o.user.Username,
                                   CountryId = o.user.CountryId,
                                   StateId = o.user.StateId,
                                   CityId = o.user.CityId,
                                   CountryName = o.user.Country.Name,
                                   StateName = o.user.State.Name,
                                   OtherState = o.user.OtherState,
                                   GradeId = o.csUser.ClassSection.ClassId,
                                   GradeName = _context.Class.FirstOrDefault(m => m.Id == o.csUser.ClassSection.ClassId).Name,
                                   SubjectId = o.subject.Id,
                                   SubjectName = o.subject.Name,
                                   PhotoUrl = _context.Photos.Where(m => m.UserId == o.user.Id && m.IsPrimary == true).FirstOrDefault() != null ? _File.AppendImagePath(_context.Photos.Where(m => m.UserId == o.user.Id && m.IsPrimary == true).FirstOrDefault().Name) : "",
                               }).ToListAsync();


            _serviceResponse.Data = users;
            _serviceResponse.Success = true;
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> UnMapUser(UnMapUserForAddDto model)
        {
            var ToRemove = await _context.ClassSectionUsers.Where(m => m.ClassSectionId == model.ClassSectionId && m.UserId == model.UserId && m.SchoolBranchId == _LoggedIn_BranchID).FirstOrDefaultAsync();
            if (ToRemove != null)
            {
                _context.ClassSectionUsers.Remove(ToRemove);
                await _context.SaveChangesAsync();

                List<ClassSectionTransaction> ToAdd = new List<ClassSectionTransaction>();

                ToAdd.Add(new ClassSectionTransaction
                {
                    ClassSectionId = ToRemove.ClassSectionId,
                    UserId = ToRemove.UserId,
                    MappedCreationDate = ToRemove.CreatedDate,
                    UserTypeId = _context.Users.FirstOrDefault(m => m.Id == ToRemove.UserId && m.Active == true).UserTypeId,
                    DeletionDate = DateTime.Now,
                    DeletedById = _LoggedIn_UserID
                });

                await _context.ClassSectionTransactions.AddRangeAsync(ToAdd);
                await _context.SaveChangesAsync();
                _serviceResponse.Message = CustomMessage.Deleted;
                _serviceResponse.Success = true;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
            }
            return _serviceResponse;
        }

        public async Task<ServiceResponse<object>> CheckUserActiveStatus()
        {
            var check = await _context.Users.Where(m => m.Id == _LoggedIn_UserID && m.Active == true).FirstOrDefaultAsync();
            if (check != null)
            {
                _serviceResponse.Message = check.Active.ToString();
                _serviceResponse.Success = true;
            }
            else
            {
                _serviceResponse.Message = CustomMessage.RecordNotFound;
                _serviceResponse.Success = false;
            }
            return _serviceResponse;
        }
    }
}

