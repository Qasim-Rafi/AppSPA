using AutoMapper;
using CoreWebApi.Dtos;
using CoreWebApi.Helpers;
using CoreWebApi.IData;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly IFilesRepository _File;
        ServiceResponse<object> _serviceResponse;

        public UserRepository(DataContext context, IMapper mapper, IWebHostEnvironment HostEnvironment, IFilesRepository file)
        {
            _context = context;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
            _File = file;
            _serviceResponse = new ServiceResponse<object>();

        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(Task entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<ServiceResponse<UserForDetailedDto>> GetUser(int id)
        {
            ServiceResponse<UserForDetailedDto> serviceResponse = new ServiceResponse<UserForDetailedDto>();
            var user = await _context.Users.Where(u => u.Id == id).Select(s => new UserForDetailedDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                DateofBirth = s.DateofBirth != null ? DateFormat.ToDate(s.DateofBirth.ToString()) : "",
                Photos = _context.Photos.Where(m => m.UserId == s.Id).ToList(),
                Email = s.Email,
                Gender = s.Gender,
                Username = s.Username,
                CountryId = s.CountryId,
                StateId = s.StateId,
                CountryName = s.Country.Name,
                StateName = s.State.Name,
                OtherState = s.OtherState,
                Active = s.Active,
                MemberSince = DateFormat.ToDate(s.CreatedDateTime.ToString())
            }).FirstOrDefaultAsync();
            if (user != null)
            {
                foreach (var item in user?.Photos)
                {
                    item.Url = _File.AppendImagePath(item.Url);
                }

                serviceResponse.Success = true;
                serviceResponse.Data = user;
                return serviceResponse;
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = CustomMessage.RecordNotFound;
                return serviceResponse;
            }

        }


        public async Task<ServiceResponse<UserForDetailedDto>> GetUserRole(int id)
        {
            ServiceResponse<UserForDetailedDto> serviceResponse = new ServiceResponse<UserForDetailedDto>();

            var user = await _context.Users.Where(u => u.Id == id && u.Active == true).Select(s => new UserForDetailedDto()
            {
                Id = s.Id,
                FullName = s.FullName,
                DateofBirth = s.DateofBirth != null ? DateFormat.ToDate(s.DateofBirth.ToString()) : "",
                Photos = _context.Photos.Where(m => m.UserId == s.Id).ToList()
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

        public async Task<IEnumerable<UserForListDto>> GetUsers()
        {


            var users = await _context.Users.Where(m => m.Active == true).OrderByDescending(m => m.Id).Include(m => m.Country).Include(m => m.State).Select(o => new UserForListDto
            {
                Id = o.Id,
                FullName = o.FullName,
                DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                Email = o.Email,
                Gender = o.Gender,
                Username = o.Username,
                CountryId = o.CountryId,
                StateId = o.StateId,
                CountryName = o.Country.Name,
                StateName = o.State.Name,
                OtherState = o.OtherState,
                Active = o.Active,
                Photos = _context.Photos.Where(m => m.UserId == o.Id).ToList()
            }).ToListAsync();

            foreach (var user in users)
            {
                foreach (var item in user?.Photos)
                {
                    item.Url = _File.AppendImagePath(item.Url);
                }
            }

            return users;

        }
        public async Task<IEnumerable<UserForListDto>> GetInActiveUsers()
        {
            var users = await _context.Users.Where(m => m.Active == false).OrderByDescending(m => m.Id).Include(m => m.Country).Include(m => m.State).Select(o => new UserForListDto
            {
                Id = o.Id,
                FullName = o.FullName,
                DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                Email = o.Email,
                Gender = o.Gender,
                Username = o.Username,
                CountryId = o.CountryId,
                StateId = o.StateId,
                CountryName = o.Country.Name,
                StateName = o.State.Name,
                OtherState = o.OtherState,
                Active = o.Active,
                Photos = _context.Photos.Where(m => m.UserId == o.Id).ToList()
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
            if (await _context.Users.AnyAsync(x => x.Username == username))
            {
                isExist = true;
            }

            return isExist;
        }


        public async Task<ServiceResponse<UserForListDto>> AddUser(UserForAddDto userDto)
        {
            ServiceResponse<UserForListDto> serviceResponse = new ServiceResponse<UserForListDto>();


            var userToCreate = new User
            {
                FullName = userDto.FullName,
                Username = userDto.Username,
                UserTypeId = userDto.UserTypeId,
                CreatedDateTime = DateTime.Now,
                Gender = userDto.Gender,
                Active = true,
                DateofBirth = Convert.ToDateTime(userDto.DateofBirth),
                LastActive = DateTime.Now,
                StateId = userDto.StateId,
                CountryId = userDto.CountryId,
                OtherState = userDto.OtherState,
                Email = userDto.Email,
                SchoolBranchId = Convert.ToInt32(userDto.LoggedIn_BranchId)
            };
            byte[] passwordHash, passwordSalt;
            Seed.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            userToCreate.PasswordHash = passwordHash;
            userToCreate.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(userToCreate);
            await _context.SaveChangesAsync();

            var createdUser = _mapper.Map<UserForListDto>(userToCreate);

            //serviceResponse.Data = createdUser;
            serviceResponse.Success = true;
            serviceResponse.Message = CustomMessage.Added;
            return serviceResponse;

        }



        public async Task<ServiceResponse<string>> EditUser(int id, UserForUpdateDto user)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();



            User dbUser = _context.Users.FirstOrDefault(s => s.Id.Equals(id));
            if (dbUser != null)
            {
                dbUser.FullName = user.FullName;
                dbUser.Email = user.Email;
                dbUser.Username = user.Username.ToLower();
                dbUser.StateId = user.StateId;
                dbUser.CountryId = user.CountryId;
                dbUser.OtherState = user.OtherState;
                dbUser.DateofBirth = Convert.ToDateTime(user.DateofBirth);
                dbUser.Gender = user.Gender;
                dbUser.Active = user.Active;
                //dbUser.UserTypeId = user.UserTypeId;
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
                            throw new Exception("You didn't provide New Password");
                        }
                        //_mapper.Map(dbUser, userForAddDto);
                        //dbUser = _mapper.Map<User>(userForAddDto);
                        //dbUser = user;
                        //if (Seed.VerifyPasswordHash(userForAddDto.OldPassword, dbUser.PasswordHash, dbUser.PasswordSalt))
                        //_context.Users.Add(dbUser);
                        //_context.Entry(dbUser).State = EntityState.Modified;


                    }
                    else
                    {
                        throw new Exception(CustomMessage.PasswordNotMatched);
                    }

                }
                await _context.SaveChangesAsync();

                // saving images

                if (user.files != null && user.files.Count() > 0)
                {

                    string contentRootPath = _HostEnvironment.ContentRootPath;
                    var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");
                    for (int i = 0; i < user.files.Count(); i++)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.files[i].FileName);
                        //var fullPath = Path.Combine(pathToSave);
                        var dbPath = Path.Combine("StaticFiles", "Images", fileName); //you can add this path to a list and then return all dbPaths to the client if require
                        if (!Directory.Exists(pathToSave))
                        {
                            Directory.CreateDirectory(pathToSave);
                        }
                        var filePath = Path.Combine(pathToSave, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await user.files[i].CopyToAsync(stream);
                        }
                        if (user.IsPrimaryPhoto)
                        {
                            IQueryable<Photo> updatePhotos = _context.Photos.Where(m => m.UserId == dbUser.Id);
                            await updatePhotos.ForEachAsync(m => m.IsPrimary = false);
                        }
                        var photo = new Photo
                        {
                            Description = "description...",
                            IsPrimary = user.IsPrimaryPhoto,
                            UserId = dbUser.Id,
                            //DateAdded = DateTime.Now
                        };
                        if (i == 0)
                        {
                            photo.Url = dbPath;
                        }
                        else
                        {
                            photo.Url = photo.Url + " || " + dbPath;
                        }

                        await _context.Photos.AddAsync(photo);
                        await _context.SaveChangesAsync();
                    }
                }
                serviceResponse.Message = CustomMessage.Updated;
                serviceResponse.Success = true;
                return serviceResponse;
            }
            else
            {
                serviceResponse.Message = CustomMessage.RecordNotFound;
                serviceResponse.Success = false;
                return serviceResponse;
                //throw new Exception("Record Not Found");
            }


        }



        public async Task<IEnumerable<UserByTypeListDto>> GetUsersByType(int typeId, int? classSectionId)
        {
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
                                   select u).OrderByDescending(m => m.Id).Select(o => new UserByTypeListDto
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
                                       Photos = _context.Photos.Where(m => m.UserId == o.Id).ToList()
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
                foreach (var user in users)
                {
                    foreach (var item in user?.Photos)
                    {
                        item.Url = _File.AppendImagePath(item.Url);
                    }
                }
                return users;
            }
            else
            {
                var users = await (from u in _context.Users
                                   where u.UserTypeId == typeId
                                   && u.Active == true
                                   select u).OrderByDescending(m => m.Id).Select(o => new UserByTypeListDto
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
                                       Photos = _context.Photos.Where(m => m.UserId == o.Id).ToList()
                                   }).ToListAsync();
                foreach (var user in users)
                {
                    foreach (var item in user?.Photos)
                    {
                        item.Url = _File.AppendImagePath(item.Url);
                    }
                }

                return users;

            }

        }

        public async Task<ServiceResponse<IEnumerable<User>>> GetUnmappedStudents()
        {
            ServiceResponse<IEnumerable<User>> serviceResponse = new ServiceResponse<IEnumerable<User>>();
            IEnumerable<int> userIds = _context.ClassSectionUsers.Select(m => m.UserId).Distinct();
            List<User> unmappedStudents = await _context.Users.Where(m => m.UserTypeId == (int)Enumm.UserType.Student && !userIds.Contains(m.Id) && m.Active == true).ToListAsync();
            serviceResponse.Data = unmappedStudents;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> GetMappedStudents(int csId)
        {
            ServiceResponse<Object> serviceResponse = new ServiceResponse<object>();
            IEnumerable<int> userIds = _context.ClassSectionUsers.Where(m => m.ClassSectionId == csId).Select(m => m.UserId).Distinct();
            List<User> mappedStudents = await _context.Users.Where(m => userIds.Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Student && m.Active == true).ToListAsync();
            User mappedTeacher = await _context.Users.Where(m => userIds.Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Teacher && m.Active == true).FirstOrDefaultAsync();
            serviceResponse.Data = new { mappedStudents, mappedTeacher };
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public async Task<ServiceResponse<object>> AddUsersInGroup(UserForAddInGroupDto model)
        {

            var group = new Group
            {
                GroupName = model.GroupName,
                ClassSectionId = model.ClassSectionId,
                Active = true,
                SchoolBranchId = Convert.ToInt32(model.LoggedIn_BranchId)
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
                   od.Group.GroupName
               })
                .Select(s => s).Distinct().ToListAsync();


            var users = string.Empty;

            foreach (var item in result)
            {

                groupList.Add(new GroupListDto
                {
                    Id = item.Id,
                    GroupName = item.GroupName,
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
                                      .Select(p => p).ToListAsync();

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


            var group = await _context.Groups.Where(m => m.Id == model.Id).FirstOrDefaultAsync();
            if (group != null)
            {
                group.GroupName = model.GroupName;
                group.Active = model.Active ?? true;
                group.SchoolBranchId = Convert.ToInt32(model.LoggedIn_BranchId);

                await _context.SaveChangesAsync();
                if (model.UserIds.Count() > 0)
                {
                    List<GroupUser> listToDelete = await _context.GroupUsers.Where(m => m.GroupId == model.Id).ToListAsync();
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

            var group = _context.Groups.Where(m => m.Id == id).FirstOrDefault();
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

            var user = _context.Users.Where(m => m.Id == id).FirstOrDefault();
            user.Active = status;
            await _context.SaveChangesAsync();
            _serviceResponse.Success = true;
            _serviceResponse.Message = CustomMessage.Deleted;
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
                               select u).Include(m => m.Country).Include(m => m.State).Select(o => new UserForListDto
                               {
                                   Id = o.Id,
                                   FullName = o.FullName,
                                   DateofBirth = o.DateofBirth != null ? DateFormat.ToDate(o.DateofBirth.ToString()) : "",
                                   Email = o.Email,
                                   Gender = o.Gender,
                                   Username = o.Username,
                                   CountryId = o.CountryId,
                                   StateId = o.StateId,
                                   CountryName = o.Country.Name,
                                   StateName = o.State.Name,
                                   OtherState = o.OtherState,
                                   Active = o.Active,
                                   Photos = _context.Photos.Where(m => m.UserId == o.Id).ToList()
                               }).ToListAsync();

            foreach (var user in users)
            {
                foreach (var item in user?.Photos)
                {
                    item.Url = _File.AppendImagePath(item.Url);
                }
            }


            _serviceResponse.Success = true;
            _serviceResponse.Data = users;
            return _serviceResponse;
        }
    }
}

