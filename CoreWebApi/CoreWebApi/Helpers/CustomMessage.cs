using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Helpers
{
    public class CustomMessage
    {
        public const string RecordNotFound = "Record not found";
        public const string Added = "Record(s) added successfully";
        public const string Updated = "Record(s) updated successfully";
        public const string Deleted = "Record(s) deleted successfully";
        public const string RecordDeActivated = "Record updated and de-activated successfully";
        public const string PasswordNotMatched = "Given password does not match";
        public const string UnableToAdd = "Unable to add data";
        public const string SomeErrorOccured = "Some error occured";
        public const string UserAlreadyExist = "User already exist";
        public const string RecordAlreadyExist = "Record already exist";
        public const string ChildRecordExist = "Record can't be deleted, because it's child record(s) exist in another table";
        public const string RecordRelationExist = "Record can't be deleted, because it's relation exist in another table";
        public const string UserUnAuthorized = "Un-Authorized. Username or Password does not match. Contact your administrator";
        public const string CantExceedLimit = "Can't add records more then specified";
        public const string URDeactivated = "You are de-activated. Contact your administrator";
        public const string DataNotProvided = "Data not provided. Please fill all the required fields";
        public const string SelectLeastOneTrue = "Please select at least one true answer";
        public const string SqlDuplicateRecord = "Record already exist in the database";
        public const string ResetPasswordReqSent = "Reset Password email sent at your given email address";
        public const string FileDeleted = "File(s) deleted successfully";
        public const string UserNotLoggedIn = "User not logged in, Please login first.";
        public const string ExpertiesRequired = "Please provide experties for current teacher";
    }
    public class BusinessRules
    {
        public const int Teacher_Min_Age = 20;
    }
}
