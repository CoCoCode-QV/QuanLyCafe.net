using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class TableDAO
    {
        #region Singleton
        private static TableDAO _ins;

        public static TableDAO Ins
        {

            get { if (_ins == null) _ins = new TableDAO(); return _ins; }
            private set { _ins = value; }
        }
        private TableDAO() { }

        #endregion

        public List<Table> loadTableList()
        {

            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("exec PR_LoadTable");

            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);

            }

            return tableList;
        }

        public void SwitchTablebyIDBill(int idTableOld, int idTableNew, int idBillOld, int idBillNew )
        {
            DataProvider.Instance.ExecuteQuery("exec PR_SwitchTable @IdTableOld , @IdTableNew , @IdBillOld , @IdBillNew", new object[] { idTableOld, idTableNew , idBillOld , idBillNew });
        }

        public bool insertTable(string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("insert into TableFood(nameTable) values ( @name ) ", new object[] { name });
            return result > 0;
        }

        public bool DeleteTable(int id)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("Delete TableFood where TableID = @id ", new object[] { id });
            return result > 0;
        }

        public bool UpdateTable(int id, string name)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("update TableFood set nameTable = @name  where TableID = @id ", new object[] {name, id });
            return result > 0;
        }

        public Table getTableByIDTable(int id)
        {

            Table table = null;
            string query = "select * from TableFood where TableID =" + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                table = new Table(item);

            }

            return table;
        }
    }
}
