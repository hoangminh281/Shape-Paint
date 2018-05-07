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
    public partial class DiamondForm : Form
    {
        private Diamond dia;

        internal Diamond Dia
        {
            get
            {
                return dia;
            }

            set
            {
                dia = value;
            }
        }

        public DiamondForm()
        {
            InitializeComponent();
        }

        private void DiamondForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            int n;
            if (!(int.TryParse(textBox_X1.Text, out n) && int.TryParse(textBox_Y1.Text, out n) && int.TryParse(textBox_X2.Text, out n) && int.TryParse(textBox_Y2.Text, out n)))
            {
                MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return;
            }
            dia = new Diamond(new Point(int.Parse(textBox_X1.Text) + 50, -int.Parse(textBox_Y1.Text) + 50), new Point(int.Parse(textBox_X2.Text) + 50, -int.Parse(textBox_Y2.Text) + 50));
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
