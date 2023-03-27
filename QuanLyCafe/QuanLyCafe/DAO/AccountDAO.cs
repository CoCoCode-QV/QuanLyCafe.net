using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe.DAO
{
     class AccountDAO
    {

        #region Singleton
        private static AccountDAO _ins;

        public static AccountDAO Ins {

            get { if (_ins == null) _ins = new AccountDAO(); return _ins; }
            private set { _ins = value; }
        }
        private AccountDAO() {}

        #endregion

        public bool isLogin(string username, string password)
        {

            string hashPass = HashPassword(password);
            string query = "EXEC PR_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, hashPass });
            return result.Rows.Count >0;
        }
        
        public Account GetAccountByUserName(string userName)
        {
            DataTable table = DataProvider.Instance.ExecuteQuery("Select * from Account where userName = @userName", new object[] { userName });
            foreach(DataRow item in table.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public List<Account> LoadAccount()
        {
            List<Account> ListAccount = new List<Account>();

            string query = "select * from Account";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            
            foreach(DataRow item in data.Rows)
            {
                Account acc = new Account(item);
                ListAccount.Add(acc);
            }
            return ListAccount;
        }

        public string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = SHA256.Create().ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public bool insertAccount(string displayname, string username, string password, int type)
        {
            string query = "insert into Account(displayName,userName,password,Type) VALUES( @displayname , @username , @password , @type )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { displayname, username, password, type });
            return result > 0;
        }

        public bool UpdateAccount(string displayname, string username, string password, int type, int accountid)
        {

            string query = "update Account set displayName = @displayName , userName = @userName , password = @password , Type = @type where AccountID = @accountid ";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { displayname , username , password , type, accountid });
            return result > 0;
        }

        public bool DeleteAccount(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("delete Account WHERE AccountID = " + id);
            return result > 0;
        }
      
    }
}
