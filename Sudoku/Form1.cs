using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
        public static int LOCKED = 1;
        public int ticks = 0;
        public Scores HS;
        public string FileName;
        public Form1()
        {

            InitializeComponent();
            schemeBuilder();
            // TO DO default view of highScores Martin
            HS = BinaryDeserializeScores();
            setHighScoresPanel(gameType.Standard,Difficulty.Easy);
            
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
                highScorePanel.Visible = false;
                mainGamePanel.Visible = true;
            }
            else if (view == 20)
            {
                highScorePanel.Visible = false;
                mainGamePanel.Visible = false;
                startPanel.Visible = true;
            }
            else if (view == 2)
            {
                startPanel.Visible = false;
                mainGamePanel.Visible = false;
                highScorePanel.Visible = true;
            }
            else if (view == 10)
            {
                mainGamePanel.Visible = false;
                highScorePanel.Visible = false;
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

            if (standard == null && squiggly == null)
            {
                setGrid(type, level);
            }
            else
            {

            }
            ticks = 0;
            timer.Start();
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
            if (standard == null && squiggly == null)
            {
                return;
            }
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

            if((standard != null && Sudoku.isSolved(standard.userGrid,Standard.scheme)) || 
                (squiggly != null && Sudoku.isSolved(squiggly.userGrid,squiggly.scheme))){
                
                long time = ticks;
                timer.Stop();
                DarkenGrid();
                //MessageBox.Show("Congratulations!!!");
                
                gameType type = standard == null ? gameType.Squiggly : gameType.Standard;
                Difficulty level = standard == null ? squiggly.diff : standard.diff;
                int d = level == Difficulty.Easy ? 0 : (level == Difficulty.Medium ? 1 : 2);

                if (HS.HS[type][d].highScores.Count == 5 && ticks > HS.HS[type][d].highScores[4].time)
                {
                    MessageBox.Show("\t\tCongratulations!!! \n I'm sorry but you didn't make it in the top 5.");
                }
                else
                {
                    Form2 form2 = new Form2(this, ticks, type, level);
                    form2.Show();
                    form2.name.Focus();
                }
                standard = null;
                squiggly = null;
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
            if (standard != null || type == gameType.Standard)
            {
                if (standard == null)
                {
                    squiggly = null;
                    standard = new Standard(level);
                }
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
                if (squiggly == null)
                {
                    standard = null;
                    schemeBuilder();

                    Random random = new Random();
                    squiggly = null;
                    bool Completed = false;
                    while (squiggly == null && !Completed)
                    {
                        squiggly = Limex(() => new Squiggly(level, Schemes[random.Next(0, Schemes.Count)]), 4000, out Completed);
                    }
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

        private void timer_Tick(object sender, EventArgs e)
        {
            ticks++;
            timerlabel.Text = (new TimeSpan(ticks * 10000000)).ToString();

        }
        public void setHighScoresPanel(gameType type, Difficulty diff)
        {
            //HS = BinaryDeserialize();

            lblScoresType.Text = type == gameType.Standard ? "Standard" : "Squiggly";
            lblScoresDiff.Text = diff == Difficulty.Easy ? "Easy" : (diff == Difficulty.Medium ? "Medium" : "Hard");
            int d = diff == Difficulty.Easy ? 0 : (diff == Difficulty.Medium ? 1 : 2);
            List<Label> visibleNames = new List<Label>();
            visibleNames.Add(label2);
            visibleNames.Add(label5);
            visibleNames.Add(label7);
            visibleNames.Add(label9);
            visibleNames.Add(label11);

            List<Label> visibleTimes = new List<Label>();
            visibleTimes.Add(label3);
            visibleTimes.Add(label4);
            visibleTimes.Add(label6);
            visibleTimes.Add(label8);
            visibleTimes.Add(label10);

            HighScores toBeShown = HS.HS[type][d];

            for (int i = 0; i < toBeShown.highScores.Count; i++)
            {
                visibleNames[i].Text = toBeShown.highScores[i].player;
                visibleTimes[i].Text = (new TimeSpan(toBeShown.highScores[i].time * 10000000)).ToString();
            }
            for (int i = toBeShown.highScores.Count; i < 5; i++)
            {
                visibleNames[i].Text = "";
                visibleTimes[i].Text = "";
            }

        }
        public void submitHighScore(string name, int ticks, gameType type, Difficulty diff)
        {
            HighScoreItem item = new HighScoreItem();
            item.player = name;
            item.time = ticks;
            HS.add(item, type, diff);
            
        }

        // Serialize an ArrayList object to a binary file.
        private static void BinarySerializeScores(Scores HS)
        {
            using (FileStream str = File.Create("C:\\Users\\Natasha\\HighScores.hs"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(str, HS);
            }
        }
        // Deserialize an ArrayList object from a binary file.
        private static Scores BinaryDeserializeScores()
        {
            Scores HS = null;
            try
            {

                using (FileStream str = File.OpenRead("C:\\Users\\Natasha\\HighScores.hs"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    HS = (Scores)bf.Deserialize(str);
                }

                File.Delete("C:\\Users\\Natasha\\HighScores.hs");

                return HS;
            }
            catch(FileNotFoundException f)
            {
                return new Scores();
            }
        }
        private static void BinarySerializeGame(Sudoku game)
        {
            using (FileStream str = File.Create("C:\\Users\\Natasha\\Sudoku.oku"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(str, game);
            }
        }
        // Deserialize an ArrayList object from a binary file.
        private static Sudoku BinaryDeserializeGame()
        {
            Sudoku game = null;
            try
            {

                using (FileStream str = File.OpenRead("C:\\Users\\Natasha\\Sudoku.oku"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    game = (Sudoku)bf.Deserialize(str);
                }

                File.Delete("C:\\Users\\Natasha\\Sudoku.oku");

                return game;
            }
            catch (FileNotFoundException f)
            {
                MessageBox.Show("You don't have any previously saved games.");
                return game;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BinarySerializeScores(HS);
            DialogResult res = MessageBox.Show("\t\tSave game?\n You may overwrite previously saved game.","Save game?",MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Yes)
            {
                if (standard != null)
                {
                    BinarySerializeGame(standard);
                }
                else if(squiggly != null)
                {
                    BinarySerializeGame(squiggly);
                }
            }
            else if (res == DialogResult.No)
            {
            }
            else if (res == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Standard, Difficulty.Easy);
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Standard, Difficulty.Medium);
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Standard, Difficulty.Hard);
        }

        private void easyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Squiggly, Difficulty.Easy);
        }

        private void mediumToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Squiggly, Difficulty.Medium);
        }

        private void hardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Squiggly, Difficulty.Hard);
        }

        private void btnToLoadGame_Click(object sender, EventArgs e)
        {
            Sudoku game = BinaryDeserializeGame();
            if (game == null)
            {
                MessageBox.Show("You don't have any previously saved games.");
            }
            else
            {

            }
        }
    }


}
