using CoreWebApi.Dtos;
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
        public DbSet<EventDayAssignment> EventDaysAssignments { get; set; }
        public DbSet<UploadedLecture> UploadedLectures { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<SubjectContent> SubjectContents { get; set; }
        public DbSet<SubjectContentDetail> SubjectContentDetails { get; set; }
        public DbSet<ClassSectionTransaction> ClassSectionTransactions { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Planner> Planners { get; set; }
        public DbSet<Substitution> Substitutions { get; set; }
        public DbSet<TeacherExperties> TeacherExperties { get; set; }
        public DbSet<TeacherExpertiesTransaction> TeacherExpertiesTransactions { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<ContactUsQuery> ContactUsQueries { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<UsefulResource> UsefulResources { get; set; }
        public DbSet<MessageReply> MessageReplies { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<StudentFee> StudentFees { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public DbSet<EmployeeSalaryTransaction> EmployeeSalaryTransactions { get; set; }
        public DbSet<StaffInventory> StaffInventory { get; set; }
        public DbSet<SchoolCashAccount> SchoolCashAccount { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SemesterFeeMapping> SemesterFeeMappings { get; set; }
        public DbSet<SemesterFeeTransaction> SemesterFeeTransactions { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<FeeVoucherDetail> FeeVoucherDetails { get; set; }
        public DbSet<FeeVoucherRecord> FeeVoucherRecords { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        //
        public DbSet<GetAttendancePercentageByMonthDto> SPGetAttendancePercentageByMonth { get; set; }
        public DbSet<GetSubstituteTeachersDto> SPGetSubstituteTeachers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (IMutableForeignKey relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // while adding new migration uncomment below line and then comment it again after...
            // modelBuilder.Ignore<GetAttendancePercentageByMonthDto>();
            modelBuilder.Ignore<GetSubstituteTeachersDto>();

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
            modelBuilder.Entity("CoreWebApi.Models.EventDayAssignment", b =>
            {
                b.HasOne("CoreWebApi.Models.Event", "Event")
                    .WithMany()
                    .HasForeignKey("EventId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            //
            modelBuilder.Entity("CoreWebApi.Models.SubjectContentDetail", b =>
            {
                b.HasOne("CoreWebApi.Models.SubjectContent", "SubjectContent")
                    .WithMany()
                    .HasForeignKey("SubjectContentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
            //
            modelBuilder.Entity<User>()
                .Property(user => user.Role).HasDefaultValue("Student");
            // composite primary key

            modelBuilder.Entity<ClassLectureAssignment>()
                .HasIndex(p => new { p.LectureId, p.TeacherId })//, p.ClassSectionId
                .IsUnique(true);
            // composite primary key
            modelBuilder.Entity<ClassSection>()
              .HasIndex(p => new { p.ClassId, p.SectionId, p.SchoolBranchId })
              .IsUnique(true);
            modelBuilder.Entity<ClassSection>()
              .HasIndex(p => new { p.SemesterId, p.SectionId, p.SchoolBranchId })
              .IsUnique(true);
            // primary key // composite primary key
            //modelBuilder.Entity<ClassSectionUser>()
            //    .HasKey(c => new { c.Id });
            modelBuilder.Entity<ClassSectionUser>()
                .HasIndex(p => new { p.SchoolBranchId, p.UserId })
                .IsUnique(true);

            // composite primary key
            modelBuilder.Entity<Subject>()
               .HasIndex(s => new { s.Name, s.SchoolBranchId })
               .IsUnique(true);
            // composite primary key
            modelBuilder.Entity<SubjectAssignment>()
               .HasIndex(s => new { s.ClassId, s.SubjectId, s.SchoolBranchId })
               .IsUnique(true);
            modelBuilder.Entity<SubjectAssignment>()
               .HasIndex(s => new { s.SemesterId, s.SubjectId, s.SchoolBranchId })
               .IsUnique(true);
            // composite primary key
            modelBuilder.Entity<User>()
               .HasIndex(s => new { s.Username, s.SchoolBranchId })
               .IsUnique(true);
            // composite primary key
            modelBuilder.Entity<Event>()
               .HasIndex(s => new { s.Title, s.SchoolBranchId })
               .IsUnique(true);
        }


    }

}
