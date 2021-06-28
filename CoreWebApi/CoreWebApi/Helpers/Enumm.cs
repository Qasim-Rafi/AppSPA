using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class Enumm
    {
        public enum UserType
        {
            Admin = 1,
            Parent,
            Student,
            Teacher,
            Tutor,
            OnlineStudent,
            SuperAdmin
        }
        public enum ClaimType
        {
            NameIdentifier = 1,
            Name,
            BranchIdentifier,
            ExamType
        }
        public enum DocumentType
        {
            Assignment = 1,
            Quiz
        }
        public enum ExamTypes
        {
            Annual = 1,
            Semester
        }
        public enum TransactionTypes
        {
            Debit,
            Credit
        }
        public class RequisitionStatus
        {
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Pending = "Pending";
        }
        public class LeaveStatus
        {
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";
            public const string Pending = "Pending";
        }
        public enum NotifyTo
        {
            Student,
            Teacher,
            Parent,
            Admin
        }
    }
    public static class AppRoles
    {
        public const string All = "SuperAdmin, Admin, Teacher, Student, Parent, Tutor, OnlineStudent";
        public const string ForSchool = "SuperAdmin, Admin, Teacher, Student, Parent";
        public const string ForTutorship = "SuperAdmin, Admin, Tutor, OnlineStudent";
        public const string SuperAdmin_Admin = "SuperAdmin, Admin";
        public const string Teacher_Student_Parent = "Teacher, Student, Parent";
        public const string Tutor_OnlineStudent = "Tutor, OnlineStudent";
    }
}
