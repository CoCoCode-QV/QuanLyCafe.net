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

        public void CheckOut(int id, int discount)
        {
            string query = "Update dbo.Bill set statusBill = 1 , dateCheckOut = GETDATE() , Discount = " + discount + " where billID = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public List<Bill> LoadBIll(DateTime fromdate, DateTime todate)
        {
            List<Bill> listBill = new List<Bill>();
            DataTable data =  DataProvider.Instance.ExecuteQuery("SELECT * FROM Bill WHERE  CAST(dateCheckOut AS DATE ) >= CAST( @fromdate AS DATE) and   CAST(dateCheckOut AS DATE) <=  CAST( @todate  AS DATE) and statusBill = 1", new object[] { fromdate, todate});
           foreach(DataRow item in data.Rows)
            {
                Bill bill = new Bill(item);
                listBill.Add(bill);
            }

            return listBill;
        }

        public int CountBillOfPeople()
        {
            return (int) DataProvider.Instance.ExecuteScalar("select count(*) from Bill where  CAST(GETDATE() AS DATE) = CAST( dateCheckin AS DATE)");
        }

        public int CountBillOfPeopleFinish()
        {
            return (int)DataProvider.Instance.ExecuteScalar("select count(*) from Bill where  CAST(GETDATE() AS DATE) = CAST( dateCheckin AS DATE) and statusBill = 1");
        }

        public int CountBillOfPeopleServing()
        {
            return (int)DataProvider.Instance.ExecuteScalar("select count(*) from Bill where  CAST(GETDATE() AS DATE) = CAST( dateCheckin AS DATE) and statusBill = 0");
        }

        public List<Bill> GetBill(string query)
        {
            List<Bill> listBill = new List<Bill>();
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Bill bill = new Bill(item);
                listBill.Add(bill);
            }
            return listBill;
        }

        public List<Bill> GetBillListOfDay()
        {
            string query = "select * from Bill where CAST(GETDATE() AS DATE) = CAST( dateCheckin AS DATE) ";
            return GetBill(query);
        }

        public List<Bill> GetBillListOfDayPaid()
        {
            string query = "select * from  Bill  where  CAST(GETDATE() AS DATE) = CAST( dateCheckOut AS DATE) and statusBill = 1";
            return GetBill(query);
        }

        public List<Bill> GetMenuListOfDayServing()
        {
            string query = "select * from Bill where statusBill = 0 and CAST(GETDATE() AS DATE) = CAST( dateCheckin AS DATE) ";
            return GetBill(query);
        }
    }
}
