using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class BillInfoDAO
    {
        #region Singleton
        private static BillInfoDAO _ins;

        public static BillInfoDAO Ins
        {

            get { if (_ins == null) _ins = new BillInfoDAO(); return _ins; }
            private set { _ins = value; }
        }
        private BillInfoDAO() { }

        #endregion

        public List<BillInfo> GetListBillInfo(int Billid)
        {
            List<BillInfo> ListInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select *from Billinfo where billID = " + Billid);

            foreach(DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                ListInfo.Add(info);
            }

            return ListInfo;
        }

        public void insetBillInfo(int idBill, int idFood, int Count)
        {
            DataProvider.Instance.ExecuteNonQuery("exec PR_InsertBillInfo @idBill, @idFood, @count ", new object[] { idBill,idFood,Count });
        }
    }
}
