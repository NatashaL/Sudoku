using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
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
        public Standard standard;
        public Squiggly squiggly;

        public static int[,] CellMap;
        public static Color [,] ColorMap;
        public static int NORMAL = 0;
        public static int LOCKED = 1;

        public List<int[,]> Schemes = new List<int[,]>();
        public List<Color> colors = new List<Color>();


        public Form1()
        {

            InitializeComponent();
            schemeBuilder();


            colors.Add(Color.AliceBlue);
            colors.Add(Color.Lavender);
            colors.Add(Color.MistyRose);
            colors.Add(Color.LightCoral);
            colors.Add(Color.LightYellow);
            colors.Add(Color.LightSkyBlue);
            colors.Add(Color.LightSlateGray);
            colors.Add(Color.LightSalmon);
            colors.Add(Color.Ivory);
            ColorMap = new Color[9, 9];
        }
        public void setSquigglyTableView()
        {
            dataGridView1.RowCount = 9;
            dataGridView1.Width = 228;
            dataGridView1.Height = 228;
            dataGridView1.Rows[2].DividerHeight = 0;
            dataGridView1.Rows[5].DividerHeight = 0;
            dataGridView1.Columns[2].DividerWidth = 0;
            dataGridView1.Columns[5].DividerWidth = 0;
            dataGridView1.Columns[2].Width = 25;
            dataGridView1.Columns[5].Width = 25;
            dataGridView1.Rows[2].Height = 25;
            dataGridView1.Rows[5].Height = 25;
            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Rows[i].Height = 25;
            }
        }
        public void setStandardTableView()
        {
            dataGridView1.RowCount = 9;
            dataGridView1.Rows[2].DividerHeight = 3;
            dataGridView1.Rows[5].DividerHeight = 3;
            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Rows[i].Height = 25;
            }
            dataGridView1.Rows[2].Height = 26;
            dataGridView1.Rows[5].Height = 26;
            dataGridView1.Columns[2].DividerWidth = 3;
            dataGridView1.Columns[5].DividerWidth = 3;
            dataGridView1.Columns[2].Width = 26;
            dataGridView1.Columns[5].Width = 26;
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
                            dataGridView1.Rows[i].Cells[j].Style.BackColor = ColorMap[i,j];
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (standard == null && squiggly == null) return;
            else highlightSelectedNumber();
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {


            gameType type;
            if (radioSquigglyMode.Checked)
            {
                setSquigglyTableView();
                type = gameType.Squiggly;
            }
            else
            {
                setStandardTableView();
                type = gameType.Standard;
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

            changeView(1);
            dataGridView1.Focus();
            dataGridView1.ClearSelection();
            clearHighlight();


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
            if (standard == null && squiggly == null) return;
            if (dataGridView1.SelectedCells.Count > 0)
            {

                //dataGridView1.SelectedCells[0].Value = "";
                var selected = dataGridView1.SelectedCells[0];
                int sel_i = selected.RowIndex;
                int sel_j = selected.ColumnIndex;
                if (CellMap[sel_i, sel_j] == LOCKED) return;
                if (!(e.KeyValue >= 49 && e.KeyValue <= 57 || e.KeyValue >= 97 && e.KeyValue <= 105))
                {
                    //MessageBox.Show(String.Format("{0}",e.KeyValue));

                    if (e.KeyValue == 27 || e.KeyValue == 8 || e.KeyValue == 46)   // Use e.KeyCode == Keys.Enter  etc.
                    {
                        selected.Value = "";
                        if (standard != null)
                        {
                            standard.userGrid[selected.RowIndex, selected.ColumnIndex] = 0;
                        }
                        else
                        {
                            squiggly.userGrid[selected.RowIndex, selected.ColumnIndex] = 0;
                        }
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

                    if(standard != null)
                        standard.userGrid[sel_i, sel_j] = value;
                    else if(squiggly != null)
                        squiggly.userGrid[sel_i, sel_j] = value;

                    highlightSelectedNumber();

                }

            }
            if (standard != null)
            {
                if (Sudoku.isSolved(standard.userGrid,Standard.scheme))
                {
                    MessageBox.Show("Congratulations!!!");
                    DarkenGrid();
                    standard = null;
                }
            }
            else if (squiggly != null)
            {
                if (Sudoku.isSolved(squiggly.userGrid,squiggly.scheme))
                {
                    DarkenGrid();
                    MessageBox.Show("Congratulations!!!");
                    squiggly = null;
                }
            }

        }
        public void DarkenGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for(int j=0; j<9;j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                    dataGridView1.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightGray;
                    dataGridView1.Rows[i].Cells[j].Style.SelectionForeColor = Color.Black;
                }
            }
        }
        public void LockCellMap()
        {
            CellMap = new int[9, 9];
            CellMap.Initialize();


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Value != "")
                    {
                        CellMap[i, j] = LOCKED;
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
                squiggly = null;
                standard = new Standard(level);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (standard.mask[i, j] != 0)
                            dataGridView1.Rows[i].Cells[j].Value = standard.mask[i, j];
                        ColorMap[i, j] = Color.White;
                    }
                }
            }
            else
            {
                standard = null;
                schemeBuilder();
                Random random = new Random();
                squiggly = null;
                bool Completed = false;
                while (squiggly == null && !Completed)
                {
                    squiggly = Limex(() => new Squiggly(level,Schemes[random.Next(0,Schemes.Count)]), 4000,out Completed);
                }

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (squiggly.mask[i, j] != 0)
                            dataGridView1.Rows[i].Cells[j].Value = squiggly.mask[i, j];
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = colors[squiggly.scheme[i, j]];
                        ColorMap[i, j] = colors[squiggly.scheme[i, j]];
                    }
                }
            }
            LockCellMap();
        }
        public static CustomSquiggly Limex<CustomSquiggly>(Func<CustomSquiggly> F, int Timeout, out bool Completed)
        {
            CustomSquiggly result = default(CustomSquiggly);
            Thread thread = new Thread(() => result = F());
            thread.Start();
            Completed = thread.Join(Timeout);
            if (!Completed) thread.Abort();
            return result;
        }
    }


}
