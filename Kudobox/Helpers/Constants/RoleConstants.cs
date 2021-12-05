namespace Kudobox.Helpers.Constants
{
    public static class RoleConstants
    {
        public const string ADMIN = "ADMIN";
        public const string USER = "USER";
        public const string USER_ADM = "USER_ADM";
        public const string USER_VIEW = "USER_VIEW";

        public static bool ValidRole(string role)
        {
            return role is ADMIN or
                USER or
                USER_ADM or
                USER_VIEW;
        }
    }
}