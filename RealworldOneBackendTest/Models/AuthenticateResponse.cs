using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealworldOneBackendTest.Models
{
    public class AuthenticateResponse : User
    {
        public string Token { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            ID = user.ID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            Token = token;
        }
    }
}
