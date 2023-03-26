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
            string query = "select * from Food where sellStop = 0 and CategoryID = " + ID;
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

        public void InsertFood(string name , int category, float price, int sellStop)
        {
            DataProvider.Instance.ExecuteQuery("exec PR_InsertFood @name , @category , @price , @sellStop", new object[] { name, category, price, sellStop });
        }

        public bool DeleteFood(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec PR_DeleteFood @FOODID", new object[] { id });
            return result > 0;
        }

        public bool UpdateFood(int idFood,string name, int category, float price, int sellStop)
        {
           int result = DataProvider.Instance.ExecuteNonQuery("update dbo.Food set name = @name , CategoryID = @category , price = @price , sellStop = @sellStop where FoodID = @idFood", new object[] { name, category, price, sellStop , idFood });
            return result > 0;
        }

        public List<Food> searchFoodByName(string name)
        {
            List<Food> List = new List<Food>();
            string query = string.Format(" select * from Food where name like N'%{0}%' ", name);
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
