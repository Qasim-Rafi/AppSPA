using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class CustomMessage
    {
        public const string RecordNotFound = "Record not found";
        public const string Added = "Record added successfully";
        public const string Updated = "Record updated successfully";
        public const string Deleted = "Record deleted successfully";
        public const string PasswordNotMatched = "Given password does not match";
        public const string UnableToAdd = "Unable to add data";
        public const string UserAlreadyExist = "User already exist";
        public const string ChildRecordExist = "Record can't be deleted, because it's child records exist in another table";
    }
}
