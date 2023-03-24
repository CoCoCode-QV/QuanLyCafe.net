using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            string query = "EXEC PR_Login @username , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, password });
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
    }
}
