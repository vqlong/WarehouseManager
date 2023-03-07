using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User : Model
    { 
        public string Username { get; set; }
        public Role Role { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
    }
}
