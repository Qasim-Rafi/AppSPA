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
            SuperAdmin = 1,
            Admin,
            Parent,
            Student,
            Teacher,
            Tutor,
            OnlineStudent
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
    }

}
