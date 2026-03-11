namespace PartsManager.Client
{
    public static class UserSession
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static int UserLevel { get; set; }

        public static void Clear()
        {
            UserID = 0;
            Username = null;
            UserLevel = 0;
        }

        public static bool IsAdmin => UserLevel == 1;
        public static bool IsManager => UserLevel <= 2;
        public static bool CanInbound => UserLevel <= 3;
    }
}
