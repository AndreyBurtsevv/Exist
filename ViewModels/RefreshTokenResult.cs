using System;

namespace Exist.ViewModels
{
    public class RefreshTokenResult
    {
        public string RefreshToken { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
