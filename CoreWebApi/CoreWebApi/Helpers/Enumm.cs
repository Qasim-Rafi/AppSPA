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
        public class InventoryType
        {
            public const string Credit = "Credit";
            public const string Debit = "Debit";
        }

    }

}
