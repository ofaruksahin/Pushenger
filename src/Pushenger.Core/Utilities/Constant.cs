namespace Pushenger.Core.Utilities
{
    public static class Constant
    {
        public static string Successful => "Successful";
        public static string Error => "Error";
        public static string TokenNotFound => "TokenNotFound";
        public static string LogOuted => "LogOuted";
        public static string UnAuthorized => "UnAuthorized";

        public static class CompanyMessages
        {
            public static string CompanyAlreadyExists => "CompanyAlreadyExists";
            public static string UserAlreadyExists => "UserAlreadyExists";
            public static string CompanyNotCreated => "CompanyNotCreated";
            public static string UserNotCreated => "UserNotCreated";
            public static string CompanyNotFound => "CompanyNotFound";
            public static string CompanyNotUpdated => "CompanyNotUpdated";
        }      
        
        public static class UserMessages
        {
            public static string UserNotFound => "UserNotFound";
            public static string LoginError => "LoginError";
            public static string UpdateError => "UpdateError";
            public static string UserNotCreated => "UserNotCreated";
            public static string UserNotDeleted => "UserNotDeleted";
            public static string UserNotDeletedYourself => "UserNotDeletedYourself";
        }
    }
}
