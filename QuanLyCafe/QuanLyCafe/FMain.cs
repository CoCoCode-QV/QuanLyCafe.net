using QuanLyCafe.DAO;
using QuanLyCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
            LoadTable();
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
            List<BillInfo> listBillInfo = BillInfoDAO.Ins.GetListBillInfo(BillDAO.Ins.getUnchecBillIdbyTableID(index));

            foreach(BillInfo item in listBillInfo)
            {
                ListViewItem listViewItem = new ListViewItem(item.FoodId.ToString());
                listViewItem.SubItems.Add(item.Count.ToString());
                listViewBill.Items.Add(listViewItem);
            }

        }
        #endregion

        #region events
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableId = Convert.ToInt32((sender as Button).Name);

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

        #endregion
    }
}
