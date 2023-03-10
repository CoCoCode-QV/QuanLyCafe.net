﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class Bill
    {
        private int _billId;
        private DateTime? _dateCheckOut;
        private DateTime? _dateCheckIn;
        private int _tableId;
        private int _status;

        public int BillId { get => _billId; set => _billId = value; }
        public DateTime? DateCheckOut { get => _dateCheckOut; set => _dateCheckOut = value; }
        public DateTime? DateCheckIn { get => _dateCheckIn; set => _dateCheckIn= value; }
        public int TableId { get => _tableId; set => _tableId = value; }
        public int Status { get => _status; set => _status = value; }
        
        public Bill(int id, DateTime? dateCheckOut, DateTime? dateCheckin,int status)
        {
            this.BillId = id;
            this.DateCheckOut = dateCheckOut;
            this.DateCheckIn = dateCheckin;
            this.Status = status;
        }
        
        public Bill (DataRow row)
        {
            this.BillId = (int)row["billID"];
            this.DateCheckIn = (DateTime?)row["dateCheckin"];
            var temp = row["dateCheckOut"];
            if(temp.ToString() != "")
                this.DateCheckOut = (DateTime?)temp;

            this.Status = (int)row["statusBill"];
        }
    }
}
