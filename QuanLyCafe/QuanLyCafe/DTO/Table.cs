using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class Table
    {
        private int _id;
        private string _name;
        private string _status;

        public Table(int id, string name, string status)
        {
            this.Id = id;
            this.Name = name;
            this.Status = status;
        }

        public Table(DataRow row)
        {
            this.Id = (int)row["TableId"];
            this.Name =(string) row["nameTable"];
            this.Status = (string) row["statusTable"];

        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
