using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_an_Ki_thuat_do_hoa
{
    public partial class ParallForm : Form
    {
        private Parall par;

        internal Parall Par
        {
            get
            {
                return par;
            }

            set
            {
                par = value;
            }
        }

        public ParallForm()
        {
            InitializeComponent();
        }

        private void ParallForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            if (!(int.TryParse(textBox_Xa.Text, out n) && int.TryParse(textBox_Ya.Text, out n) && int.TryParse(textBox_Xb.Text, out n) && int.TryParse(textBox_Yb.Text, out n) && int.TryParse(textBox_Xc.Text, out n) && int.TryParse(textBox_Yc.Text, out n)))
            {
                MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return;
            }
            par = new Parall(new Point(int.Parse(textBox_Xa.Text)+50, -int.Parse(textBox_Ya.Text)+50), new Point(int.Parse(textBox_Xb.Text)+50, -int.Parse(textBox_Yb.Text)+50), new Point(int.Parse(textBox_Xc.Text)+50, -int.Parse(textBox_Yc.Text)+50));
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
