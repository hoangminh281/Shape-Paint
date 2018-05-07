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
    public partial class PointForm : Form
    {
        private Point point;

        internal Point Point
        {
            get
            {
                return point;
            }

            set
            {
                point = value;
            }
        }

        public PointForm()
        {
            InitializeComponent();
        }

        private void PointForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        public void ok_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_X.Text, out n) && int.TryParse(textBox_Y.Text, out n))
            {
                point = new Point(int.Parse(textBox_X.Text)+50, -int.Parse(textBox_Y.Text)+50);
                this.Close();
            }
            else MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
