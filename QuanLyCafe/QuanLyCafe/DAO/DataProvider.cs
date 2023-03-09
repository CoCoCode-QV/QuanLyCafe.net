using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.DAO
{
    class DataProvider
    {
        #region Singleton
        private static DataProvider _instance;
        internal static DataProvider Instance
        {
            get {
                if (_instance == null) 
                    _instance = new DataProvider();

                return DataProvider._instance;
            }
            private set { DataProvider._instance = value; }
        }
        private DataProvider() {}
        #endregion

        private string _ConnectionString = @"Data Source=.\;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";

        public DataTable ExecuteQuery(string Query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
                if(parameter != null)
                {
                    string[] ListPara = Query.Split(' ');
                    int i = 0;
                    foreach(string item in ListPara)
                    {
                        if(item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }    
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        public int ExecuteNonQuery(string Query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
                if (parameter != null)
                {
                    string[] ListPara = Query.Split(' ');
                    int i = 0;
                    foreach (string item in ListPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteNonQuery();
                connection.Close();
            }
            return data;
        }

        public object ExecuteScalar(string Query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
                if (parameter != null)
                {
                    string[] ListPara = Query.Split(' ');
                    int i = 0;
                    foreach (string item in ListPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = command.ExecuteScalar();
                connection.Close();
            }
            return data;
        }   
    }
}
