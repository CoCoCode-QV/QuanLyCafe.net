﻿using QuanLyCafe.DTO;
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

        public List<TableDTO> loadTableList()
        {

            List<TableDTO> tableList = new List<TableDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("exec PR_LoadTable");

            foreach(DataRow item in data.Rows)
            {
                TableDTO table = new TableDTO(item);
                tableList.Add(table);

            }

            return tableList;
        }
    }
}