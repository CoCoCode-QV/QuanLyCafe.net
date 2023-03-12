using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class Category
    {
        private int _CategoryID;
        private string _name;

        public int CategoryID { get => _CategoryID; set => _CategoryID = value; }
        public string Name { get => _name; set => _name = value; }

        public Category(int id, string name)
        {
            this.CategoryID = id;
            this.Name = name;
        }
        public Category(DataRow row)
        {
            this.CategoryID = (int)row["FoodCategoryID"];
            this.Name = (string)row["name"];
        }

    }

}
