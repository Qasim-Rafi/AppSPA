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
            Supervisor = 1,
            Admin,
            Student,
            Teacher
        }
        public enum ClaimType
        {
            NameIdentifier = 1,
            Name,
            BranchIdentifier,
        }
    }
    
}
