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
            DataTable data = DataProvider.Instance.ExecuteQuery("Select *from dbo.Bill where statusBill = 0 and TableID = " + id );
            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.BillId;
            }
            return -1;
        }

        public void insertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec PR_InsertBill @TableId ", new object[] { id });
        }

        public int GetMaxIdBill()
        {
             return (int)   DataProvider.Instance.ExecuteScalar("select max(billID) from dbo.Bill");
        }

        public void CheckOut(int id)
        {
            string query = "Update dbo.Bill set statusBill = 1 , dateCheckOut = GETDATE() where billID = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        
    }
}
