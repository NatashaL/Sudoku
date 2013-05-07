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

        public StandardSudoku standardSudoku;
        public PuzzleSolver standardSolver;
        public PuzzleGrid standardGrid;

        public SquigglySudoku squigglySudoku;
        public SquigglySolver squigglySolver;
        public SquigglyGrid squigglyGrid;

        public Form1()
        {
            InitializeComponent();
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
                dataGridView1.Focus();
                dataGridView1.ClearSelection();

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
            if (dataGridView1.SelectedCells.Count > 0)
            {
                dataGridView1.SelectedCells[0].Style.BackColor = Color.White;
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            changeView(1);
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
                        value = e.KeyValue-48;
                    }
                    else
                    {
                        selected.Value = e.KeyValue - 96;
                        value = e.KeyValue - 96;
                    }



                    int i = selected.RowIndex;
                    int j = selected.ColumnIndex;

                    if (standardSolver.IsPossible(standardGrid, i, j, value))
                    {
                        standardGrid.Grid[i, j] = value;
                    }
                    else
                    {
                        int ii = -1;
                        int jj = -1;
                        standardGrid.Grid[i, j] = value;
                        if (standardSolver.IsInRow(standardGrid, i, value, out ii, out jj))
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;                            
                            dataGridView1.Rows[ii].Cells[jj].Style.BackColor = Color.Red;
                        }

                        if (standardSolver.IsInCol(standardGrid, j, value, out ii, out jj))
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            dataGridView1.Rows[ii].Cells[jj].Style.BackColor = Color.Red;
                        }

                        if (standardSolver.IsIn3X3(standardGrid, i, j, value, out ii, out jj))
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            dataGridView1.Rows[ii].Cells[jj].Style.BackColor = Color.Red;
                        }

                        if (!(standardSolver.IsInRow(standardGrid, i, value, out ii, out jj)
                            && standardSolver.IsInCol(standardGrid, j, value, out ii, out jj)
                            && standardSolver.IsIn3X3(standardGrid, i, j, value, out ii, out jj)))
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                            dataGridView1.Rows[ii].Cells[jj].Style.BackColor = Color.White;
                        }
                    }


                }
                
                if (standardSolver.SolveGrid(standardGrid, false))
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.White;
                        }
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
                standardSudoku = new StandardSudoku(grid.Grid, gen.SolutionGrid.Grid);
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
        }
    }

}
