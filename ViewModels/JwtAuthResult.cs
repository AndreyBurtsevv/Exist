using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.ViewModels
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }

        public RefreshTokenResult RefreshToken { get; set; }
    }
}
