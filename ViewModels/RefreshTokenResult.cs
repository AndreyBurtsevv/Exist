using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.ViewModels
{
    public class RefreshTokenResult
    {
        public string RefreshToken { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
