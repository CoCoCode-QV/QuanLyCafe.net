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
using Menu = QuanLyCafe.DTO.Menu;

namespace QuanLyCafe
{
    public partial class fOverview : Form
    {
        public fOverview()
        {
            InitializeComponent();
            LoadOverViewRevenue();
            LoadOverViewPeople();
        }

        private void LoadOverViewRevenue()
        {
            List<Bill> listBill = new List<Bill>();
            listBill = BillDAO.Ins.GetBillListOfDay();
            string formattedTotal = returnTotal(listBill);
            LbTotalOverView.Text = formattedTotal;

            List<Bill> listBillPaid = new List<Bill>();
            listBillPaid = BillDAO.Ins.GetBillListOfDayPaid();
            string formattedTotalPaid = returnTotal(listBillPaid);
            lbpaid.Text = formattedTotalPaid;

            List<Bill> listBillServing = new List<Bill>();
            listBillServing = BillDAO.Ins.GetMenuListOfDayServing();
            string formattedTotalServing = returnTotal(listBillServing);
            lbServing.Text = formattedTotalServing;
        }

        private string returnTotal(List<Bill> a)
        {
            float TotalPrice = 0;
            foreach (Bill item in a)
            {
                List<Menu> menu = MenuDAO.Ins.GetMenuByBill(item.BillId);
                float PriceTotal = 0;
                foreach (Menu temp in menu)
                {
                    PriceTotal += temp.Total;
                }
                float TotalReduce = PriceTotal - (PriceTotal * item.Discount / 100);
                TotalPrice += TotalReduce;
            }
            string formattedTotal = $"{TotalPrice:N0} VNĐ";
            return formattedTotal;
        }
      
        private void LoadOverViewPeople()
        {
            lbCustomer.Text = BillDAO.Ins.CountBillOfPeople().ToString();
            lbCustomerFinish.Text = BillDAO.Ins.CountBillOfPeopleFinish().ToString();
            lbCustomerServing.Text = BillDAO.Ins.CountBillOfPeopleServing().ToString();
        }
    }
}
