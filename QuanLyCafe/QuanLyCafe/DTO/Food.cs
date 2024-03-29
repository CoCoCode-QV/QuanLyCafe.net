﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class Food
    {
        private int _id;
        private string _name;
        private int _CategoryId;
        private float _price;
        private int _SellStop;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public int CategoryId { get => _CategoryId; set => _CategoryId = value; }
        public float Price { get => _price; set => _price = value; }
        public int SellStop { get => _SellStop; set => _SellStop = value; }

        public Food(int id, string name, int categoryid, float price, int sellstop)
        {
            this.Id = id;
            this.Name = name;
            this.CategoryId = categoryid;
            this.Price = price;
            this.SellStop = sellstop;
        }

        public Food(DataRow row)
        {
            this.Id = (int)row["FoodID"];
            this.Name = (string)row["name"];
            this.CategoryId = (int)row["CategoryID"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.SellStop = (int)row["sellStop"];
        }
    }
}
