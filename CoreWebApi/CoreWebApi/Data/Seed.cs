﻿using CoreWebApi.Helpers;
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
        
        public static void SeedUserTypes(DataContext context)
        {

            if (!context.UserTypes.Any())
            {

                var fileData = System.IO.File.ReadAllText("Data/UserTypeSeedData.json");
                var userTypes = JsonConvert.DeserializeObject<List<UserType>>(fileData);

                foreach (var type in userTypes)
                {
                    //type.Creatdatetime = DateTime.UtcNow;
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
                    var schoolAcademy = new SchoolAcademy
                    {
                        Name = "School-01",//"LGS";
                        PrimaryContactPerson = "FabIntel",// "Qasim Rafi";
                        SecondaryContactPerson = "FabIntel2",// "Ahsan Meraj";
                        PrimaryphoneNumber = "0000-0000000",//"03217575840";
                        SecondaryphoneNumber = "0000-0000000",// "03003207433";
                        Email = "Email@OnlineAcademy.com",// "Qasim@FabIntel.com";
                        PrimaryAddress = "Garden Town",// "Allama Iqbal Town";
                        SecondaryAddress = "Lahore",// "Garden Tonw";
                        Active = true
                    };

                    context.SchoolAcademy.Add(schoolAcademy);
                    context.SaveChanges();
                    int schoolAcademyId = schoolAcademy.Id;

                    if (schoolAcademyId > 0)
                    {

                        var schoolBranch = new SchoolBranch { BranchName = "Online Academy", SchoolAcademyID = schoolAcademyId, CreatedDateTime = DateTime.UtcNow, Active = true, RegistrationNumber = "20000001" };

                        context.SchoolBranch.Add(schoolBranch);
                        context.SaveChanges();

                        schoolBranchId = schoolBranch.Id;

                    }

                }



            }

        }


        public static void SeedUsers(DataContext context)
        {

            if (!context.Users.Any())
            {
                var schoolBranch = context.SchoolBranch.FirstOrDefault();
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
                        user.DateofBirth = DateTime.UtcNow;
                        user.Email = "test@email";
                        user.FullName = "test name 0" + (index + 1);
                        user.UserTypeId = index == 0 ? context.UserTypes.FirstOrDefault(m => m.Name == "Admin").Id : context.UserTypes.FirstOrDefault(m => m.Name == "Student").Id;
                        user.CreatedDateTime = DateTime.UtcNow;
                        user.SchoolBranchId = schoolBranch.Id;
                        user.RollNumber = index == 0 ? null : "R-00" + (index + 1);
                        user.Active = true;
                        user.Role = index == 0 ? context.UserTypes.FirstOrDefault(m => m.Name == "Admin").Name : context.UserTypes.FirstOrDefault(m => m.Name == "Student").Name;
                        context.Users.Add(user);


                    }
                    context.SaveChanges();



                }
            }
        }
        public static void SeedGenericData(DataContext context)
        {

            var fileData = System.IO.File.ReadAllText("Data/GenericSeedData.json");
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(fileData);
            var schoolBranch = context.SchoolBranch.FirstOrDefault();
            if (schoolBranch != null)
            {
                ////
                //if (!context.Class.Any())
                //{
                //    var ClassesJson = JsonConvert.SerializeObject(dataSet.Tables["Classes"]);
                //    var Classes = JsonConvert.DeserializeObject<List<Class>>(ClassesJson);
                //    foreach (var obj in Classes)
                //    {
                //        obj.Active = true;
                //        obj.SchoolBranchId = context.SchoolBranch.First().Id;
                //        obj.CreatedById = context.Users.FirstOrDefault().Id;
                //        context.Class.Add(obj);
                //    }
                //    context.SaveChanges();
                //}
                ////
                //if (!context.Sections.Any())
                //{
                //    var SectionsJson = JsonConvert.SerializeObject(dataSet.Tables["Sections"]);
                //    var Sections = JsonConvert.DeserializeObject<List<Section>>(SectionsJson);

                //    foreach (var item in Sections)
                //    {
                //        item.CreatedById = context.Users.First().Id;
                //        item.SchoolBranchId = context.SchoolBranch.First().Id;
                //        context.Sections.Add(item);
                //    }

                //    context.SaveChanges();
                //}
                ////
                //if (!context.Subjects.Any())
                //{
                //    var SubjectsJson = JsonConvert.SerializeObject(dataSet.Tables["Subjects"]);
                //    var Subjects = JsonConvert.DeserializeObject<List<Subject>>(SubjectsJson);

                //    foreach (var (item, index) in ReturnIndex(Subjects))
                //    {
                //        item.CreditHours = 3;
                //        item.Active = true;
                //        item.SchoolBranchId = schoolBranch.Id;
                //        context.Subjects.Add(item);
                //    }

                //    context.SaveChanges();
                //}
                ////
                //if (!context.ClassSections.Any())
                //{
                //    var GetClasses = context.Class.Where(m => m.Active == true).ToList();
                //    var GetSections = context.Sections.ToList();
                //    //var schoolBranch = context.SchoolBranch.FirstOrDefault();

                //    foreach (var obj in GetClasses)
                //    {
                //        foreach (var item in GetSections)
                //        {
                //            var newobj = new ClassSection
                //            {
                //                ClassId = obj.Id,
                //                SectionId = item.Id,
                //                SchoolBranchId = schoolBranch != null ? schoolBranch.Id : 0,
                //                Active = true
                //            };
                //            context.ClassSections.Add(newobj);
                //        }

                //    }
                //    context.SaveChanges();
                //}
                ////
                //if (!context.ClassSectionUsers.Any())
                //{
                //    var GetClassSection = context.ClassSections.AsEnumerable().First();
                //    var StudentID = context.UserTypes.FirstOrDefault(m => m.Name == "Student").Id;
                //    var TeacherID = context.UserTypes.FirstOrDefault(m => m.Name == "Teacher").Id;
                //    var GetStudentUsers = context.Users.Where(m => m.UserTypeId == StudentID).ToList();
                //    var GetTeacherUsers = context.Users.Where(m => m.UserTypeId == TeacherID).ToList();
                //    //int half = GetClassSections.Count() / 2;
                //    //foreach (var (obj, index) in ReturnIndex(GetClassSections.First()))
                //    //{
                //    //   
                //    foreach (var item in GetStudentUsers)
                //    {
                //        var newobj = new ClassSectionUser
                //        {
                //            ClassSectionId = GetClassSection.Id,
                //            UserId = item.Id,
                //            SchoolBranchId = schoolBranch.Id
                //        };
                //        context.ClassSectionUsers.Add(newobj);
                //    }

                //    foreach (var item in GetTeacherUsers)
                //    {
                //        var newobj = new ClassSectionUser
                //        {
                //            ClassSectionId = GetClassSection.Id,
                //            UserId = item.Id,
                //            SchoolBranchId = schoolBranch.Id
                //        };
                //        context.ClassSectionUsers.Add(newobj);
                //    }
                //    context.SaveChanges();
                //    //}
                //}

                //
                //if (!context.Countries.Any())
                //{
                //    var CountriesJson = JsonConvert.SerializeObject(dataSet.Tables["Countries"]);
                //    var Countries = JsonConvert.DeserializeObject<List<Country>>(CountriesJson);
                //    foreach (var obj in Countries)
                //    {
                //        context.Countries.Add(obj);
                //    }
                //    context.SaveChanges();
                //}
                ////
                //if (!context.States.Any())
                //{
                //    var StatesJson = JsonConvert.SerializeObject(dataSet.Tables["States"]);
                //    var States = JsonConvert.DeserializeObject<List<State>>(StatesJson);
                //    foreach (var (obj, index) in ReturnIndex(States))
                //    {
                //        obj.CountryId = context.Countries.First().Id;

                //        context.States.Add(obj);
                //        if (index == (States.Count - 1))
                //        {
                //            var other = new State
                //            {
                //                Name = "Other",
                //                CountryId = obj.CountryId,
                //            };
                //            context.States.Add(other);
                //        }

                //    }
                //    context.SaveChanges();
                //}
              
                //
                if (!context.LeaveType.Any())
                {
                    var LeaveTypesJson = JsonConvert.SerializeObject(dataSet.Tables["LeaveTypes"]);
                    var leaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(LeaveTypesJson);

                    foreach (var type in leaveTypes)
                    {
                        context.LeaveType.Add(type);
                    }
                    context.SaveChanges();
                }
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
                //
                if (!context.ExamTypes.Any())
                {
                    var ExamTypesJson = JsonConvert.SerializeObject(dataSet.Tables["ExamTypes"]);
                    var ExamTypes = JsonConvert.DeserializeObject<List<ExamType>>(ExamTypesJson);
                    foreach (var obj in ExamTypes)
                    {                        
                        context.ExamTypes.Add(obj);
                    }
                    context.SaveChanges();
                }

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
