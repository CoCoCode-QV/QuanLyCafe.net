using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class FoodDAO
    {
        #region Singleton
        private static FoodDAO _ins;

        public static FoodDAO Ins
        {
            get { if (_ins == null) _ins = new FoodDAO(); return _ins; }
            private set { _ins = value; }
        }
        private FoodDAO() { }

        #endregion

        public List<Food> GetFoodByCategoryID(int ID)
        {
            List<Food> List = new List<Food>();
            string query = "select * from Food where CategoryID = " + ID;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Food f = new Food(item);
                List.Add(f);
            }
            return List;
        }
        public List<Food> GetListFood()
        {
            List<Food> List = new List<Food>();
            string query = "select * from Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food f = new Food(item);
                List.Add(f);
            }
            return List;
        }
    }
}
