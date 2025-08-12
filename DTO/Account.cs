using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Account
    {
        private string username;
        private string password;
        private string displayname;
        private string accountType;

        public Account(string username, string displayname, string accountType, string password = null)
        {
            this.username = username;
            this.password = password;
            this.displayname = displayname;
            this.accountType = accountType;
        }

        public Account(DataRow row)
        {
            username = row["Username"].ToString();
            password = row["Password"].ToString();
            displayname = row["DisplayName"].ToString();
            accountType = row["AccountType"].ToString();
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Displayname { get => displayname; set => displayname = value; }
        public string AccountType { get => accountType; set => accountType = value; }

    }
}
