namespace ProjectTisa.Controllers.GeneralData.Consts
{
    /// <summary>
    /// Constants using in validation.
    /// </summary>
    public static class ValidationConst
    {
        public const int MAX_FILE_SIZE = 4194304;//4MB
        public const int MAX_PAGE_SIZE = 50;
        public const int MAX_STR_LENGTH = 32;
        public const int MAX_USERNAME_LENGTH = 20;
        public const int MAX_DOMAIN_LENGTH = 256;
        public const int MIN_STR_LENGTH = 3;
        public const string REGEX_NUM_SYMBS = @"^[a-zA-Z0-9]*$";
        public const string REGEX_NUM_SYMBS_SPACE = @"^[a-zA-Z0-9 ]*$";
        public const string REGEX_EMAIL = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        public const string REGEX_PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
        public const string NO_REGEX = null;
    }
}
