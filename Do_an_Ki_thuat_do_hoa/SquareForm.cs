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
    public partial class SquareForm : Form
    {
        private Square squa;

        internal Square Squa
        {
            get
            {
                return squa;
            }

            set
            {
                squa = value;
            }
        }

        public SquareForm()
        {
            InitializeComponent();
        }

        private void SquareForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_X.Text, out n) && int.TryParse(textBox_Y.Text, out n) && int.TryParse(textBox_size.Text, out n))
            {
                squa = new Square(new Point(int.Parse(textBox_X.Text)+50, -int.Parse(textBox_Y.Text)+50), int.Parse(textBox_size.Text));
                this.Close();
            } else MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
