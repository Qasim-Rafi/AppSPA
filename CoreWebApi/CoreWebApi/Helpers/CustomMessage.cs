using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class CustomMessage
    {
        public const string RecordNotFound = "Record Not Found";
        public const string Added = "Record Added Successfully";
        public const string Updated = "Record Updated Successfully";
        public const string Deleted = "Record Deleted Successfully";
        public const string PasswordNotMatched = "Given password does not match";
        public const string UnableToAdd = "Unable to Add Data";
        public const string UserAlreadyExist = "User Already Exist";
    }
}
