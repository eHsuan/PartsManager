namespace PartsManager.Client
{
    public static class UserSession
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static int UserLevel { get; set; } // 1:Admin, 2:Manager, 3:Staff, 4:Guest, 5:Remote Restricted

        public static void Clear()
        {
            UserID = 0;
            Username = null;
            UserLevel = 0;
        }

        public static bool IsAdmin => UserLevel == 1;
        public static bool IsManager => UserLevel <= 2;
        public static bool CanInbound => UserLevel >= 1 && UserLevel <= 3; // Level 5 cannot inbound
    }
}
