﻿// <auto-generated />
using System;
using CoreWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreWebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreWebApi.Models.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("Creationdatetime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedMaterial")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("CoreWebApi.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Absent")
                        .HasColumnType("bit");

                    b.Property<int>("ClassSectionUserAssignmentId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Late")
                        .HasColumnType("bit");

                    b.Property<bool>("Present")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("CoreWebApi.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int?>("SectionId")
                        .HasColumnType("int");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("CoreWebApi.Models.ClassSectionAssigmentSubmission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassAssignmentSectionId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int?>("classSectionAssignmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("dateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("documentPath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("classSectionAssignmentId");

                    b.ToTable("ClassSectionAssigmentSubmissions");
                });

            modelBuilder.Entity("CoreWebApi.Models.ClassSectionAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("SectionId");

                    b.HasIndex("SubjectId");

                    b.ToTable("ClassSectionAssignment");
                });

            modelBuilder.Entity("CoreWebApi.Models.ClassSectionUserAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AttendanceId")
                        .HasColumnType("int");

                    b.Property<int?>("ClassSectionAssignmentId")
                        .HasColumnType("int");

                    b.Property<int>("ClassSectionId")
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttendanceId");

                    b.HasIndex("ClassSectionAssignmentId");

                    b.HasIndex("SessionId")
                        .IsUnique();

                    b.HasIndex("UserTypeId")
                        .IsUnique();

                    b.ToTable("ClassSectionUserAssignment");
                });

            modelBuilder.Entity("CoreWebApi.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active1")
                        .HasColumnType("bit");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("CoreWebApi.Models.GroupUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("CoreWebApi.Models.Leave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeaveTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeaveTypeId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("CoreWebApi.Models.LeaveType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LeaveTypes");
                });

            modelBuilder.Entity("CoreWebApi.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AttachmentPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MessageFromUserId")
                        .HasColumnType("int");

                    b.Property<int>("MessageToUserId")
                        .HasColumnType("int");

                    b.Property<int>("ReplyMessageId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("CoreWebApi.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("message")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("CoreWebApi.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("bit");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("CoreWebApi.Models.SchoolAcademy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("PrimaryAddress")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("PrimaryContactPerson")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("PrimaryphoneNumber")
                        .HasColumnType("int")
                        .HasMaxLength(15);

                    b.Property<string>("SecondaryAddress")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("SecondaryContactPerson")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("SecondaryphoneNumber")
                        .HasColumnType("int")
                        .HasMaxLength(15);

                    b.Property<int?>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsersId");

                    b.ToTable("SchoolAcademy");
                });

            modelBuilder.Entity("CoreWebApi.Models.SchoolBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BranchName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SchoolAcademiesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SchoolAcademiesId");

                    b.ToTable("SchoolBranch");
                });

            modelBuilder.Entity("CoreWebApi.Models.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDatetime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SectionName")
                        .IsRequired()
                        .HasColumnName("SectionName")
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("CoreWebApi.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("SessionYear")
                        .IsRequired()
                        .HasColumnType("nvarchar(4)")
                        .HasMaxLength(4);

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("CoreWebApi.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("CoreWebApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateofBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastActive")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserTypeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CoreWebApi.Models.UserAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<bool>("IsPrimaryAddress")
                        .HasColumnType("bit");

                    b.Property<int?>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UsersId");

                    b.ToTable("UserAddress");
                });

            modelBuilder.Entity("CoreWebApi.Models.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Creatdatetime")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("UserTypes");
                });

            modelBuilder.Entity("CoreWebApi.Models.Value", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("CoreWebApi.Models.Assignment", b =>
                {
                    b.HasOne("CoreWebApi.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");
                });

            modelBuilder.Entity("CoreWebApi.Models.Attendance", b =>
                {
                    b.HasOne("CoreWebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.Class", b =>
                {
                    b.HasOne("CoreWebApi.Models.Section", null)
                        .WithMany("Classes")
                        .HasForeignKey("SectionId");

                    b.HasOne("CoreWebApi.Models.Subject", "Subject")
                        .WithMany("classes")
                        .HasForeignKey("SubjectId");
                });

            modelBuilder.Entity("CoreWebApi.Models.ClassSectionAssigmentSubmission", b =>
                {
                    b.HasOne("CoreWebApi.Models.ClassSectionAssignment", "classSectionAssignment")
                        .WithMany()
                        .HasForeignKey("classSectionAssignmentId");
                });

            modelBuilder.Entity("CoreWebApi.Models.ClassSectionAssignment", b =>
                {
                    b.HasOne("CoreWebApi.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreWebApi.Models.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreWebApi.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.ClassSectionUserAssignment", b =>
                {
                    b.HasOne("CoreWebApi.Models.Attendance", "Attendance")
                        .WithMany("ClassSectionUserAssignments")
                        .HasForeignKey("AttendanceId");

                    b.HasOne("CoreWebApi.Models.ClassSectionAssignment", "ClassSectionAssignment")
                        .WithMany()
                        .HasForeignKey("ClassSectionAssignmentId");

                    b.HasOne("CoreWebApi.Models.Session", "Session")
                        .WithOne("ClassSectionUserAssignment")
                        .HasForeignKey("CoreWebApi.Models.ClassSectionUserAssignment", "SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreWebApi.Models.UserType", "UserType")
                        .WithOne("ClassSectionUserAssignment")
                        .HasForeignKey("CoreWebApi.Models.ClassSectionUserAssignment", "UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.GroupUser", b =>
                {
                    b.HasOne("CoreWebApi.Models.Group", "Groups")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.Leave", b =>
                {
                    b.HasOne("CoreWebApi.Models.LeaveType", "LeaveType")
                        .WithOne("Leave")
                        .HasForeignKey("CoreWebApi.Models.Leave", "LeaveTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreWebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.Message", b =>
                {
                    b.HasOne("CoreWebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CoreWebApi.Models.Notification", b =>
                {
                    b.HasOne("CoreWebApi.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.Photo", b =>
                {
                    b.HasOne("CoreWebApi.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.SchoolAcademy", b =>
                {
                    b.HasOne("CoreWebApi.Models.User", "Users")
                        .WithMany("SchoolAcademies")
                        .HasForeignKey("UsersId");
                });

            modelBuilder.Entity("CoreWebApi.Models.SchoolBranch", b =>
                {
                    b.HasOne("CoreWebApi.Models.SchoolAcademy", "SchoolAcademies")
                        .WithMany("SchoolBranches")
                        .HasForeignKey("SchoolAcademiesId");
                });

            modelBuilder.Entity("CoreWebApi.Models.Session", b =>
                {
                    b.HasOne("CoreWebApi.Models.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");
                });

            modelBuilder.Entity("CoreWebApi.Models.User", b =>
                {
                    b.HasOne("CoreWebApi.Models.Group", null)
                        .WithMany("Users")
                        .HasForeignKey("GroupId");

                    b.HasOne("CoreWebApi.Models.UserType", "Usertypes")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreWebApi.Models.UserAddress", b =>
                {
                    b.HasOne("CoreWebApi.Models.User", "Users")
                        .WithMany("UserAddresses")
                        .HasForeignKey("UsersId");
                });
#pragma warning restore 612, 618
        }
    }
}
