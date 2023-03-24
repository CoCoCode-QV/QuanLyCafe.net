using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class Account
    {
        private int _accountID;
        private string _displayName;
        private string _userName;
        private string _passWord;
        private int _type;

        public int AccountID { get => _accountID; set => _accountID = value; }
        public string DisplayName { get => _displayName; set => _displayName = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string PassWord { get => _passWord; set => _passWord = value; }
        public int Type { get => _type; set => _type = value; }

        public Account(int accountId, string displayname, string username, int type, string password = null)
        {
            this.AccountID = accountId;
            this.DisplayName = displayname;
            this.UserName = username;
            this.PassWord = password;
            this.Type = type;
        }

        public Account(DataRow row)
        {
            this.AccountID = (int)row["AccountID"];
            this.DisplayName = (string)row["displayName"];
            this.UserName = (string)row["userName"];
            this.PassWord = (string)row["password"];
            this.Type = (int)row["Type"];
        }
    }
}
