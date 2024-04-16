using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHousekeepingBook
{
    public partial class ItemForm : Form
    {
        public ItemForm(CategoryDataSet dsCategory)
        {
            InitializeComponent();
            categoryDataSet.Merge(dsCategory);
            this.Text = "新規";
        }
        public ItemForm(CategoryDataSet dsCategory,
            DateTime nowDate,
            string category,
            string item,
            int money,
            string remarks)
        {
            InitializeComponent();
            categoryDataSet.Merge(dsCategory);
            monCalendar.SetDate(nowDate);   //カレンダー
            cmbCategory.Text = category;    //カテゴリー
            txtItem.Text = item;            //品名
            mtxtMoney.Text = money.ToString();//金額
            txtRemarks.Text = remarks;      //備考
            this.Text = "変更";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

        }
    }
}
