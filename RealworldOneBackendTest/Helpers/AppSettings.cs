using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealworldOneBackendTest.Helpers
{
    public class AppSettings
    {
        public string AppSecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
