using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicSandBox
{
    public class User : Rush.RushObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthData { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }
    }
}
