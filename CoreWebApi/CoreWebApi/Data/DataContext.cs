using CoreWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreWebApi.Data
{
    public class DataContext : DbContext
    {



        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<SchoolAcademy> SchoolAcademy { get; set; }
        public DbSet<SchoolBranch> SchoolBranch { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
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
        public virtual DbSet<QuizSubmission> QuizSubmissions { get; set; }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        public DbSet<Value> Values { get; set; }
        public DbSet<ClassSection> ClassSections { get; set; }
        public DbSet<ClassSectionUser> ClassSectionUsers { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LectureTiming> LectureTiming { get; set; }
        public DbSet<ClassLectureAssignment> ClassLectureAssignment { get; set; }
        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //
            modelBuilder.Entity("CoreWebApi.Models.GroupUser", b =>
            {
                b.HasOne("CoreWebApi.Models.Group", "Group")
                    .WithMany()
                    .HasForeignKey("GroupId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("CoreWebApi.Models.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            //
            modelBuilder.Entity("CoreWebApi.Models.ClassSectionUser", b =>
            {
                b.HasOne("CoreWebApi.Models.ClassSection", "ClassSection")
                         .WithMany()
                         .HasForeignKey("ClassSectionId")
                         .OnDelete(DeleteBehavior.Cascade)
                         .IsRequired();
            });
            //
            modelBuilder.Entity<User>()
                .Property(user => user.Role).HasDefaultValue("Student");
            // composite primary key
            modelBuilder.Entity<ClassLectureAssignment>()
                 .HasKey(c => new { c.LectureId, c.TeacherId });
            // modelBuilder.Entity<ClassLectureAssignment>()
            // .Property(p => p.TeacherId).ValueGeneratedNever();
            // modelBuilder.Entity<ClassLectureAssignment>()
            // .Property(p => p.LectureId).ValueGeneratedNever();
        }


    }

}
