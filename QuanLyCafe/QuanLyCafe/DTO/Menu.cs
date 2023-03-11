using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class Menu
    {
        private string _foodName;
        private int _count;
        private float _price;
        private float _total;

        public string FoodName { get => _foodName; set => _foodName = value; }
        public int Count { get => _count; set => _count = value; }
        public float Price { get => _price; set => _price = value; }
        public  float Total { get => _total; set => _total = value; }

        public Menu(string foodname, int count, float price, float total)
        {
            this.FoodName = foodname;
            this.Count = count;
            this.Price = price;
            this.Total = total;
        }
        public Menu(DataRow row)
        {
            this.FoodName = (string)row["name"];
            this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.Total = (float)Convert.ToDouble(row["Total"].ToString());
        }
    }
}
