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
        
        //Phương thức chung
        public List<Menu> GetMenu(string query)
        {
            List<Menu> listMenu = new List<Menu>();
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }
            return listMenu;
        }

        public List<Menu> GetListMenuByTable(int id)
        {
            string query = "select f.name, bi.count, f.price, Total = (f.price *bi.count) from Food f, Bill B, Billinfo Bi where f.FoodID = Bi.foodID and bi.billID = b.billID and b.statusBill = 0 and b.TableID = " + id;
            return GetMenu(query);
        }

        public List<Menu> GetMenuByBill(int id)
        {
            string query = "select f.name, bi.count, f.price, Total = (f.price *bi.count) from Food f, Billinfo Bi where f.FoodID = Bi.foodID and billID = " + id;
            return GetMenu(query);
        }

        public List<Menu> GetTopSellingFoods()
        {
            string query = " EXEC GetTopSellingFoods ";
            return GetMenu(query);
        }

        public List<Menu> GetToSellingOfTheMonth()
        {
            string query = "exec GetTopSellingOfTheMonth";
            return GetMenu(query);
        }

        public List<Menu> GetToSellingOfTheWeek()
        {
            string query = "exec GetTopSellingOfTheWeek";
            return GetMenu(query);
        }
        public List<Menu> GetToSellingOfTheDay()
        {
            string query = "exec GetTopSellingOfTheDay";
            return GetMenu(query);
        }


    }
}
