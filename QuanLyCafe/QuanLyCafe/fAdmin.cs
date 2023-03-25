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
            loadCategory();
        }

        public void loadListFood()
        {
            dtGridViewFood.Rows.Clear();
            List<Food> listFood = FoodDAO.Ins.GetListFood();
            foreach(Food item in listFood)
            {
                Category category = CategoryDAO.Ins.GetCategoryByID(item.CategoryId);
                dtGridViewFood.Rows.Add(item.Id, item.Name, item.Price,category.Name);
            }
          
        }

        private void loadCategory()
        {
            List<Category> listCategory = CategoryDAO.Ins.GetListCategory();
            cbboxCategoryFood.DataSource = listCategory;

            cbboxCategoryFood.DisplayMember = "Name";
        }

        #endregion


        #region event
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int categoryID = (cbboxCategoryFood.SelectedItem as Category).CategoryID;
            float price = (float) nmPrice.Value;

            if (name ==  ""|| categoryID == null || price == 0)
                return;

            FoodDAO.Ins.InsertFood(name, categoryID, price);
            loadListFood();
        }

        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            
            DataGridViewSelectedRowCollection selectedRows = dtGridViewFood.SelectedRows;
            
            foreach (DataGridViewRow row in selectedRows)
            {
                int id = ((int)row.Cells[0].Value) == null ? -1: (int)row.Cells[0].Value;
                if (id == -1)
                    return;
                if (MessageBox.Show("Bạn có muốn xóa  " + row.Cells[1].Value , "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    FoodDAO.Ins.DeleteFood(id);
            }
            loadListFood();
        }
        #endregion

    }
}
