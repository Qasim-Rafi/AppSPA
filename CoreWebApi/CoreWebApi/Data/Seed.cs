using CoreWebApi.Helpers;
using CoreWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public static class Seed
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

                Log.Exception(ex);
                throw ex;
            }
        }
        public static void SeedUserTypes(DataContext context)
        {
            try
            {
                if (!context.UserTypes.Any())
                {

                    var fileData = System.IO.File.ReadAllText("Data/UserTypeSeedData.json");
                    var userTypes = JsonConvert.DeserializeObject<List<UserType>>(fileData);

                    foreach (var type in userTypes)
                    {
                        type.Creatdatetime = DateTime.Now;
                        context.UserTypes.Add(type);
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
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

                    foreach (var (user, index) in ReturnIndex(users))
                    {

                        byte[] passwordhash, passwordSalt;
                        CreatePasswordHash("password", out passwordhash, out passwordSalt);
                        user.PasswordHash = passwordhash;
                        user.PasswordSalt = passwordSalt;
                        user.Username = user.Username.ToLower();
                        user.Email = "test@email";
                        user.FullName = "test name " + (index + 1);
                        user.UserTypeId = context.UserTypes.FirstOrDefault(m => m.Name == "Student").Id;
                        user.CreatedDateTime = DateTime.Now;
                        user.Active = true;
                        context.Users.Add(user);


                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }
        public static void SeedGenericData(DataContext context)
        {
            try
            {
                var fileData = System.IO.File.ReadAllText("Data/GenericSeedData.json");
                DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(fileData);
                //
                if (!context.Class.Any())
                {
                    var ClassesJson = JsonConvert.SerializeObject(dataSet.Tables["Classes"]);
                    var Classes = JsonConvert.DeserializeObject<List<Class>>(ClassesJson);
                    foreach (var obj in Classes)
                    {
                        obj.Active = true;
                        context.Class.Add(obj);
                    }
                    context.SaveChanges();
                }
                //
                if (!context.Sections.Any())
                {
                    var SectionsJson = JsonConvert.SerializeObject(dataSet.Tables["Sections"]);
                    var Sections = JsonConvert.DeserializeObject<List<Section>>(SectionsJson);

                    foreach (var item in Sections)
                    {

                        context.Sections.Add(item);
                    }

                    context.SaveChanges();
                }
                //
                if (!context.ClassSections.Any())
                {
                    var GetClasses = context.Class.Where(m => m.Active == true).ToList();
                    var GetSections = context.Sections.ToList();
                    foreach (var obj in GetClasses)
                    {
                        foreach (var item in GetSections)
                        {
                            var newobj = new ClassSection
                            {
                                ClassId = obj.Id,
                                SectionId = item.Id,
                                Active = true
                            };
                            context.ClassSections.Add(newobj);
                        }

                    }
                    context.SaveChanges();
                }
                //
                if (!context.ClassSectionUsers.Any())
                {
                    var GetClassSections = context.ClassSections.AsEnumerable();
                    var StudentID = context.UserTypes.FirstOrDefault(m => m.Name == "Student").Id;
                    var TeacherID = context.UserTypes.FirstOrDefault(m => m.Name == "Teacher").Id;
                    var GetStudentUsers = context.Users.Where(m => m.UserTypeId == StudentID).ToList();
                    var GetTeacherUsers = context.Users.Where(m => m.UserTypeId == TeacherID).ToList();
                    int half = GetClassSections.Count() / 2;
                    foreach (var (obj, index) in ReturnIndex(GetClassSections))
                    {
                        if (index < half)
                        {
                            foreach (var item in GetStudentUsers)
                            {
                                var newobj = new ClassSectionUser
                                {
                                    ClassSectionId = obj.Id,
                                    UserId = item.Id
                                };
                                context.ClassSectionUsers.Add(newobj);
                            }
                        }
                        else
                        {
                            foreach (var item in GetTeacherUsers)
                            {
                                var newobj = new ClassSectionUser
                                {
                                    ClassSectionId = obj.Id,
                                    UserId = item.Id
                                };
                                context.ClassSectionUsers.Add(newobj);
                            }
                        }
                    }
                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                Log.Exception(ex);
                throw ex;
            }
        }
        public static IEnumerable<(T item, int index)> ReturnIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
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
