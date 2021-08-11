namespace Exist.ViewModels
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }

        public RefreshTokenResult RefreshToken { get; set; }
    }
}
