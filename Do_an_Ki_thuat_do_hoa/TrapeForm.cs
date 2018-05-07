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
    public partial class TrapeForm : Form
    {
        private Trape tra;

        internal Trape Tra
        {
            get
            {
                return tra;
            }

            set
            {
                tra = value;
            }
        }

        public TrapeForm()
        {
            InitializeComponent();
        }

        private void TrapeForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void ok_Click(object sender, EventArgs e)
        {
            int n;
            if (!int.TryParse(textBox_X1.Text, out n) && int.TryParse(textBox_Y1.Text, out n) && int.TryParse(textBox_botS.Text, out n) && int.TryParse(textBox_topS.Text, out n) && int.TryParse(textBox_X2.Text, out n) && int.TryParse(textBox_Y2.Text, out n))
            {
                MessageBox.Show("Nhap sai. Hay nhap so", "Canh bao!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                return;
            }
            tra = new Trape(new Point(int.Parse(textBox_X1.Text)+50, -int.Parse(textBox_Y1.Text)+50), new Point(int.Parse(textBox_X2.Text)+50, -int.Parse(textBox_Y2.Text)+50), int.Parse(textBox_botS.Text), int.Parse(textBox_topS.Text));
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
