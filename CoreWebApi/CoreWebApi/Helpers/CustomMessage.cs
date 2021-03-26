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
        public const string NewPasswordNotGiven = "New password not provided";
        public const string UnableToAdd = "Unable to add data";
        public const string SomeErrorOccured = "Some error occured";
        public const string UserAlreadyExist = "User already exist";
        public const string RecordAlreadyExist = "Record already exist";
        public const string ChildRecordExist = "Record contains child record(s) in another table. Please remove them first";
        public const string RecordRelationExist = "{0} record contains association in another table. Please remove all associations";
        public const string UserUnAuthorized = "Un-Authorized. Username or Password does not match. Contact your administrator";
        public const string CantExceedLimit = "Student assigned limit exceed from allocated student of numbers {0}";
        public const string URDeactivated = "You are de-activated. Contact your administrator";
        public const string DataNotProvided = "Data not provided. Please fill all the required fields";
        public const string SelectLeastOneTrue = "Please select at least one true answer";
        public const string SqlDuplicateRecord = "Duplicate record found";
        public const string ResetPasswordReqSent = "Reset Password email sent at your given email address";
        public const string FileDeleted = "File(s) deleted successfully";
        public const string UserNotLoggedIn = "User not logged in, Please login first.";
        public const string ExpertiesRequired = "Please provide experties for this teacher";
        public const string LevelFromToCheck = "Level from should be less than level to";
        public const string TeacherMinAge = "Teacher age cannot be less then {0} years";
        public const string StudentMinAge = "Student age cannot be less then {0} years";
        public const string CantChangeUserType = "Can't change user-type of {0} record, because it has reference in other system operation(s)";
        public const string ActiveStatusUpdated = "Active status updated successfully";
        public const string CantDeleteEvent = "Event mapped in Calendar could not be able to remove";
        public const string TeacherLectureDuplicate = "Duplicate teacher in same time slot in another class-section found";
        public const string ExpertiesHasRelation = "These experties {0} have associations in time-table. Please remove all assiciations first";
        public const string SubjectNotProvided = "Please fill the empty subject fields";
        public const string NoOfQuestionIsLowerNow = "Please remove extra questions first then set new number of questions";
        public const string NoOfStudentLimitIsLowerNow = "Please un-map some students first then set new limit";
        public const string EmailSameOfParentChild = "Please enter different email address for student and parent";
        public const string FeeAlreadyPaid = "Fee already paid of this student";
        public const string FeeVouchersGenerated = "Fee vouchers of {0} generated successfully";
    }
    public class BusinessRules
    {
        public const int Teacher_Min_Age = 20;
        public const int Student_Min_Age = 8;
    }

}
