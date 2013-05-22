using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
namespace Sudoku
{
    public partial class Form2 : Form
    {
        public string text = "";

        public Form2()
        {
            InitializeComponent();
        }

        private void name_TextChanged(object sender, EventArgs e)
        {
            int length = name.TextLength;

            if (length > 10)
            {
                System.Media.SystemSounds.Asterisk.Play();
                length = 10;
            }

            if (length == 10)
            {
                name.Text = name.Text.Substring(0, 10);
                name.SelectionStart = name.TextLength;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (name.TextLength == 0)
            {
                MessageBox.Show("Please enter your name.");
                return;
            }
            else
            {
                text = name.Text;
                this.Close();
            }
        }

        private void thanks_Click(object sender, EventArgs e)
        {
            //this.Close();
        }

    }
}
