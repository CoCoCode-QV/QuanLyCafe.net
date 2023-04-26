 using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Menu = QuanLyCafe.DTO.Menu;

namespace QuanLyCafe
{
     partial class fMain : Form
    {
        public fMain(Account account)
        {
            InitializeComponent();
            changeAccount(account);
            Load();
        }

        #region Method

        private void Load()
        {
            LoadTable();
            loadCategory();
            loadComboBoxSwitchTable(cbSwitchTable);
        }

        private void changeAccount(Account account)
        {
            adminToolMnStrip.Enabled = account.Type == 1 ? true : false;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " - " + account.DisplayName;
        }

        private void LoadTable()
        {
            flPanelTable.Controls.Clear();
            List<Table> Table =  TableDAO.Ins.loadTableList();

            foreach(Table item  in Table)
            {
                Button btn = new Button() { Width = 90, Height = 90};
                btn.Text = item.Name;
                btn.Name = item.Id.ToString();
                btn.Tag = item;
                btn.Font = new Font("Arial", 12);
                btn.Click += Btn_Click;
                switch(item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.LightSeaGreen;
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
            txtReceived.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0} VNĐ", 0); ;

        }

        private void loadCategory()
        {
            List<Category> listCategory = CategoryDAO.Ins.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "NameCategory";
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
            int discount =(int)nmDisCount.Value;

            try
            {
                string strReturn = txtMoneyReturn.Text.Replace(" VNĐ", "");
                float MoneyReturn = float.Parse(strReturn);
                if (MoneyReturn < 0)
                {
                    MessageBox.Show("Vui lòng nhập số tiền nhận của khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (idBill != -1)
                {
                    if (MessageBox.Show("Bạn có muốn thanh toán hóa đơn " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        BillDAO.Ins.CheckOut(idBill, discount);
                        showBill(table.Id);
                        LoadTable();
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Vui lòng nhập số tiền nhận của khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        }


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
            string formattedTotal = $"{TotalReduce:N0} VNĐ";
            txtTotalPrice.Text = formattedTotal;
        }

        private void tổngQuanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fOverview f = new fOverview();
            f.ShowDialog();
        }

        private void txtReceived_Enter(object sender, EventArgs e)
        {
            string strReceived = txtReceived.Text.Replace(" VNĐ", "");
            string strTotalPrice = txtTotalPrice.Text.Replace(" VNĐ", "");
            try
            {
                float totalPrice = float.Parse(strTotalPrice);
                float Received = strReceived != "" ? float.Parse(strReceived) : 0;
                float MoneyReturn = Received - totalPrice;
                txtMoneyReturn.Text = string.Format(new CultureInfo("vi-VN"), "{0:#,##0} VNĐ", MoneyReturn);
            }catch(FormatException ex)
            {
                return;
            }

        }

        private Stopwatch _stopwatch;
        private const int _DWELL_TIME_THRESHOLD = 500;
        private Timer _searchTimer;
        private void txtReceived_TextChanged(object sender, EventArgs e)
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
                _searchTimer.Tick += new EventHandler(txtReceived_Enter);
            }
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        #endregion




    }
}
