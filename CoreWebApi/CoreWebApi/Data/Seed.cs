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

        private static int schoolBranchId = 0;
        public static void SeedLeaveTypes(DataContext context)
        {

            if (!context.LeaveType.Any())
            {

                var fileData = System.IO.File.ReadAllText("Data/LeaveTypeSeedData.json");
                var leaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(fileData);

                foreach (var type in leaveTypes)
                {

                    context.LeaveType.Add(type);
                }
                context.SaveChanges();

            }

        }
        public static void SeedUserTypes(DataContext context)
        {

            if (!context.UserTypes.Any())
            {

                var fileData = System.IO.File.ReadAllText("Data/UserTypeSeedData.json");
                var userTypes = JsonConvert.DeserializeObject<List<UserType>>(fileData);

                foreach (var type in userTypes)
                {
                    //type.Creatdatetime = DateTime.Now;
                    context.UserTypes.Add(type);
                }
                context.SaveChanges();

            }

        }

        public static void SeedSchoolAcademy(DataContext context)
        {

            if (!context.SchoolAcademy.Any())
            {

                //var fileData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                //var schoolAcademies = JsonConvert.DeserializeObject<List<SchoolAcademy>>(fileData);
                var schoolAcademies = context.SchoolAcademy.FirstOrDefault();

                if (schoolAcademies == null)
                {
                    var schoolAcademy = new SchoolAcademy();

                    schoolAcademy.Name = "LGS";
                    schoolAcademy.PrimaryContactPerson = "Qasim Rafi";
                    schoolAcademy.SecondaryContactPerson = "Ahsan Meraj";
                    schoolAcademy.PrimaryphoneNumber = "03217575840";
                    schoolAcademy.SecondaryphoneNumber = "03003207433";
                    schoolAcademy.Email = "Qasim@FabIntel.com";
                    schoolAcademy.PrimaryAddress = "Allama Iqbal Town";
                    schoolAcademy.SecondaryAddress = "Garden Tonw";
                    schoolAcademy.Active = true;

                    context.SchoolAcademy.Add(schoolAcademy);
                    context.SaveChanges();
                    int schoolAcademyId = schoolAcademy.Id;

                    if (schoolAcademyId > 0)
                    {

                        var schoolBranhes = new List<SchoolBranch>
                           {
                              new SchoolBranch { BranchName ="Dolphin",SchoolAcademyID = schoolAcademyId,CreatedDateTime = DateTime.Now, Active = true, RegistrationNumber = "10420001"},
                              new SchoolBranch { BranchName ="Jasmine",SchoolAcademyID = schoolAcademyId,CreatedDateTime = DateTime.Now, Active = true, RegistrationNumber = "10420002"}
                          };
                        context.AddRange(schoolBranhes);
                        context.SaveChanges();

                        schoolBranchId = schoolBranhes[0].Id;

                    }

                }



            }

        }


        public static void SeedUsers(DataContext context)
        {





            var schoolBranch = context.SchoolBranch.FirstOrDefault(m => m.Id == schoolBranchId);
            if (schoolBranch != null)
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
                    user.FullName = "test name 0" + (index + 1);
                    user.UserTypeId = context.UserTypes.FirstOrDefault(m => m.Name == "Student").Id;
                    user.CreatedDateTime = DateTime.Now;
                    user.SchoolBranchId = schoolBranchId;
                    user.RollNumber = "R-00" + (index + 1);
                    user.Active = true;
                    context.Users.Add(user);


                }
                context.SaveChanges();



            }

        }
        public static void SeedGenericData(DataContext context)
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
                    obj.SchoolBranchId = context.SchoolBranch.First().Id;
                    obj.CreatedById = context.Users.FirstOrDefault().Id;
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
                    item.CreatedById = context.Users.First().Id;
                    item.SchoolBranchId = context.SchoolBranch.First().Id;
                    context.Sections.Add(item);
                }

                context.SaveChanges();
            }
            //
            if (!context.Subjects.Any())
            {
                var SubjectsJson = JsonConvert.SerializeObject(dataSet.Tables["Subjects"]);
                var Subjects = JsonConvert.DeserializeObject<List<Subject>>(SubjectsJson);

                foreach (var (item, index) in ReturnIndex(Subjects))
                {
                    item.ClassId = context.Class.First().Id + index;

                    context.Subjects.Add(item);
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
            //
            if (!context.Countries.Any())
            {
                var CountriesJson = JsonConvert.SerializeObject(dataSet.Tables["Countries"]);
                var Countries = JsonConvert.DeserializeObject<List<Country>>(CountriesJson);
                foreach (var obj in Countries)
                {
                    context.Countries.Add(obj);
                }
                context.SaveChanges();
            }
            //
            if (!context.States.Any())
            {
                var StatesJson = JsonConvert.SerializeObject(dataSet.Tables["States"]);
                var States = JsonConvert.DeserializeObject<List<State>>(StatesJson);
                foreach (var (obj, index) in ReturnIndex(States))
                {
                    obj.CountryId = context.Countries.First().Id;

                    context.States.Add(obj);
                    if (index == (States.Count - 1))
                    {
                        var other = new State
                        {
                            Name = "Other",
                            CountryId = obj.CountryId,
                        };
                        context.States.Add(other);
                    }

                }
                context.SaveChanges();
            }

            //
            //if (!context.SchoolAcademy.Any())
            //{
            //    var SchoolAcademiesJson = JsonConvert.SerializeObject(dataSet.Tables["SchoolAcademies"]);
            //    var SchoolAcademies = JsonConvert.DeserializeObject<List<SchoolAcademy>>(SchoolAcademiesJson);
            //    foreach (var obj in SchoolAcademies)
            //    {
            //        obj.Email = "test@test.com";
            //        context.SchoolAcademy.Add(obj);
            //    }
            //    context.SaveChanges();
            //}
            //
            if (!context.QuestionTypes.Any())
            {
                var QuestionTypesJson = JsonConvert.SerializeObject(dataSet.Tables["QuestionTypes"]);
                var QuestionTypes = JsonConvert.DeserializeObject<List<QuestionTypes>>(QuestionTypesJson);
                foreach (var obj in QuestionTypes)
                {
                    obj.schoolBranchId = context.SchoolBranch.First().Id;

                    context.QuestionTypes.Add(obj);
                }
                context.SaveChanges();
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
