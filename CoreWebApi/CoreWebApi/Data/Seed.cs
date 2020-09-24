using CoreWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class Seed
    {

        public static void SeedLeaveTypes(DataContext context)
        {
            try
            {
                if (!context.LeaveTypes.Any())
                {
                   
                    var fileData = System.IO.File.ReadAllText("Data/LeaveTypeSeedData.json");
                    var leaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(fileData);

                    foreach (var type in leaveTypes)
                    {
                        context.LeaveTypes.Add(type);
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void SeedUserTypes(DataContext context)
        {
            try
            {
                if (!context.UserTypes.Any())
                {
                    //var fileData = System.IO.File.ReadAllText("Data/UserTypeSeedData.json");
                    //DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(fileData);
                    //var jsonObj = JsonConvert.SerializeObject(dataSet.Tables["UserTypes"]);
                    //var userTypes = JsonConvert.DeserializeObject<List<UserType>>(jsonObj);
                    var fileData = System.IO.File.ReadAllText("Data/UserTypeSeedData.json");
                    var userTypes = JsonConvert.DeserializeObject<List<UserType>>(fileData);

                    foreach (var type in userTypes)
                    {
                        context.UserTypes.Add(type);
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void SeedUsers(DataContext context)
        {

            try
            {
                if (!context.Users.Any())
                {
                   
                    var fileData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                    var users = JsonConvert.DeserializeObject<List<User>>(fileData);

                    foreach (var user in users)
                    {
                        byte[] passwordhash, passwordSalt;
                        CreatePasswordHash("password", out passwordhash, out passwordSalt);
                        user.PasswordHash = passwordhash;
                        user.PasswordSalt = passwordSalt;
                        user.Username = user.Username.ToLower();
                        user.Email = "test@email";
                        user.FullName = "test name";
                        user.UserTypeId = context.UserTypes.FirstOrDefault().Id;
                        context.Users.Add(user);


                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            byte[] key = new Byte[64];
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


                // var hmac = System.Security.Cryptography.HMACSHA512()
            }

        }
        
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }

        }
    }
}
