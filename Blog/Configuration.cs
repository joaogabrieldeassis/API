
namespace Blog
{
    public static class Configuration
    {
        public static string JwtKey  = "AZBFGh8iPoKwXmYV46fUsJ9r";
        public static string ApiKeyName = "api_Key";
        public static string ApiKey = "aOGJKP[IJFE354==";
        public static SmtpConfiguration Smtp = new();
        public class SmtpConfiguration
        {
            public string Host { get; set; }
            public int Port { get; set; } = 25;
            public string UserName { get; set; }
            public string Password { get; set; }
        }

    }


}


