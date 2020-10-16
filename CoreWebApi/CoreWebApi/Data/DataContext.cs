using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class DataContext : DbContext
    {

        

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<SchoolAcademy> SchoolAcademy { get; set; }
        public DbSet<SchoolBranch> SchoolBranch { get; set; }
        //public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<ClassSectionAssigmentSubmission> ClassSectionAssigmentSubmissions { get; set; }
        public DbSet<ClassSectionAssignment> ClassSectionAssignment { get; set; }
        //public DbSet<ClassSectionUserAssignment> ClassSectionUserAssignment { get; set; }
        //public DbSet<ClassSessionAssignment> ClassSessionAssignment { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public virtual DbSet<QuestionTypes> QuestionTypes { get; set; }
        public virtual DbSet<QuizAnswers> QuizAnswers { get; set; }
        public virtual DbSet<QuizQuestions> QuizQuestions { get; set; }
        public virtual DbSet<Quizzes> Quizzes { get; set; }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Value> Values { get; set; }
        public DbSet<ClassSection> ClassSections { get; set; }
        public DbSet<ClassSectionUser> ClassSectionUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }










    }

}
