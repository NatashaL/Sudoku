using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sudoku
{
    public class Standard : Sudoku
    {
        public int[,] blanked;
        //public int[,] solution;
        //public int[,] rows;
        //public int[,] cols;
        //public int[,] groups;
        //public Difficulty diff;
        public Random random = new Random();
        public int[,] getGroup;

        public Standard(Difficulty diff) : base (diff)
        {
            String rez = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    rez += base.solution[i, j]+" ";
                }
                rez += "\n";
            }
            MessageBox.Show(rez);
            solve();
            mask();
        }
        override public int[,] solve()
        {
            PuzzleSolver solver = new PuzzleSolver();
            do
            {
                PuzzleGrid puzzlegrid = new PuzzleGrid();
                puzzlegrid.Grid = (int[,])base.solution.Clone();
                solver.SolveGrid(puzzlegrid, true);
            }
            while (solver.SolutionGrid == null);

            return solver.SolutionGrid.Grid;
        }
        public int[,] mask()
        {                          //enable blanking of squares based on difficulty
            int[,] solvedGrid = solution;
            int[,] tempGrid;
            int[,] saveCopy;
            //temporary grids to save between tests
            bool unique = true;          //flag for if blanked form has unique soln
            int totalBlanks = 0;	                      //count of current blanks
            int tries = 0;                  //count of tries to blank appropriately
            int desiredBlanks;            //amount of blanks desired via difficulty
            int symmetry = 0;                                       //symmetry type
            tempGrid = (int[,])solvedGrid.Clone();
            //cloned input grid (no damage)
            //Random rnd = new Random();         //allow for random number generation

            switch (diff)           //set desiredBlanks via chosen difficulty
            {
                case Difficulty.Easy: //easy difficulty
                    desiredBlanks = 40;
                    break;
                case Difficulty.Medium: //medium difficulty
                    desiredBlanks = 50;
                    break;
                case Difficulty.Hard: //hard difficulty
                    desiredBlanks = 60;
                    break;
                default: //easy difficulty
                    desiredBlanks = 40;
                    break;
            }

            symmetry = random.Next(0, 2);                   //Randomly select symmetry
            do
            {          //call RandomlyBlank() to blank random squares symmetrically
                saveCopy = (int[,])tempGrid.Clone();     // in case undo needed
                tempGrid = RandomlyBlank(tempGrid, symmetry, ref totalBlanks);
                //blanks 1 or 2 squares according to symmetry chosen
                Solver puzzleSolver = new Solver(tempGrid);
                unique = puzzleSolver.SolveGrid(true);         // will it solve uniquely?
                if (!unique)
                {
                    tempGrid = (int[,])saveCopy.Clone();
                    tries++;
                }
            } while ((totalBlanks < desiredBlanks) && (tries < 1000));
            solvedGrid = tempGrid;
            blanked = (int[,])solvedGrid.Clone();
            return solvedGrid;
        }
        public int[,] RandomlyBlank(int[,] tempGrid, int sym, ref int blankCount)
        {
            //blank one or two squares(depending on if on center line) randomly
            Random rnd = new Random(); //allow random number generation
            int row = rnd.Next(0, 1000)%137 %9; //choose randomly the row
            int column = rnd.Next(0, 100000) % 137 % 9; //and column of cell to blank
            while (tempGrid[row, column] == 0) //don't blank a blank cell
            {
                row = rnd.Next(0, 8);
                column = rnd.Next(0, 8);
            }
            tempGrid[row, column] = 0; //clear chosen cell
            blankCount++; //increment the count of blanks
            switch (sym)
            {
                //based on symmetry, blank a second cell
                case 0: //vertical symmetry
                    if (tempGrid[row, 8 - column] != 0) //if not already blanked
                        blankCount++; //increment blank counter
                    tempGrid[row, 8 - column] =  0; //blank opposite cell
                    break;
                case 1: //horizontal symmetry
                    if (tempGrid[8 - row, column] != 0)
                        blankCount++;
                    tempGrid[8 - row, column] = 0;
                    break;
                case 2: //diagonal symmetry
                    if (tempGrid[column, row] != 0)
                        blankCount++;
                    tempGrid[column, row] = 0;
                    break;
                default: //diagonal symmetry
                    if (tempGrid[row, 8 - column] != 0)
                        blankCount++;
                    tempGrid[column, row] = 0;
                    break;
            }
            return tempGrid;
        }
    }
}
