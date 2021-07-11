using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtorsProcessing.Api.Configuration
{
    public class JwtConfig
    {
        public string LoginSecret { get; set; }
        public string SessionSecret { get; set; }
    }
}
