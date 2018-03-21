using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LuckyIT
{
    public partial class Form1 : Form
    {
        bool isEnd = false;
        DateTime oldTime = new DateTime();
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime oldTime = DateTime.Now;
            DataTable DtTemp = DbHelperOra.QueryDataTable("select staff_cname+' '+staff_code as description ,staff_code from lot_user where award='N'", "lot_user");
            label4.Text = Convert.ToString(DtTemp.Rows.Count) + "人";
            btnStart.Focus();

        }

        private void Start()
        {
            try {
                //设置循环的时间
                string time = "0.001";
                Random rand = new Random();

                DataTable DtTemp = DbHelperOra.QueryDataTable("select staff_cname+' '+staff_code as description ,staff_code from lot_user where award='N'", "lot_user");
                if (DtTemp.Rows.Count == 0)
                {
                    textBox1.Text = "所有人都已中奖";
                    return;
                }
                DataTable DtAwardName = DtTemp.Clone();
                foreach (DataRow row in DtTemp.Rows)
                {
                    DataRow dr = DtAwardName.NewRow();
                    dr.ItemArray = row.ItemArray;
                    Random random = new Random();
                    DtAwardName.Rows.InsertAt(dr, random.Next(DtAwardName.Rows.Count));
                }


                while (true)
                {
                    //设置循环的时间
                    int h = rand.Next(DtAwardName.Rows.Count);
                    if (isEnd)
                    {
                        textBox1.Text = DtAwardName.Rows[h]["description"].ToString();
                        textBox1.Enabled = true;
                        DbHelperOra.ExecuteSql("update lot_user set award='O',CREATEDATE=now() where staff_code='" + DtAwardName.Rows[h]["staff_code"].ToString() + "'");
                        label4.Text = Convert.ToString(DtAwardName.Rows.Count - 1) + "人";
                        break;
                    }
                    else
                    {
                        for (int y = 0; y < Convert.ToDecimal(time) * 1000; y++)
                        {
                            Application.DoEvents();
                        }

                        textBox1.Text = DtAwardName.Rows[h]["description"].ToString();
                    }
                }            
            }
            catch (Exception ex) {
                aWMD.close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string strTempPath = System.Windows.Forms.Application.StartupPath;
                if (oldTime.AddSeconds(3) > DateTime.Now)
                {
                    MessageBox.Show("请3秒后再抽下一个奖项", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                oldTime = DateTime.Now;
                isEnd = false;
                btnEnd.Visible = true;
                btnEnd.Focus();
                btnStart.Visible = false;
                textBox1.Enabled = false;
                aWMD.URL = strTempPath + "\\lottery.mp3";
                Start();
            }
            catch (Exception ex) {
                aWMD.close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isEnd = true;
            btnStart.Visible = true;
            btnEnd.Visible = false;
            aWMD.close();
            btnStart.Focus();           
        }

      


        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                return false;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //重新初始化
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认初始化吗?", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                //用户选择确认的操作
                DbHelperOra.ExecuteSql("update lot_user set award='N',CREATEDATE=NULL");
                DateTime oldTime = DateTime.Now;
                DataTable DtTemp = DbHelperOra.QueryDataTable("select staff_cname+' '+staff_code as description ,staff_code from lot_user where award='N'", "lot_user");
                label4.Text = Convert.ToString(DtTemp.Rows.Count) + "人";
                textBox1.Text = "";
                btnStart.Focus();
            }
            //else if (dr == DialogResult.Cancel)
            //{
            //    //用户选择取消的操作
            //    MessageBox.Show("您选择的是【取消】");
            //}

            
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            String strText = "";
            DataTable DtTemp = DbHelperOra.QueryDataTable("select staff_cname+' '+staff_code as description from lot_user where award='O' order by createdate", "lot_user");
            
            if (DtTemp.Rows.Count > 0) {
                foreach (DataRow row in DtTemp.Rows)
                {
                    strText += row["description"].ToString() + "\n";
                }

                MessageBox.Show(strText);            
            }
            
        }

        private void aWMD_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if ( btnStart.Visible ==false)
                aWMD.Ctlcontrols.play();
        }


    }
}
