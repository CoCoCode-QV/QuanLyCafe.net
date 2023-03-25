using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class CategoryDAO
    {

        #region Singleton
        private static CategoryDAO _ins;

        public static CategoryDAO Ins
        {
            get { if (_ins == null) _ins = new CategoryDAO(); return _ins; }
            private set { _ins = value; }
        }
        private CategoryDAO() { }

        #endregion

        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "select * from FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }
            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;
            string query = "select * from dbo.FoodCategory where FoodCategoryID = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                 category = new Category(item);
                
            }

            return category;
        }

    }
}
