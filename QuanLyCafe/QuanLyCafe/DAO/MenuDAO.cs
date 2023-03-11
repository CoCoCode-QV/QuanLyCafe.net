using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class MenuDAO
    {
        #region Singleton
        private static MenuDAO _ins;

        public static MenuDAO Ins
        {
            get { if (_ins == null) _ins = new MenuDAO(); return _ins; }
            private set { _ins = value; }
        }
        private MenuDAO() { }

        #endregion

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> ListData = new List<Menu>();

            string query = "select f.name, bi.count, f.price, Total = (f.price *bi.count) from Food f, Bill B, Billinfo Bi where f.FoodID = Bi.billID and bi.billID = b.billID  and b.TableID = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query); 

            foreach(DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                ListData.Add(menu);
            }

            return ListData;
        }
    }
}
