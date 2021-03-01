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
            UserTypeId
        }
        public enum DocumentType
        {
            Assignment = 1,
            Quiz
        }
        public static class RequisitionStatus
        {
            public static string Approved { get; set; } = "Approved";
            public static string Rejected { get; set; } = "Rejected";
            public static string Pending { get; set; } = "Pending";
        }
        public static class LeaveStatus
        {
            public static string Approved { get; set; } = "Approved";
            public static string Rejected { get; set; } = "Rejected";
            public static string Pending { get; set; } = "Pending";
        }

    }

}
