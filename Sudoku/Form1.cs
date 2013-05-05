using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.RowCount = 9;
            dataGridView1.GetRowDisplayRectangle(3, false);
            dataGridView1.Rows.GetNextRow(2, DataGridViewElementStates.None);
            //var cell = dataGridView1.Rows[0].Cells[0];
            dataGridView1.Rows[2].DividerHeight = 3;
            dataGridView1.Rows[5].DividerHeight = 3;
            //dataGridView1.Rows[0].Cells[0].Style.BackColor = ColorTranslator.FromHtml("#C00");
            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Rows[i].Height = 25;
                for (int j = 0; j < 9; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = Color.Gray;
                }
            }

            dataGridView1.Rows[2].Height = 26;
            dataGridView1.Rows[5].Height = 26;
        }

        int view = 0;

        public void changeView(int view)
        {
            if (view == 1)
            {
                startPanel.Visible = false;
                mainGamePanel.Visible = true;
            }
            else if (view == 20)
            {
                highScorePanel.Visible = false;
                startPanel.Visible = true;
            }
            else if (view == 2)
            {
                startPanel.Visible = false;
                highScorePanel.Visible = true;
            }
            else if (view == 10)
            {
                mainGamePanel.Visible = false;
                startPanel.Visible = true;
            }

            else
            {
                MessageBox.Show("Ova uste ne e implementirano");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.SelectedCells[0].Style.BackColor = Color.White;
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            view = 1;
            changeView(view);
        }

        private void btnMainMenuBack_Click(object sender, EventArgs e)
        {
            view = 20;
            changeView(view);
        }

        private void btnScores_Click(object sender, EventArgs e)
        {
            changeView(2);
        }

        private void btnMainGameBack_Click(object sender, EventArgs e)
        {
            changeView(10);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown -= dataGridView1_KeyDown;
                tb.KeyDown += new KeyEventHandler(dataGridView1_KeyDown);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            
            //dataGridView1.SelectedCells[0].Value = "";
            var selected = dataGridView1.SelectedCells[0];
            if (!(e.KeyValue >= 49 && e.KeyValue <= 57 || e.KeyValue >= 97 && e.KeyValue <= 105))
            {
                //MessageBox.Show(String.Format("{0}",e.KeyValue));
                if (e.KeyValue == 27 || e.KeyValue == 8 || e.KeyValue == 46)
                {
                    selected.Value = "";
                }
            }
            else
            {
                if (e.KeyValue >= 49 && e.KeyValue <= 57)
                {
                    selected.Value = (char)e.KeyValue;
                }
                else
                {
                    selected.Value = String.Format("{0}", e.KeyValue - 96);
                } 
            }
        }
    }
}
