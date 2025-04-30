using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileShopee.Model
{
    internal class UserModel
    {
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Address { get; set; }
        public String MobileNumber { get; set; }
        public String Hint { get; set; }
        public String Role { get; set; }
    }
}
