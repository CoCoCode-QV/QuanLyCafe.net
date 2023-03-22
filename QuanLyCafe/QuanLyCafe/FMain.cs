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
            loadComboBoxSwitchTable(cbSwitchTable);
        }

        #region Method
        private void LoadTable()
        {
            flPanelTable.Controls.Clear();
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

        private void loadComboBoxSwitchTable(ComboBox cb)
        {
            List<Table> table = TableDAO.Ins.loadTableList();
            cb.DataSource = table;
            cb.DisplayMember = "Name";
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
        
            Table table = listViewBill.Tag as Table;
            if (table == null)
                return;
            int idBill = BillDAO.Ins.getUnchecBillIdbyTableID(table.Id);
            int foodId = (cbFood.SelectedItem as Food).Id;
            int count = (int)nmFoodAccount.Value;

            if(idBill == -1)
            {
                if(count > 0)
                {

                    BillDAO.Ins.insertBill(table.Id);
                    BillInfoDAO.Ins.insetBillInfo(BillDAO.Ins.GetMaxIdBill(), foodId, count);
                }
            }
            else
            {
                BillInfoDAO.Ins.insetBillInfo(idBill, foodId, count);
            }
            showBill(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = listViewBill.Tag as Table;
            if (table == null)
                return;
            int idBill = BillDAO.Ins.getUnchecBillIdbyTableID(table.Id);
            if(idBill != -1)
            {
                if(MessageBox.Show("Bạn có muốn thanh toán hóa đơn " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Ins.CheckOut(idBill);
                    showBill(table.Id);
                    LoadTable();
                }
            }
            
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {

            Table tableOld = listViewBill.Tag as Table;
            Table tableNew = cbSwitchTable.SelectedItem as Table;
            int idBillNew = BillDAO.Ins.getUnchecBillIdbyTableID(tableNew.Id);
            int idBillOld = BillDAO.Ins.getUnchecBillIdbyTableID(tableOld.Id);

            if (tableOld == null || tableNew == null || tableOld.Id == tableNew.Id)
                return;

            if (idBillNew == -1)
            {
                BillDAO.Ins.insertBill(tableNew.Id);
                idBillNew = BillDAO.Ins.getUnchecBillIdbyTableID(tableNew.Id);
            }
            TableDAO.Ins.SwitchTablebyIDBill(tableOld.Id, tableNew.Id, idBillOld, idBillNew);
            LoadTable();
            //lỗi
        }




        #endregion

        private void nmDisCount_ValueChanged(object sender, EventArgs e)
        {
            Table table = listViewBill.Tag as Table;
            List<Menu> ListBillInfo = MenuDAO.Ins.GetListMenuByTable(table.Id);
            float TotalPrice = 0;

            foreach(Menu item in ListBillInfo)
            {
                TotalPrice += item.Total;
            }
            float TotalReduce = TotalPrice - (TotalPrice * (float)nmDisCount.Value / 100);
            string strPrice = string.Format(new CultureInfo("vi-VN"), "{0:#,##0} VNĐ", TotalReduce);
            txtTotalPrice.Text = strPrice;
        }
    }
}
