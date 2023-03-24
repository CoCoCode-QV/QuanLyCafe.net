using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
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
            load();
        }

        #region methods
        public void load()
        {
            loadListFood();
        }

        public void loadListFood()
        {
            List<Food> listFood = FoodDAO.Ins.GetListFood();
            foreach(Food item in listFood)
            {
                dtGridViewFood.Rows.Add(item.Id, item.Name, item.Price, item.CategoryId);
            }
          
        }
        #endregion

        private void dtGridViewFood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
