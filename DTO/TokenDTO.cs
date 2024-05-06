namespace DTO
{
    public class JwtManagement
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryAccessMinutes { get; set; }
        public int ExpiryRefreshMinutes { get; set; }
    }

    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
