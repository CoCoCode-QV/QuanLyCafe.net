using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class TableDTO
    {
        private int _id;
        private string _name;
        private string _status;

        public TableDTO(int id, string name, string status)
        {
            this.Id = id;
            this.Name = name;
            this.Status = status;
        }

        public TableDTO(DataRow row)
        {
            this.Id = (int)row["TableId"];
            this.Name =(string) row["nameFood"];
            this.Status = (string)row["statusFood"];

        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Status { get => _status; set => _status = value; }
    }
}
