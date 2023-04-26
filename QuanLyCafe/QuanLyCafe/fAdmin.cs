using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyCafe.DTO.Menu;

namespace QuanLyCafe
{
    public partial class fAdmin : Form
    {
        public fAdmin()
        {
            InitializeComponent();
            LoadRevenue();
        }

        #region methods

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

        private void LoadRevenue()
        {
            dataGridStatis.Rows.Clear();
            DateTime dateFrom = dtpkFromDate.Value;
            DateTime dateTo = dtpkToDate.Value;
            List<Bill> listCategory = BillDAO.Ins.LoadBIll(dateFrom, dateTo);
            float TotalRevenue = 0;
            foreach (Bill item in listCategory)
            {
                Table table = TableDAO.Ins.getTableByIDTable(item.TableId);
                List<Menu> menus = MenuDAO.Ins.GetMenuByBill(item.BillId);
                float PriceTotal = 0;
                foreach (Menu temp in menus)
                {
                    PriceTotal += temp.Total;
                }
                float TotalReduce = PriceTotal - (PriceTotal * item.Discount / 100);
                TotalRevenue += TotalReduce;
                string formattedTotal = $"{TotalReduce:N0} VNĐ";
                dataGridStatis.Rows.Add(item.BillId, table.Name, item.DateCheckIn, item.DateCheckOut, formattedTotal);
            }
            string formattedTotalRevenue = $"{TotalRevenue:N0} VNĐ";
            txtTotalRevenue.Text = formattedTotalRevenue;
        }

        private void LoadStatisticals(List<Menu> listFoodTopselling)
        {
            chartTopFood.Series["Số lượng"].Points.Clear();
            chartTopFood.ChartAreas["ChartArea1"].AxisX.Title = "Tên sản phẩm";
            chartTopFood.ChartAreas["ChartArea1"].AxisY.Title = "Số lượng sản phẩm đã bán ra";

            foreach (Menu item in listFoodTopselling)
            {
                chartTopFood.Series["Số lượng"].Points.AddXY(item.FoodName, item.Count);
            }
        }

        private void LoadStatisticMonth()
        {
            List<Menu> listFoodTopselling = new List<Menu>();
            listFoodTopselling = MenuDAO.Ins.GetToSellingOfTheMonth();
            LoadStatisticals(listFoodTopselling);
        }

        private void LoadStatisticWeek()
        {
            List<Menu> listFoodTopselling = new List<Menu>();
            listFoodTopselling = MenuDAO.Ins.GetToSellingOfTheWeek();
            LoadStatisticals(listFoodTopselling);
        }

        private void LoadStatisticDay()
        {
            List<Menu> listFoodTopselling = new List<Menu>();
            listFoodTopselling = MenuDAO.Ins.GetToSellingOfTheDay();
            LoadStatisticals(listFoodTopselling);
        }





        #endregion

