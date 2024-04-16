using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
/*
 * SQLServerに先にテーブルを作製
 * 
 *  //IDは自動増番にする
 *  create table moneyTable (
 *  ID int IDENTITY (1,1),
 *  日付 date not null,
 *  分類 varchar(30) not null,
 *  品名 varchar(30),
 *  金額 int,
 *  備考 varchar(50),
 *  primary key(ID) );
 *  
 *  create table categoryTable (
 *  分類 varchar(10) not null,
 *  入出金分類 varchar(10),
 *  primary key(分類));
 * 
 * 
 */
namespace MyHousekeepingBook
{
    public partial class formView : Form
    {
        public string connString = "Data Source=localhost\\SQLEXPRESS;" +
            "Initial Catalog=mydb;" +
            "Integrated Security=SSPI;" +
            "TrustServerCertificate=true;";
        public formView()
        {
            InitializeComponent();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddData();
        }

        private void AddData()
        {
            //カテゴリーデータをコンボボックスに渡す
            ItemForm frmItem = new ItemForm(categoryDataSet1);
            DialogResult drRet = frmItem.ShowDialog();
            if (drRet == DialogResult.OK)
            {
                DateTime dt =
                    frmItem.monCalendar.SelectionRange.Start;
                string br = frmItem.cmbCategory.Text;
                string hm = frmItem.txtItem.Text;
                int kg = int.Parse(frmItem.mtxtMoney.Text);
                string bk = frmItem.txtRemarks.Text;
                //SQLでSQLserverに追加する
                SaveData(dt, br, hm, kg, bk);
                //再表示する
                LoadData();
            }
        }

