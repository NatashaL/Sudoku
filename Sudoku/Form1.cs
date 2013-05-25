using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public bool SaveScoreHasAppeared = false;
        public Form1()
        {
            InitializeComponent();
            this.Text = "Sudoku";
            schemeBuilder();
            HS = BinaryDeserializeScores();
            setHighScoresPanel(gameType.Standard,Difficulty.Easy);
        }
        /// <summary>
        /// Sets the look of the dataGridView for the Squiggly Sudoku.
        /// Makes sure there are no dividers between rows and/or columns.
        /// Makes sure the cells have the same witdh and height.
        /// </summary>
        public void setSquigglyTableView()
        {
            dataGridView.RowCount = 9;
            dataGridView.Width = 228;
            dataGridView.Height = 228;
            dataGridView.Rows[2].DividerHeight = 0;
            dataGridView.Rows[5].DividerHeight = 0;
            dataGridView.Columns[2].DividerWidth = 0;
            dataGridView.Columns[5].DividerWidth = 0;
            dataGridView.Columns[2].Width = 25;
            dataGridView.Columns[5].Width = 25;
            dataGridView.Rows[2].Height = 25;
            dataGridView.Rows[5].Height = 25;
            for (int i = 0; i < 9; i++)
            {
                dataGridView.Rows[i].Height = 25;
            }
        }
        /// <summary>
        /// Sets the look of the dataGridView for the Standard Sudoku.
        /// Makes sure there ARE dividers between some rows and/or some columns.
        /// Makes sure the cells have the same witdh and height.
        /// </summary>
        public void setStandardTableView()
        {
            dataGridView.RowCount = 9;
            dataGridView.Rows[2].DividerHeight = 3;
            dataGridView.Rows[5].DividerHeight = 3;
            for (int i = 0; i < 9; i++)
            {
                dataGridView.Rows[i].Height = 25;
            }
            dataGridView.Rows[2].Height = 26;
            dataGridView.Rows[5].Height = 26;
            dataGridView.Columns[2].DividerWidth = 3;
            dataGridView.Columns[5].DividerWidth = 3;
            dataGridView.Columns[2].Width = 26;
            dataGridView.Columns[5].Width = 26;
        }

        /// <summary>
        /// Takes care of which panel is to be shown to the player.
        /// </summary>
        /// <param name="view"></param>
        public void changeView(int view)
        {
            if (view == 1) // Main Game Panel is Visible
            {
                this.Text = "Sudoku";
                startPanel.Visible = false;
                highScorePanel.Visible = false;
                mainGamePanel.Visible = true;
            }
            else if (view == 2) //High Scores Panel is Visible
            {
                this.Text = "Sudoku - HighScores";
                startPanel.Visible = false;
                mainGamePanel.Visible = false;
                highScorePanel.Visible = true;
            }
            else if (view == 3) // Start Panel is Visible
            {
                this.Text = "Sudoku";
                highScorePanel.Visible = false;
                mainGamePanel.Visible = false;
                startPanel.Visible = true;
            }
            else
            {
                MessageBox.Show("Ova uste ne e implementirano");
            }
        }
        /// <summary>
        /// Takes the value from the currently selected cell
        /// and highlights every occurrence of the same number in the grid
        /// </summary>
        public void highlightSelectedNumber()
        {
            var selected = dataGridView.SelectedCells[0];
            int value = -1;


            if (int.TryParse(selected.Value.ToString(), out value) && value != 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        int val = -1;

                        if (int.TryParse(dataGridView.Rows[i].Cells[j].Value.ToString(), out val) && val == value)
                        {
                            dataGridView.Rows[i].Cells[j].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            dataGridView.Rows[i].Cells[j].Style.BackColor = ColorMap[i,j];
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Clears the highlighted cells.
        /// This is used when the newly selected cell has a different value from the previous or
        /// when the game is paused.
        /// </summary>
        public void clearHighlight()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView.Rows[i].Cells[j].Style.BackColor = Color.White;
                }
            }
        }
        /// <summary>
        /// Calls highlightSelectedNumber() when the grid is still to be solved.
        /// If it is solved, this feature (highlighting the cells) is disabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (standard == null && squiggly == null) return;
            else highlightSelectedNumber();
        }
        /// <summary>
        /// Takes the selected values for gameType and Difficulty.
        /// Creates a new game with these parameters,
        /// sets the grid and starts the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            SaveScoreHasAppeared = false;
            standard = null;
            squiggly = null;
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
            dataGridView.Focus();
            dataGridView.ClearSelection();
            clearHighlight();

            setGrid(type, level);
            
            ticks = 0;
            timer.Start();
        }
        /// <summary>
        /// From the high scores panel, it returns the player to the StartPanel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainMenuBack_Click(object sender, EventArgs e)
        {
            changeView(3);
        }
        /// <summary>
        /// From the StartPanel, it takes the player to the highScores panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnScores_Click(object sender, EventArgs e)
        {
            changeView(2);
        }
        /// <summary>
        /// From the MainGamePanel, it returns the player to the StartPanel.
        /// It asks the player whether to save the current game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainGameBack_Click(object sender, EventArgs e)
        {
            if (SaveScoreHasAppeared == true)
            {
                unPauseGrid();
                timer.Stop();
                timerlabel.Text = (new TimeSpan(0)).ToString();
                changeView(3);
                return;
            }
            if (saveGame() == 0)
            {
                unPauseGrid();
                timer.Start();
                return;
            }
            unPauseGrid();
            timer.Stop();
            timerlabel.Text = (new TimeSpan(0)).ToString();
            changeView(3);
        }
        /// <summary>
        /// MARTIN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown -= dataGridView_KeyDown;
                tb.KeyDown += new KeyEventHandler(dataGridView_KeyDown);
            }
        }
        /// <summary>
        /// Takes action according to the keyboard button pressed.
        ///  - If it is a number from numpad or from the main keyboard, 
        ///         it writes it down or overwrites the current number in that field.
        ///  - If it is up/down/left/right/tab/enter, 
        ///         it selects the respective cell.
        ///  - If it is escape/backspace or delete, 
        ///         it deletes the value in the cell.
        ///  - Else, it doesn't do anything.
        ///  - It checks if the grid has been solved,
        ///         and prompts a window according to the result.
        ///  - If the grid is full but it isn't a valid solution to the game,
        ///         it doesn't do anything.
        ///  - If the grid is solved but the time does not make it in the top 5, 
        ///         it prompts a MessageBox
        ///  - If the grid is solved AND the time makes it in the top 5, 
        ///         it prompts a new form, that asks for the player's name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (standard == null && squiggly == null)
            {
                return;
            }
            if (dataGridView.SelectedCells.Count > 0)
            {
                var selected = dataGridView.SelectedCells[0];
                int sel_i = selected.RowIndex;
                int sel_j = selected.ColumnIndex;
                if (CellMap[sel_i, sel_j] == LOCKED) return;
                if (!(e.KeyValue >= 49 && e.KeyValue <= 57 || e.KeyValue >= 97 && e.KeyValue <= 105))
                {
                    if (e.KeyValue == 27 || e.KeyValue == 8 || e.KeyValue == 46)   // Same as e.KeyCode == Keys.Enter  etc.
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
                System.Media.SoundPlayer finishSoundPlayer = new System.Media.SoundPlayer(@"C:\Windows\Media\tada.wav");
                System.Media.SoundPlayer finish_no_high_SoundPlayer = new System.Media.SoundPlayer(@"C:\Windows\Media\chimes.wav");
                gameType type = standard == null ? gameType.Squiggly : gameType.Standard;
                Difficulty level = standard == null ? squiggly.diff : standard.diff;
                int d = level == Difficulty.Easy ? 0 : (level == Difficulty.Medium ? 1 : 2);
                SaveScoreHasAppeared = true;
                if (HS.HS[type][d].highScores.Count == 5 && ticks > HS.HS[type][d].highScores[4].time)
                {
                    finish_no_high_SoundPlayer.Play();
                    MessageBox.Show("\t\tCongratulations!!! \n I'm sorry but you didn't make it in the top 5.");
                }
                else
                {
                    finishSoundPlayer.Play();
                    Form2 form2 = new Form2(this, ticks, type, level);
                    form2.Show();
                    form2.name.Focus();
                }
                standard = null;
                squiggly = null;
            }

        }
        /// <summary>
        /// It is used when the grid has been solved.
        /// </summary>
        public void DarkenGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for(int j=0; j<9;j++)
                {
                    dataGridView.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                    dataGridView.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightGray;
                    dataGridView.Rows[i].Cells[j].Style.SelectionForeColor = Color.Black;
                }
            }
        }
        /// <summary>
        /// It is used to lock the values in the grid that are given from the start.
        /// It makes sure the player cannot change them.
        /// </summary>
        public void LockCellMap()
        {
            CellMap = new int[9, 9];
            CellMap.Initialize();


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (dataGridView.Rows[i].Cells[j].Value != "")
                    {
                        CellMap[i, j] = LOCKED;
                        dataGridView.Rows[i].Cells[j].Style.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml("#FFA1A1");
                    }
                    else
                    {
                        dataGridView.Rows[i].Cells[j].Style.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml("#61D465");
                    }
                }
            }
        }
        /// <summary>
        /// This is used to create a new game object, according to the given parameters,
        /// to fill the dataGridView with the starting values,
        /// and to update the text shown on the form.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        public void setGrid(gameType type, Difficulty level)
        {
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView.Rows[i].Cells[j].Value = "";
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
                            dataGridView.Rows[i].Cells[j].Value = standard.mask[i, j];
                        ColorMap[i, j] = Color.White;
                    }
                }
                this.Text += " - Standard " + (standard.diff == Difficulty.Easy ? "(easy) " : (standard.diff == Difficulty.Medium ? "(medium)" : "(hard)"));
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
                            dataGridView.Rows[i].Cells[j].Value = squiggly.mask[i, j];
                        dataGridView.Rows[i].Cells[j].Style.BackColor = colors[squiggly.scheme[i, j]];
                        ColorMap[i, j] = colors[squiggly.scheme[i, j]];
                    }
                }
                this.Text += " - Squiggly " + (squiggly.diff == Difficulty.Easy ? "(easy) " : (squiggly.diff == Difficulty.Medium ? "(medium)" : "(hard)"));
            }
            LockCellMap();
        }
        /// <summary>
        /// This is used to timeout the thread used to create a new Squiggly object.
        /// This function is needed, because not always the starting grid (as randomly filled with values)
        /// can have a solution. If a solution is not found in less than 4 seconds, it start all over again.
        /// </summary>
        /// <typeparam name="CustomSquiggly"></typeparam>
        /// <param name="F">The function that might need be called multiple times.</param>
        /// <param name="Timeout">The timespan given to one thread, in miliseconds.</param>
        /// <param name="Completed">Out parameter, used to check if a solution has been found.</param>
        /// <returns></returns>
        public static CustomSquiggly Limex<CustomSquiggly>(Func<CustomSquiggly> F, int Timeout, out bool Completed)
        {
            CustomSquiggly result = default(CustomSquiggly);
            Thread thread = new Thread(() => result = F());
            thread.Start();
            Completed = thread.Join(Timeout);
            if (!Completed) thread.Abort();
            return result;
        }
        /// <summary>
        /// It is called every second,
        /// it increases the number of seconds(ticks) for the current game
        /// and it updates the label that shows the player how much time has passed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            ticks++;
            timerlabel.Text = (new TimeSpan(ticks * 10000000)).ToString();

        }
        /// <summary>
        /// It fills the highScorePanel with the highScore lists according to the input parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="diff"></param>
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
        /// <summary>
        /// This is used to generate a new HighScoreItem object, that needs to be added to the highScores lists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ticks"></param>
        /// <param name="type"></param>
        /// <param name="diff"></param>
        public void submitHighScore(string name, int ticks, gameType type, Difficulty diff)
        {
            HighScoreItem item = new HighScoreItem();
            item.player = name;
            item.time = ticks;
            HS.add(item, type, diff);
            
        }

        /// <summary>
        /// Serialize a Scores object to a binary file.
        /// </summary>
        /// <param name="HS">T object that needs to be serialized.</param>
        private static void BinarySerializeScores(Scores HS)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            using (FileStream str = File.Create(path+"\\HighScores.hs"))
            {
                File.SetAttributes(path + "\\HighScores.hs", File.GetAttributes(path + "\\HighScores.hs") | FileAttributes.Hidden);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(str, HS);
            }
        }
        /// <summary>
        /// Deserialize a Scores object from a binary file.
        /// </summary>
        /// <returns>Scores()</returns>
        private static Scores BinaryDeserializeScores()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Scores HS = null;
            try
            {

                using (FileStream str = File.OpenRead(path+"\\HighScores.hs"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    HS = (Scores)bf.Deserialize(str);
                }

                File.Delete(path+"\\HighScores.hs");

                return HS;
            }
            catch(FileNotFoundException)
            {
                return new Scores();
            }
        }
        /// <summary>
        /// Serialize a Sudoku object to a binary file.
        /// </summary>
        /// <param name="game">Game that needs to be serialized</param>
        private static void BinarySerializeGame(Sudoku game)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            using (FileStream str = File.Create(path+"\\Sudoku.oku"))
            {
                File.SetAttributes(path + "\\Sudoku.oku", File.GetAttributes(path + "\\Sudoku.oku") | FileAttributes.Hidden);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(str, game);
            }
        }
        /// <summary>
        /// Deserialize a Sudoku object from a binary file.
        /// </summary>
        /// <returns>Saved game</returns>
        private static Sudoku BinaryDeserializeGame()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Sudoku game = null;
            try
            {
                using (FileStream str = File.OpenRead(path+"\\Sudoku.oku"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    game = (Sudoku)bf.Deserialize(str);
                }
                File.Delete("C:\\Users\\Sudoku.oku");
                return game;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("You don't have any previously saved games.");
                return game;
            }
        }
        /// <summary>
        /// It is called on the click of the 'X' button on the form.
        /// It asks the player whether to save the current game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BinarySerializeScores(HS);
            //if (!mainGamePanel.Visible || (standard == null && squiggly == null)) return;
            if (!mainGamePanel.Visible || SaveScoreHasAppeared==true) return;
            if (saveGame() == 0)
            {
                unPauseGrid();
                e.Cancel = true;
                timer.Start();
                return;
            }
                
            unPauseGrid();
            timer.Stop();
            timerlabel.Text = (new TimeSpan(0)).ToString();
            
        }
        /// <summary>
        /// It is called whenever the 'X' button on the form or the 'Back' button on the MainGamePanel
        /// has been clicked.
        /// It asks the player whether to save the current game.
        /// </summary>
        /// <returns></returns>
        private int saveGame()
        {
            timer.Stop();
            pauseGrid();
            DialogResult res = MessageBox.Show("Would you like to save this game and resume at a later time?\nThis will overwrite previously saved games.", "Save game?", MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Yes)
            {
                if (standard != null)
                {
                    BinarySerializeGame(standard);
                }
                else if (squiggly != null)
                {
                    BinarySerializeGame(squiggly);
                }
                return 2;
            }
            else if (res == DialogResult.No)
            {
                return 1;
            }
            else if (res == DialogResult.Cancel)
            {
                return 0;
            }
            else return -1;
        }
        /// <summary>
        /// It is used to display the highscores for Standard/Easy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Standard, Difficulty.Easy);
        }
        /// <summary>
        /// It is used to display the highscores for Standard/Medium.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Standard, Difficulty.Medium);
        }
        /// <summary>
        /// It is used to display the highscores for Standard/Hard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Standard, Difficulty.Hard);
        }
        /// <summary>
        /// It is used to display the highscores for Squiggly/Easy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void easyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Squiggly, Difficulty.Easy);
        }
        /// <summary>
        /// It is used to display the highscores for Squiggly/Medium.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Squiggly, Difficulty.Medium);
        }
        /// <summary>
        /// It is used to display the highscores for Squiggly/Hard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setHighScoresPanel(gameType.Squiggly, Difficulty.Hard);
        }
        /// <summary>
        /// It is used to call deserialization of a previously saved game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToLoadGame_Click(object sender, EventArgs e)
        {
            Sudoku game = BinaryDeserializeGame();
            if (game == null)
            {
                //MessageBox.Show("Game e null");
                return;
            }
            else
            {
                game = BinaryDeserializeGame();

                if (game is Standard)
                {
                    standard = (Standard)game;
                    setStandardTableView();
                }
                else
                {
                    squiggly = (Squiggly)game;
                    setSquigglyTableView();
                }
                timerlabel.Text = (new TimeSpan(0)).ToString();
                setGrid(gameType.Standard,Difficulty.Easy); //can have any combination of parameters.
                changeView(1);
                timer.Start();
            }
        }
        /// <summary>
        /// It is used to mask the grid, whenever a dialog is open and the timer is stopped.
        /// </summary>
        private void pauseGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView.Rows[i].Cells[j].Style.ForeColor = Color.Turquoise;
                    dataGridView.Rows[i].Cells[j].Value = "X";
                    dataGridView.Rows[i].Cells[j].Selected = false;
                }
            }
        }
        /// <summary>
        /// It is used to unmask the grid when the dialog has closed.
        /// </summary>
        private void unPauseGrid()
        {
            Sudoku game = standard == null ? (Sudoku)squiggly : (Sudoku)standard;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    dataGridView.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                    try
                    {
                        if (game.userGrid[i, j] != 0)
                            dataGridView.Rows[i].Cells[j].Value = game.userGrid[i, j];
                        else
                            dataGridView.Rows[i].Cells[j].Value = "";
                    }
                    catch { }
                }
            }
        }
        /// <summary>
        /// It is used to clear all High Scores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearHS_Click(object sender, EventArgs e)
        {
            DialogResult clear_all=MessageBox.Show("Are you absolutely sure you want to clear ALL high scores?","ATTENTION!",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (clear_all == DialogResult.Yes)
            {
                HS = new Scores();
                setHighScoresPanel(gameType.Standard, Difficulty.Easy);
            }
        }

        private void btnClearHS_MouseHover(object sender, EventArgs e)
        {
            btnClearHS.ForeColor = Color.FromArgb(255, 0, 0, 0);
        }

        private void btnClearHS_MouseLeave(object sender, EventArgs e)
        {
            btnClearHS.ForeColor = Color.FromArgb(255, 32, 32, 32);
        }
    }
}
