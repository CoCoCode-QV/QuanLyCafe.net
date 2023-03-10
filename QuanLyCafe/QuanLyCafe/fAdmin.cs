using QuanLyCafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe
{
    public partial class fAdmin : Form
    {
        public fAdmin()
        {
            InitializeComponent();
          
        }
        void LoadAccountList()
        {
            string query = "Select * from dbo.Account";

            dataGridViewAccount.DataSource = DataProvider.Instance.ExecuteQuery(query);

        }
    }
}
