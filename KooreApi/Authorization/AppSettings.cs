namespace KooreApi.Authorization
{
    public class AppSettings
    {
        public string ACCESS_TOKEN_SECRET { get; set; }
        public string REFRESH_TOKEN_SECRET { get; set; }

        public string PASS_RESET_URL { get; set; }
        public string EMAIL_SENDER { get; set; }
        public string REDIS_URL { get; set; }
        public int REDIS_PORT { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }
        // access token time to live (in minutes), inactive tokens are
        public int AccessTokenTTL { get; set; }
    }
}