using CoreWebApi.Dtos;
using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CoreWebApi.Data;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CoreWebApi.IData;

namespace CoreWebApi.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _HostEnvironment;
        public UserRepository(DataContext context, IMapper mapper, IWebHostEnvironment HostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _HostEnvironment = HostEnvironment;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(Task entity) where T : class
        {
            _context.Remove(entity);
        }

        public Task<User> GetUser(int id)
        {
            var user = _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
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
                    Username = userDto.Username,
                    DateofBirth = Convert.ToDateTime(userDto.DateofBirth),
                    LastActive = Convert.ToDateTime(DateTime.Now),
                    City = "Lahore",
                    Country = "Pakistan",
                    Email = userDto.Email,
                    UserTypeId = _context.UserTypes.First().Id,
                    CreatedTimestamp = DateTime.Now,
                    Gender = userDto.Gender

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

                throw ex;
            }
        }



        public async Task<User> EditUser(int id, UserForAddDto user)
        {

            try
            {

                User dbUser = _context.Users.FirstOrDefault(s => s.Id.Equals(id));
                if (dbUser != null)
                {
                    dbUser.FullName = user.FullName;
                    dbUser.Email = user.Email;
                    dbUser.Username = user.Username.ToLower();
                    dbUser.City = user.City;
                    dbUser.Country = user.Country;
                    dbUser.DateofBirth = user.DateofBirth;
                    dbUser.Gender = user.Gender;
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

                    //// saving images
                    //if (user.files.Any(f => f.Length == 0))
                    //{
                    //    throw new Exception("No files found");
                    //}
                    //string contentRootPath = _HostEnvironment.ContentRootPath;

                    //var pathToSave = Path.Combine(contentRootPath, "StaticFiles", "Images");

                    //foreach (var file in user.files)
                    //{
                    //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //    var fullPath = Path.Combine(pathToSave);
                    //    var dbPath = Path.Combine("StaticFiles", "Images", fileName); //you can add this path to a list and then return all dbPaths to the client if require
                    //    if (!Directory.Exists(fullPath))
                    //    {
                    //        Directory.CreateDirectory(fullPath);
                    //    }
                    //    var filePath = Path.Combine(fullPath, fileName);
                    //    using (var stream = new FileStream(filePath, FileMode.Create))
                    //    {
                    //        await file.CopyToAsync(stream);
                    //    }
                    //    if (user.IsPrimaryPhoto)
                    //    {
                    //        IQueryable<Photo> updatePhotos = _context.Photos.Where(m => m.UserId == dbUser.Id);
                    //        await updatePhotos.ForEachAsync(m => m.IsPrimary = false);
                    //    }
                    //    var photo = new Photo
                    //    {
                    //        Url = dbPath,
                    //        Description = "description...",
                    //        IsPrimary = user.IsPrimaryPhoto,
                    //        UserId = dbUser.Id,
                    //        DateAdded = DateTime.Now
                    //    };
                    //    await _context.Photos.AddAsync(photo);
                    //    await _context.SaveChangesAsync();
                    //}
                    return dbUser;
                }
                else
                {
                    throw new Exception("Record Not Found");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        //{
        //    byte[] key = new Byte[64];
        //    using (HMACSHA512 hmac = new HMACSHA512(key))
        //    {
        //        passwordSalt = hmac.Key;
        //        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


        //        // var hmac = System.Security.Cryptography.HMACSHA512()
        //    }

        //}


    }
}