        private void SaveData(DateTime dt,string br, string hm, int kg, string bk)
        {
            //auto incrementなのでIDは書かない
            string strSql = "INSERT INTO [moneyTable] ([日付],[分類],[品名],[金額],[備考]) VALUES (@日付, @分類, @品名,@金額,@備考)";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    conn.Open();
                    cmd.CommandType = CommandType.Text;

                    //strSQLの@の部分にパラメータを展開する
                    cmd.Parameters.Clear();
                    SqlParameter param = cmd.CreateParameter();
                    param = cmd.CreateParameter();
                    param.ParameterName = "@日付";
                    param.SqlDbType = SqlDbType.Date;
                    param.Direction = ParameterDirection.Input;
                    param.Value = dt;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@分類";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 30;
                    param.Direction = ParameterDirection.Input;
                    param.Value = br;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@品名";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 30;
                    param.Direction = ParameterDirection.Input;
                    param.Value = hm;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@金額";
                    param.SqlDbType = SqlDbType.Int;
                    param.Direction = ParameterDirection.Input;
                    param.Value = kg;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@備考";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Size = 30;
                    param.Value = bk;
                    cmd.Parameters.Add(param);

                    //リターンのないSQLを実行する
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("書き込み失敗");
                }
            }
        }

        private void UpData(int id, DateTime dt, string br, string hm, int kg, string bk)
        {
            string strSql = "UPDATE [moneyTable] SET [日付]=@日付,[分類]=@分類,[品名]=@品名,[金額]=@金額,[備考]=@備考 WHERE [ID]=@ID";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    conn.Open();
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Clear();
                    SqlParameter param = cmd.CreateParameter();
                    param = cmd.CreateParameter();
                    param.ParameterName = "@日付";
                    param.SqlDbType = SqlDbType.Date;
                    param.Direction = ParameterDirection.Input;
                    param.Value = dt;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@分類";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 30;
                    param.Direction = ParameterDirection.Input;
                    param.Value = br;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@品名";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 30;
                    param.Direction = ParameterDirection.Input;
                    param.Value = hm;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@金額";
                    param.SqlDbType = SqlDbType.Int;
                    param.Direction = ParameterDirection.Input;
                    param.Value = kg;
                    cmd.Parameters.Add(param);

                    param = cmd.CreateParameter();
                    param.ParameterName = "@備考";
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Size = 30;
                    param.Value = bk;
                    cmd.Parameters.Add(param);

                    //primary key
                    param = cmd.CreateParameter();
                    param.ParameterName = "@ID";
                    param.SqlDbType = SqlDbType.Int;
                    param.Direction = ParameterDirection.Input;
                    param.Value = id;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("変更失敗");
                }
            }
        }

        private void formView_Load(object sender, EventArgs e)
        {
            //カテゴリーデータを最初に一度だけ読み込む
            string strSql = "select * from categoryTable;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    DataRow drow;
                    while (dr.Read())
                    {
                        drow = categoryDataSet1.CategoryDataTable.NewRow();
                        for (int i = 0; i < dr.FieldCount; i++)
                        {
                            drow[i] = dr[i];
                        }
                        categoryDataSet1.CategoryDataTable.Rows.Add(drow);
                    }
                    //while(dr.Read())
                    //{
                    //    categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow(
                    //        dr["分類"].ToString(),
                    //        dr["入出金分類"].ToString());
                    //}
                }
            }
            //moneyTableを読んで再表示する
            LoadData();

        }

        private void 追加AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddData();
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveData();
        }

        private void formView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveData();
        }

        private void LoadData()
        {
            string sqlStr = "Select * from moneyTable";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sqlStr, conn);
                conn.Open();

                //一度データを消して再読み込みする
                moneyDataSet.moneyDataTable.Clear();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    DataRow drow;
                    while (dr.Read())
                    {
                        drow = moneyDataSet.moneyDataTable.NewRow();
                        for(int i = 0;i<dr.FieldCount;i++)
                        {
                            drow[i] = dr[i];
                        }
                        moneyDataSet.moneyDataTable.Rows.Add(drow);
                    }
                    //while (dr.Read())
                    //{
                    //    moneyDataSet.moneyDataTable.AddmoneyDataTableRow(
                    //        int.Parse(dr[0].ToString()),
                    //        DateTime.Parse(dr[1].ToString()),
                    //        dr[2].ToString(),
                    //        dr[3].ToString(),
                    //        int.Parse(dr[4].ToString()),
                    //        dr[5].ToString()
                    //        );
                    //}
                }

            }
        }
        private void UpdateData()
        {
            //データグリッゴビューのデータを変更する
            int nowRow = dgv.CurrentRow.Index;
            //oldIDを取っておいて、書き戻す
            int oldID = int.Parse(dgv.Rows[nowRow].Cells[0].Value.ToString());
            DateTime oldDate = 
                DateTime.Parse(dgv.Rows[nowRow].Cells[1].Value.ToString());
            string oldCategory = dgv.Rows[nowRow].Cells[2].Value.ToString();
            string oldItem = dgv.Rows[nowRow].Cells[3].Value.ToString();
            int oldMoney = int.Parse(dgv.Rows[nowRow].Cells[4].Value.ToString());
            string oldRemarks = dgv.Rows[nowRow].Cells[5].Value.ToString();

            ItemForm frmItem = new ItemForm(categoryDataSet1,
                oldDate,
                oldCategory,
                oldItem,
                oldMoney,
                oldRemarks);
            DialogResult drRet = frmItem.ShowDialog();
            //OKならば
            if (drRet == DialogResult.OK)
            {
                DateTime dt =
                    frmItem.monCalendar.SelectionRange.Start;
                string br = frmItem.cmbCategory.Text;
                string hm = frmItem.txtItem.Text;
                int kg = int.Parse(frmItem.mtxtMoney.Text);
                string bk = frmItem.txtRemarks.Text;
                //同じIDのところにUPDATEで書き戻す
                UpData(oldID, dt, br, hm, kg, bk);
                //再読み込みをして書き直す
                LoadData();
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void 変更CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void DeleteData()
        {
            //現在行のIDを取得して削除する
            int nowRow = dgv.CurrentRow.Index;
            int ID = int.Parse(dgv.Rows[nowRow].Cells[0].Value.ToString());

            string strSql = "DELETE FROM [moneyTable] WHERE [ID]=@ID";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(strSql, conn);
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();

                    SqlParameter param = cmd.CreateParameter();
                    //primary key
                    param = cmd.CreateParameter();
                    param.ParameterName = "@ID";
                    param.SqlDbType = SqlDbType.Int;
                    param.Direction = ParameterDirection.Input;
                    param.Value = ID;
                    cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("削除失敗");
                }
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteData();
        }
        private void CalcSummary()
        {
            string expression;
            summaryDataSet.SumDataTable.Clear();
            //データテーブルの中を参照
            foreach(MoneyDataSet.moneyDataTableRow drMoney
                in moneyDataSet.moneyDataTable)
            {
                //日付で絞り込んだレコードの配列を取得する
                expression = "日付= '" + drMoney.日付.ToShortDateString() + "'";
                SummaryDataSet.SumDataTableRow[] curDR
                    = (SummaryDataSet.SumDataTableRow[])
                        summaryDataSet.SumDataTable.Select(expression);

                if (curDR.Length == 0)
                {
                    CategoryDataSet.CategoryDataTableRow[] selectedDataRow;
                    selectedDataRow = (CategoryDataSet.CategoryDataTableRow[])
                        categoryDataSet1.CategoryDataTable.Select(
                            "分類='" + drMoney.分類 + "'");
                    if (selectedDataRow[0].入出金分類 == "入金")
                    {
                        summaryDataSet.SumDataTable.AddSumDataTableRow(
                            drMoney.日付, drMoney.金額, 0);
                    }
                    else if (selectedDataRow[0].入出金分類 == "出金")
                    {
                        summaryDataSet.SumDataTable.AddSumDataTableRow(
                            drMoney.日付, 0, drMoney.金額);
                    }

                }
                else
                {
                    CategoryDataSet.CategoryDataTableRow[] selectedDataRow;
                    selectedDataRow =
                        (CategoryDataSet.CategoryDataTableRow[])
                        categoryDataSet1.CategoryDataTable.Select(
                            "分類='" + drMoney.分類 + "'");
                    if (selectedDataRow[0].入出金分類 == "入金")
                    {
                        curDR[0].入金合計 += drMoney.金額;
                    }
                    else if (selectedDataRow[0].入出金分類 == "出金")
                    {
                        curDR[0].出金合計 += drMoney.金額;
                    }
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSummary();
        }

        private void 一覧表示LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabList);
        }

        private void 集計表示SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabSummary);
        }

    }
}
