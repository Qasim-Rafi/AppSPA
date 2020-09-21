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

        public static void SeedUserTypes(DataContext context)
        {
            try
            {
                if (!context.Users.Any())
                {
                    var fileData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(fileData);
                    var jsonObj = JsonConvert.SerializeObject(dataSet.Tables["UserTypes"]);
                    var userTypes = JsonConvert.DeserializeObject<List<UserType>>(jsonObj);

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
                    DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(fileData);
                    var jsonObj = JsonConvert.SerializeObject(dataSet.Tables["Users"]);
                    var users = JsonConvert.DeserializeObject<List<User>>(jsonObj);

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

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            byte[] key = new Byte[64];
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));


                // var hmac = System.Security.Cryptography.HMACSHA512()
            }

        }

    }
}
