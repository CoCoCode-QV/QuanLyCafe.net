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
        private void load()
        {
            loadListFood();
            loadCategoryByListFood();
            LoadListCategory();
        }

        private void loadListFood()
        {
            dtGridViewFood.Rows.Clear();
            List<Food> listFood = FoodDAO.Ins.GetListFood();

            foreach (Food item in listFood)
            {
                Category category = CategoryDAO.Ins.GetCategoryByID(item.CategoryId);
                string sellstop = item.SellStop != 1 ? "Còn bán" : "Ngừng bán";
                dtGridViewFood.Rows.Add(item.Id, item.Name, item.Price, category.NameCategory, sellstop);
            }

        }

        private void loadCategoryByListFood()
        {
            List<Category> listCategory = CategoryDAO.Ins.GetListCategory();
            cbboxCategoryFood.DataSource = listCategory;
            cbboxCategoryFood.DisplayMember = "NameCategory";
        }

        private void LoadListCategory()
        {
            dtgridViewCategory.Rows.Clear();
            List<Category> listCategory = CategoryDAO.Ins.GetListCategory();
            foreach (Category item in listCategory)
            {
               
                dtgridViewCategory.Rows.Add(item.CategoryID, item.NameCategory);
            }

        }

        #endregion


        #region event Food
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int categoryID = (cbboxCategoryFood.SelectedItem as Category).CategoryID;
            float price = (float)nmPrice.Value;
            int sellStop = comboBoxSellStop.Text == "Còn bán" ? 0 : 1;
            if (name == "" || categoryID == 0 || price == 0 )
            {
                MessageBox.Show("Bạn cần điền đầy đủ và chính xác! ", "Thông báo");
                return;
            }
            FoodDAO.Ins.InsertFood(name, categoryID, price, sellStop);
            loadListFood();
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {

            int id = int.TryParse(txtIDFood.Text, out int tempId) ? tempId : -1;
            string name = txtNameFood.Text;
            int categoryID = (cbboxCategoryFood.SelectedItem as Category).CategoryID;
            float price = (float)nmPrice.Value;
            int sellStop = comboBoxSellStop.Text == "Còn bán" ? 0 : 1;
            if (name == "" || categoryID == 0 || price == 0)
            {
                MessageBox.Show("Bạn cần điền đầy đủ và chính xác! ", "Thông báo");
                return;
            }


            if (FoodDAO.Ins.UpdateFood(id, name, categoryID, price, sellStop))
            {
                MessageBox.Show("Sửa món thành công" , "Thông báo", MessageBoxButtons.OK);
                loadListFood();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món ăn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
            
        private void btnRemoveFood_Click(object sender, EventArgs e)
        {

            DataGridViewSelectedRowCollection selectedRows = dtGridViewFood.SelectedRows;
            if (selectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in selectedRows)
                {
                    int id = -1;
                    if (row.Cells[0].Value is int)
                    {
                        id = (int)row.Cells[0].Value;
                    }
                    if (id == -1)
                        return;
                    if (MessageBox.Show("Bạn có muốn xóa  " + row.Cells[1].Value, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {

                        if (FoodDAO.Ins.DeleteFood(id))
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo");

                        }
                        else
                        {
                            MessageBox.Show("Sản phẩm hiện đang ở trong một hóa đơn","Thông báo" ,MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                      
                    }    
                }
                loadListFood();
            }
        }


        private void dtGridViewFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Kiểm tra xem người dùng có click vào tiêu đề của bảng không
            {
                DataGridViewRow row = dtGridViewFood.Rows[e.RowIndex];
                if (row.Cells[0].Value == null || row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null ||row.Cells[4] == null)
                    return;
                txtIDFood.Text = row.Cells[0].Value.ToString();
                txtNameFood.Text = row.Cells[1].Value.ToString();
                nmPrice.Value = Convert.ToDecimal(row.Cells[2].Value);
                cbboxCategoryFood.Text = row.Cells[3].Value.ToString();
                comboBoxSellStop.Text = row.Cells[4].Value.ToString();
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            string name = txtSearch.Text;
            List<Food> listFood = FoodDAO.Ins.searchFoodByName(name);
            dtGridViewFood.Rows.Clear();
            foreach (Food item in listFood)
            {
                Category category = CategoryDAO.Ins.GetCategoryByID(item.CategoryId);
                string sellstop = item.SellStop != 1 ? "Còn bán" : "Ngừng bán";
                dtGridViewFood.Rows.Add(item.Id, item.Name, item.Price, category.NameCategory,sellstop );
            }
        }
        #endregion

        #region event Category
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            if (name == null)
                return;
            if (CategoryDAO.Ins.insertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công");
                LoadListCategory();
            }
        }

        private void dtgridViewCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Kiểm tra xem người dùng có click vào tiêu đề của bảng không
            {
                DataGridViewRow row = dtgridViewCategory.Rows[e.RowIndex];
                if (row.Cells[0].Value == null || row.Cells[1].Value == null )
                    return;
                txtCategoryName.Text = row.Cells[1].Value.ToString();
                txtIDCategory.Text = row.Cells[0].Value.ToString();
               
            }
        }


        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dtgridViewCategory.SelectedRows;
            if (selectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in selectedRows)
                {
                    int id = -1;
                    if (row.Cells[0].Value is int)
                    {
                        id = (int)row.Cells[0].Value;
                    }
                    if (id == -1)
                        return;
                    if (MessageBox.Show("Bạn có muốn xóa  " + row.Cells[1].Value, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {

                        if(CategoryDAO.Ins.DeleteCategory(id))
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo");

                        }
                        else
                        {
                            MessageBox.Show("Hiện đang có các món ăn ở trong danh mục, vui lòng xóa các món ăn rồi quay lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }
                }
                LoadListCategory();
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDCategory.Text);
            string name = txtCategoryName.Text;
            if(name == null )
            {
                MessageBox.Show("Bạn cần điền đầy đủ và chính xác! ", "Thông báo");
                return;
            }

            if (CategoryDAO.Ins.UpdateCategory(id,name))
            {
                MessageBox.Show("Cập nhật danh mục thành công", "Thông báo", MessageBoxButtons.OK);
                LoadListCategory();
            }
            else
            {
                MessageBox.Show("Có lỗi khi cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

    }
}
