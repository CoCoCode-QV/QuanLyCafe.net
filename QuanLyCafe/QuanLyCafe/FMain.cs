using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyCafe.DTO.Menu;

namespace QuanLyCafe
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
            LoadTable();
            loadCategory();
        }

        #region Method
        private void LoadTable()
        {
            List<Table> Table =  TableDAO.Ins.loadTableList();

            foreach(Table item  in Table)
            {
                Button btn = new Button() { Width = 100, Height = 100 };
                btn.Text = item.Name;
                btn.Name = item.Id.ToString();
                btn.Tag = item;
                btn.Click += Btn_Click;
                switch(item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.IndianRed;
                        break;
                }    
                flPanelTable.Controls.Add(btn);
            }
        }

        private void showBill(int index)
        {
            listViewBill.Items.Clear();
            List<Menu> ListBillInfo = MenuDAO.Ins.GetListMenuByTable(index);
            float TotalPrice = 0;

            foreach(Menu item in ListBillInfo)
            {
                ListViewItem listViewItem = new ListViewItem(item.FoodName.ToString());

                listViewItem.SubItems.Add(item.Count.ToString());
                listViewItem.SubItems.Add(item.Price.ToString());
                listViewItem.SubItems.Add(item.Total.ToString());
                TotalPrice += item.Total ;
                listViewBill.Items.Add(listViewItem);
            }
            string strPrice = string.Format(new CultureInfo("vi-VN"), "{0:#,##0} VNĐ", TotalPrice);
            txtTotalPrice.Text = strPrice;

        }

        private void loadCategory()
        {
            List<Category> listCategory = CategoryDAO.Ins.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        private void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Ins.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "name";
        }
        #endregion

        #region events
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableId = Convert.ToInt32((sender as Button).Name);
            listViewBill.Tag = (sender as Button).Tag;
            showBill(tableId);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            this.Hide();
            f.ShowDialog();
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
      
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            int id = selected.CategoryID;
            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {

        }

        #endregion


    }
}
