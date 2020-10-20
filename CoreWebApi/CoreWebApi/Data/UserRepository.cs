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

        public UserRepository(DataContext context, IMapper mapper, IWebHostEnvironment HostEnvironment, IFilesRepository file)
        {
            _context = context;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
            _File = file;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(Task entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id && u.Active == true);
            foreach (var item in user?.Photos)
            {
                item.Url = _File.AppendImagePath(item.Url);
            }
            return user;

        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Where(m => m.Active == true).Include(p => p.Photos).Include(m => m.Country).Include(m => m.State).ToListAsync();
            foreach (var user in users)
            {
                foreach (var item in user?.Photos)
                {
                    item.Url = _File.AppendImagePath(item.Url);
                }
            }
            return users;

        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }


        public async Task<User> AddUser(UserForAddDto userDto)
        {
            try
            {
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

                return userToCreate;
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }



        public async Task<string> EditUser(int id, UserForUpdateDto user)
        {

            try
            {

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
                            throw new Exception("Password does not match");
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
                                photo.Url = dbPath;
                            else
                                photo.Url = photo.Url + " || " + dbPath;

                            await _context.Photos.AddAsync(photo);
                            await _context.SaveChangesAsync();
                        }
                    }
                    return StatusCodes.Status200OK.ToString();
                }
                else
                {
                    throw new Exception("Record Not Found");
                }

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw ex;
            }
        }



        public async Task<IEnumerable<User>> GetUsersByType(int typeId, int? classSectionId)
        {
            try
            {
                if (!string.IsNullOrEmpty(classSectionId.ToString()))
                {
                    IEnumerable<int> studentIds = _context.ClassSectionUsers.Where(m => m.ClassSectionId == classSectionId).Select(m => m.UserId).Distinct();
                    var users = await _context.Users.Where(m => m.UserTypeId == typeId && studentIds.Contains(m.Id) && m.Active == true).Include(p => p.Photos).ToListAsync();
                    //var users = await (from u in _context.Users
                    //                   join csU in _context.ClassSectionUsers
                    //                   on u.Id equals csU.UserId
                    //                   where u.UserTypeId == typeId
                    //                   && csU.ClassSectionId == classSectionId
                    //                   && u.Active == true
                    //                   select u).Include(p => p.Photos).ToListAsync();
                    foreach (var user in users)
                    {
                        foreach (var item in user.Photos)
                        {
                            item.Url = _File.AppendImagePath(item.Url);
                        }
                    }
                    return users;
                }
                else
                {
                    var users = await _context.Users.Where(m => m.UserTypeId == typeId && m.Active == true).Include(p => p.Photos).ToListAsync();
                    foreach (var user in users)
                    {
                        foreach (var item in user.Photos)
                        {
                            item.Url = _File.AppendImagePath(item.Url);
                        }
                    }
                    return users;

                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw ex;
            }
        }

        public async Task<IEnumerable<User>> GetUnmappedStudents()
        {

            IEnumerable<int> userIds = _context.ClassSectionUsers.Select(m => m.UserId).Distinct();
            List<User> unmappedStudents = await _context.Users.Where(m => m.UserTypeId == (int)Enumm.UserType.Student && !userIds.Contains(m.Id) && m.Active == true).ToListAsync();
            return unmappedStudents;
        }

        public async Task<object> GetMappedStudents(int csId)
        {

            IEnumerable<int> userIds = _context.ClassSectionUsers.Where(m => m.ClassSectionId == csId).Select(m => m.UserId).Distinct();
            List<User> mappedStudents = await _context.Users.Where(m => userIds.Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Student && m.Active == true).ToListAsync();
            User mappedTeacher = await _context.Users.Where(m => userIds.Contains(m.Id) && m.UserTypeId == (int)Enumm.UserType.Teacher && m.Active == true).FirstOrDefaultAsync();
            return new { mappedStudents, mappedTeacher };
        }

        public async Task<bool> AddUsersInGroup(UserForAddInGroupDto model)
        {
            var group = new Group
            {
                GroupName = model.GroupName,
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
            return true;
        }

        public async Task<object> GetGroupUsers()
        {

            var users = (await _context.GroupUsers
                .Include(m => m.Group).Include(m => m.User).ToListAsync())
                .GroupBy(m => m.Group.GroupName)
                .ToDictionary(e => e.Key, e => e.Select(e2 => new
                {
                    Id = e2.User.Id,
                    FullName = e2.User.FullName
                }));
            return users;
        }

        public async Task<bool> UpdatesersInGroup(UserForAddInGroupDto model)
        {
            var group = await _context.Groups.Where(m => m.Id == model.Id).FirstOrDefaultAsync();

            group.GroupName = model.GroupName;
            group.Active = model.Active ?? true;
            group.SchoolBranchId = Convert.ToInt32(model.LoggedIn_BranchId);

            await _context.SaveChangesAsync();

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
            return true;
        }

        public async Task<object> GetGroupUsersById(int id)
        {
            var gUser = await _context.GroupUsers.Where(m => m.GroupId == id)
                .Include(m => m.Group).Include(m => m.User).ToListAsync();

            return gUser;
        }


    }
}

