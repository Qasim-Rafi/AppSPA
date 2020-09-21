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

namespace CoreWebApi.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

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


        public async Task<User> AddUser(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            Seed.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
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
                    if (dbUser != null)
                        await _context.SaveChangesAsync();

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
