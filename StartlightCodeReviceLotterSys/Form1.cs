using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public List<string> CandidateList;
        public int index;
        public List<WinningPeople> WinningPeoples = new List<WinningPeople>();

        public Form1()
        {

            string userStr = ConfigurationSettings.AppSettings["Users"];
            string WinningPeoplesCount = ConfigurationSettings.AppSettings["LotteryNumber"];
            if (userStr != null && WinningPeoplesCount != null)
            {
                CandidateList = userStr.Split(',').ToList();
                var lotteryNumber = int.Parse(WinningPeoplesCount) > CandidateList.Count() ? CandidateList.Count() : int.Parse(WinningPeoplesCount);
                for (int i = 0; i < lotteryNumber / 2; i++)
                {
                    WinningPeoples.Add(new WinningPeople());
                }
            }
            else
            {
                throw new Exception("请填写正确的配置");
            }

            InitializeComponent();
            this.AcceptButton = btnLotter;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataLoad();
        }
        public void DataLoad()
        {
            this.timer1.Start();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = WinningPeoples;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            var count = CandidateList.Count;
            Random rd = new Random(DateTime.Now.Millisecond);
            index = rd.Next(0, count);
            if (!CandidateList.Any())
            {
                this.timer1.Stop();
                return;
            }
            textBox1.Text = CandidateList[index];
        }

        private void LuckyDraw()
        {
            textBox1.Text = "";
            if (CandidateList.Count > 0)
            {
                this.timer1.Stop();
                var data = CandidateList[index];
                CandidateList.Remove(data);
                foreach (var item in WinningPeoples)
                {
                    if (item.CodeReviewName == null)
                    {
                        item.CodeReviewName = data;
                        DataLoad();
                        return;
                    }
                    else if (item.updateCodeName == null)
                    {
                        item.updateCodeName = data;
                        DataLoad();
                        return;
                    }
                }
                MessageBox.Show("本次抽奖结束");
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("本次抽奖结束");
            }
        }
        public class WinningPeople
        {
            [DisplayName("Code Review")]
            public string CodeReviewName { get; set; }
            [DisplayName("Code Update")]
            public string updateCodeName { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LuckyDraw();
        }
    }
}
