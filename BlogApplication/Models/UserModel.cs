using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogApplication.Models
{
    public class UserModel
    {
        public int id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public int usertype { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public string Tokendata { get; set; }
    }
}