using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Models
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public int TokenExp { get; set; }
        public int RefreshTokenExp { get; set; }
    }
}