        #region event tab
        private void tab_SelectedIndexChanged(object sender, EventArgs e)
        {

            int selectedIndex = tab.SelectedIndex;


            switch (selectedIndex)
            {
                case 0:
                    LoadRevenue();
                    break;
                case 1:
                    loadListFood();
                    loadCategoryByListFood();
                    break;
                case 2:
                    LoadListCategory();
                    break;
                case 3:
                    loadTable();
                    break;
                case 4:
                    LoadAccount();
                    break;
                default:
                    LoadStatisticMonth();
                    break;
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
            if (name == "" || categoryID == 0 || price == 0)
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
                MessageBox.Show("Sửa món thành công", "Thông báo", MessageBoxButtons.OK);
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
                            MessageBox.Show("Sản phẩm hiện đang ở trong một hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (row.Cells[0].Value == null || row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null || row.Cells[4] == null)
                    return;
                txtIDFood.Text = row.Cells[0].Value.ToString();
                txtNameFood.Text = row.Cells[1].Value.ToString();
                nmPrice.Value = Convert.ToDecimal(row.Cells[2].Value);
                cbboxCategoryFood.Text = row.Cells[3].Value.ToString();
                comboBoxSellStop.Text = row.Cells[4].Value.ToString();
            }
        }

        private Stopwatch _stopwatch;
        private const int _DWELL_TIME_THRESHOLD = 300;
        private Timer _searchTimer;

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { 
            if (_stopwatch == null)
            {
                _stopwatch = Stopwatch.StartNew();
            }
            else
            {
                _stopwatch.Restart();
            }

            if (_searchTimer == null)
            {
                _searchTimer = new Timer();
                _searchTimer.Interval = _DWELL_TIME_THRESHOLD;
                _searchTimer.Tick += new EventHandler(btnSearchFood_Click);
            }
            _searchTimer.Stop();
            _searchTimer.Start();
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
                dtGridViewFood.Rows.Add(item.Id, item.Name, item.Price, category.NameCategory, sellstop);
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
                if (row.Cells[0].Value == null || row.Cells[1].Value == null)
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

                        if (CategoryDAO.Ins.DeleteCategory(id))
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo");
                            LoadListCategory();
                        }
                        else
                        {
                            MessageBox.Show("Hiện đang có các món ăn ở trong danh mục, vui lòng xóa các món ăn rồi quay lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }
                }

            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtIDCategory.Text);
            string name = txtCategoryName.Text;
            if (name == null)
            {
                MessageBox.Show("Bạn cần điền đầy đủ và chính xác! ", "Thông báo");
                return;
            }

            if (CategoryDAO.Ins.UpdateCategory(id, name))
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

        #region event Table
        private void loadTable()
        {
            dataGridViewTable.Rows.Clear();
            List<Table> listtable = TableDAO.Ins.loadTableList();

            foreach (Table item in listtable)
            {
                dataGridViewTable.Rows.Add(item.Id, item.Name, item.Status);
            }


        }

        private void dataGridViewTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Kiểm tra xem người dùng có click vào tiêu đề của bảng không
            {
                DataGridViewRow row = dataGridViewTable.Rows[e.RowIndex];
                if (row.Cells[0].Value == null || row.Cells[1].Value == null || row.Cells[2].Value == null)
                    return;
                txtTableID.Text = row.Cells[0].Value.ToString();
                txtNameTable.Text = row.Cells[1].Value.ToString();
                txtStatusTable.Text = row.Cells[2].Value.ToString();

            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = "Bàn " + txtNameTable.Text;
            if (name == null)
                return;
            if (TableDAO.Ins.insertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo", MessageBoxButtons.OK);
                loadTable();
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveTable_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridViewTable.SelectedRows;
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

                        if (TableDAO.Ins.DeleteTable(id))
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo");
                            loadTable();
                        }
                        else
                        {
                            MessageBox.Show("Bị lỗi khi xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }
                }
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtTableID.Text);
            string name = txtNameTable.Text;
            if (name == null)
            {
                MessageBox.Show("Bạn cần điền đầy đủ và chính xác! ", "Thông báo");
                return;
            }

            if (TableDAO.Ins.UpdateTable(id, name))
            {
                MessageBox.Show("Cập nhật bàn thành công", "Thông báo", MessageBoxButtons.OK);
                loadTable();
            }
            else
            {
                MessageBox.Show("Có lỗi khi cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region event Account
        private void LoadAccount()
        {
            dataGridViewAccount.Rows.Clear();
            List<Account> listAccount = AccountDAO.Ins.LoadAccount();

            foreach (Account item in listAccount)
            {
                string password = AccountDAO.Ins.HashPassword(item.PassWord);
                string type = item.Type == 1 ? "Quản lý" : "Nhân viên";
                dataGridViewAccount.Rows.Add(item.AccountID, item.DisplayName, item.UserName, password, type);
            }
        }

        private void dataGridViewAccount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) // Kiểm tra xem người dùng có click vào tiêu đề của bảng không
            {
                DataGridViewRow row = dataGridViewAccount.Rows[e.RowIndex];
                if (row.Cells[0].Value == null || row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null || row.Cells[4].Value == null)
                    return;
                txtIDAccount.Text = row.Cells[0].Value.ToString();
                txtDisplay.Text = row.Cells[1].Value.ToString();
                txtUser.Text = row.Cells[2].Value.ToString();
                txtPassword.Text = row.Cells[3].Value.ToString();
                cbTypeUser.Text = row.Cells[4].Value.ToString();

            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string displayname = txtDisplay.Text;
            string userName = txtUser.Text;
            string passwords = AccountDAO.Ins.HashPassword(txtPassword.Text);
            int type = cbTypeUser.Text == "Quản lý" ? 1 : 0;

            if (displayname == null || userName == null || passwords == null || type == null)
            {
                MessageBox.Show("Cần nhập đầy đủ các trường thông tin", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (AccountDAO.Ins.insertAccount(displayname, userName, passwords, type))
            {
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection selectedRows = dataGridViewAccount.SelectedRows;
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
                    if (id == 1)
                    {
                        MessageBox.Show("Tài khoản này không được xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (MessageBox.Show("Bạn có muốn xóa  " + row.Cells[1].Value, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {

                        if (AccountDAO.Ins.DeleteAccount(id))
                        {
                            MessageBox.Show("Xóa thành công!", "Thông báo");
                            LoadAccount();
                        }
                        else
                        {
                            MessageBox.Show("Bị lỗi khi xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }


                    }
                }
            }
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            string displayname = txtDisplay.Text;
            string userName = txtUser.Text;
            string passwords = AccountDAO.Ins.HashPassword(txtPassword.Text);
            int type = cbTypeUser.Text == "Quản lý" ? 1 : 0;
            int idAccount = Convert.ToInt32(txtIDAccount.Text);

            if (displayname == null || userName == null || passwords == null || type == null || idAccount == null)
            {
                MessageBox.Show("Cần nhập đầy đủ các trường thông tin", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            if (AccountDAO.Ins.UpdateAccount(displayname, userName, passwords, type, idAccount))
            {
                MessageBox.Show("Cập nhật tài khoản thành công", "Thông báo", MessageBoxButtons.OK);
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        #endregion

        #region event Revenue
        private void btnStatis_Click(object sender, EventArgs e)
        {
            LoadRevenue();
        }



        #endregion

        #region event Statistic
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;
            int selectedIndex = comboBox1.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    LoadStatisticMonth();
                    break;
                case 1:
                    LoadStatisticDay();
                    break;
                default:
                    LoadStatisticWeek();
                    break;
            }


        }
        #endregion
    }
}
