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
        public int ticks;
        public gameType type;
        public Difficulty diff;
        public Form1 parent;

        public Form2(Form1 parent,int ticks,gameType type, Difficulty diff)
        {   
            this.parent = parent;
            this.ticks = ticks;
            this.type = type;
            this.diff = diff;
            InitializeComponent();
            name.Focus();
        }
        /// <summary>
        /// Checks if the length of the name entered in the textBox is longer than 10 characters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Saves the achieved score in the list of high scores 
        /// for the respective sudoku type and difficulty level.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveScore_Click(object sender, EventArgs e)
        {
            if (name.TextLength == 0)
            {
                MessageBox.Show("Please enter your name.");
                return;
            }
            else
            {
                parent.submitHighScore(name.Text, ticks, type, diff);
                text = name.Text;
                parent.setHighScoresPanel(type, diff);
                parent.changeView(2);
                this.Close();
            }
            
        }
        /// <summary>
        /// Avoid saving the high score achieved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thanks_Click(object sender, EventArgs e)
        {
            text = "";
            this.Close();
        }

    }
}
