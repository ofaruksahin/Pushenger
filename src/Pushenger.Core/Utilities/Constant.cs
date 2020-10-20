namespace Pushenger.Core.Utilities
{
    public static class Constant
    {
        public static string Successful => "Successful";
        public static string Error => "Error";

        public static class CompanyMessages
        {
            public static string CompanyAlreadyExists => "CompanyAlreadyExists";
            public static string UserAlreadyExists => "UserAlreadyExists";
            public static string CompanyNotCreated => "CompanyNotCreated";
            public static string UserNotCreated => "UserNotCreated";
            public static string CompanyNotFound => "CompanyNotFound";
        }      
        
        public static class UserMessages
        {
            public static string UserNotFound => "UserNotFound";
            public static string LoginError => "LoginError";
        }
    }
}
