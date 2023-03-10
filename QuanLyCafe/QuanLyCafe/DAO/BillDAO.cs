using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class BillDAO
    {
        #region Singleton
        private static BillDAO _ins;

        public static BillDAO Ins
        {

            get { if (_ins == null) _ins = new BillDAO(); return _ins; }
            private set { _ins = value; }
        }
        private BillDAO() { }

        #endregion

        public int getUnchecBillIdbyTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select *from dbo.Bill where TableID =" + id + "");
            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.BillId;
            }
            return -1;
        }
    }
}
