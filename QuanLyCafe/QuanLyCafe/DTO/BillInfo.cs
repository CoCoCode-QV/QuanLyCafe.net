using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DTO
{
    class BillInfo
    {
        private int _billInforId;
        private int _billId;
        private int _foodId;
        private int _count;

        public int BillInforId { get => _billInforId; set => _billInforId = value; }
        public int BillId { get => _billId; set => _billId = value; }
        public int FoodId { get => _foodId; set => _foodId = value; }
        public int Count { get => _count; set => _count = value; }

        public BillInfo(int billInforID, int billid, int foodId, int count)
        {
            this.BillInforId = billInforID;
            this.BillId = billid;
            this.FoodId = foodId;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.BillInforId = (int)row["billInfoID"];
            this.BillId = (int)row["billID"];
            this.FoodId = (int)row["foodID"];
            this.Count = (int)row["count"];
        }
    }
}
