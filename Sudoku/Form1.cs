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
    public enum gameType
    {
        Standard,
        Squiggly
    }
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
    public partial class Form1 : Form
    {
        public PuzzleSolver standardSolver;
        public PuzzleGrid standardGrid;

        public SquigglySolver squigglySolver;
        public SquigglyGrid squigglyGrid;

        public static int NORMAL = 0;
        public static int LOCKED = 1;

        public Form1()
        {
            InitializeComponent();
            setTableView();

        }
        public void setTableView()
        {
            dataGridView1.RowCount = 9;
            dataGridView1.Rows[2].DividerHeight = 3;
            dataGridView1.Rows[5].DividerHeight = 3;
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
        public void clearHighlight()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
        }
        public void highlightSelectedNumber()
        {
            var selected = dataGridView1.SelectedCells[0];
            int value = -1;


            if (int.TryParse(selected.Value.ToString(), out value) && value != 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int val = -1;

                        if (int.TryParse(dataGridView1.Rows[i].Cells[j].Value.ToString(), out val) && val == value)
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            highlightSelectedNumber();
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            changeView(1);
            dataGridView1.Focus();
            dataGridView1.ClearSelection();
            clearHighlight();
            
            gameType type = gameType.Standard;
            if (radioSquigglyMode.Checked)
            {
                type = gameType.Squiggly;
            }
            Difficulty level = Difficulty.Easy;
            if (radioMediumMode.Checked)
            {
                level = Difficulty.Medium;
            }
            else if (radioHardMode.Checked)
            {
                level = Difficulty.Hard;
            }

            setGrid(type, level);
        }

        private void btnMainMenuBack_Click(object sender, EventArgs e)
        {
            changeView(20);
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
            if (dataGridView1.SelectedCells.Count > 0)
            {

                //dataGridView1.SelectedCells[0].Value = "";
                var selected = dataGridView1.SelectedCells[0];
                int sel_i=selected.RowIndex;
                int sel_j=selected.ColumnIndex;
                if (standardGrid.CellMap[sel_i, sel_j] == LOCKED)
                {
                    return;
                }
                if (!(e.KeyValue >= 49 && e.KeyValue <= 57 || e.KeyValue >= 97 && e.KeyValue <= 105))
                {
                    //MessageBox.Show(String.Format("{0}",e.KeyValue));
                    
                    if (e.KeyValue == 27 || e.KeyValue == 8 || e.KeyValue == 46)   // Use e.KeyCode == Keys.Enter  etc.
                    {
                        selected.Value = "";
                    }

                }
                else
                {
                    int value = -1;
                    if (e.KeyValue >= 49 && e.KeyValue <= 57)
                    {
                        selected.Value = e.KeyValue - 48;
                        value = e.KeyValue - 48;
                    }
                    else
                    {
                        selected.Value = e.KeyValue - 96;
                        value = e.KeyValue - 96;
                    }



                    highlightSelectedNumber();

                }

            }
        }
        public void LockCellMap()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (standardGrid.Grid[i, j] != 0)
                    {
                        standardGrid.CellMap[i, j] = LOCKED;
                        //dataGridView1.Rows[i].Cells[j].Style.Font = new Font(dataGridView1.Rows[i].Cells[j].Style.Font, FontStyle.Bold);
                        dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml("#FFA1A1");
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml("#61D465");
                    }
                }
            }
        }
        public void setGrid(gameType type, Difficulty level)
        {

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = "";
                }
            }
            if (type == gameType.Standard)
            {

                PuzzleGenerator gen = new PuzzleGenerator(level);
                PuzzleGrid grid = gen.InitGrid();
                standardSolver = new PuzzleSolver();
                standardSolver.SolutionGrid = gen.SolutionGrid;
                standardGrid = new PuzzleGrid();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (grid.Grid[i, j] != 0)
                            dataGridView1.Rows[i].Cells[j].Value = -grid.Grid[i, j];
                        standardGrid.Grid[i, j] = -grid.Grid[i, j];
                    }
                }

            }
            else
            {
                /*SquigglyGenerator gen = new SquigglyGenerator(level);
                SquigglyGrid grid = gen.InitGrid();
                squigglySudoku = new SquigglySudoku(grid.Grid, gen.SolutionGrid.Grid, new List<List<Field>>());

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (grid.Grid[i, j] != 0)
                            dataGridView1.Rows[i].Cells[j].Value = -grid.Grid[i, j];
                    }
                }*/
            }
            LockCellMap();
        }
    }
}
