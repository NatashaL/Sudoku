using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sudoku
{
    public class SquigglyGenerator
    {
        private SquigglySolver squigglySolver;
        public SquigglyGrid PermaGrid;
        public SquigglyGrid SolutionGrid;
        private Difficulty difficulty;
        public int[,] scheme;

        /// This constructs a squiggly generator class.
        public SquigglyGenerator(int[,] solution, int[,] scheme,Difficulty difficultyIn)
        {
            this.scheme = scheme;
            this.SolutionGrid = new SquigglyGrid();
            this.SolutionGrid.Grid = solution;
            squigglySolver = new SquigglySolver(scheme);
            difficulty = difficultyIn;
        }

        public SquigglyGrid InitGrid()
        {
            //Randomly fill in the first row and column of squigglygrid
            SquigglyGrid tempGrid = new SquigglyGrid { };       //temporary grid to assign values into
            int row = 0;                         		    //variable for navigating 'rows'
            int col = 0;                        			//variable for navigating 'columns'
            int newVal;                        	            //value to place into grid
            //bool solved;
            List<int> valueSet = new List<int>(Enumerable.Range(1, 9));   //range of numbers that can be added to the grid

            List<int> valueSet2 = new List<int>(); 			//placeholder values in column 0
            Random rnd = new Random(); 						//random variable for choosing random number
            int randIndex = 0;       						//index in valueSet/valueSet2 that is accessed
            randIndex = rnd.Next(0, 8); 						//get a random number and place in grid(0,0)
            newVal = valueSet[randIndex];
            tempGrid.InitSetCell(row, col, newVal);
            valueSet.Remove(newVal);                        //remove paced value from options

            for (row = 1; row < 9; row++)
            { 								//fills in column 0 with remaining possible values, storing in place-
                //holder as it goes so as to preserve when placing in row 0 later
                randIndex = rnd.Next(0, valueSet.Count);
                newVal = valueSet[randIndex];
                valueSet2.Add(newVal);
                valueSet.Remove(newVal);
                tempGrid.InitSetCell(row, col, newVal);
            }
            
            //row = 0;                                               //reset row to 0

            for (int i = 8; i >= 0; i--)
            {
                tempGrid.InitSetCell(0, 9 - i, tempGrid.Grid[i, 0]);
            }

            do
            {
                squigglySolver = new SquigglySolver(scheme);
                squigglySolver.SolveGrid((SquigglyGrid)tempGrid.Clone(), false); //Slv to fill remainder of grid
                SolutionGrid = squigglySolver.SolutionGrid;
            } while (SolutionGrid == null || SolutionGrid.IsBlank());

            PermaGrid = Blanker(SolutionGrid);       //call Blanker to carry out the
            return PermaGrid;         //blanking of fileds,then return the grid to user to solve
        }

        //	Call SolveGrid to solve squigglygrid
        //Store solved gamegrid as the correct solution in solutiongrid

        public SquigglyGrid Blanker(SquigglyGrid solvedGrid)
        {                          //enable blanking of squares based on difficulty
            SquigglyGrid tempGrid;
            SquigglyGrid saveCopy;
            //temporary grids to save between tests
            bool unique = true;          //flag for if blanked form has unique soln
            int totalBlanks = 0;	                      //count of current blanks
            int tries = 0;                  //count of tries to blank appropriately
            int desiredBlanks;            //amount of blanks desired via difficulty
            int symmetry = 0;                                       //symmetry type
            tempGrid = (SquigglyGrid)solvedGrid.Clone();
            //cloned input grid (no damage)
            Random rnd = new Random();         //allow for random number generation

            switch (difficulty)           //set desiredBlanks via chosen difficulty
            {
                case Difficulty.Easy: //easy difficulty
                    desiredBlanks = 2;
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

            symmetry = rnd.Next(0, 2);                   //Randomly select symmetry
            do
            {          //call RandomlyBlank() to blank random squares symmetrically
                saveCopy = (SquigglyGrid)tempGrid.Clone();     // in case undo needed
                tempGrid = RandomlyBlank(tempGrid, symmetry, ref totalBlanks);
                //blanks 1 or 2 squares according to symmetry chosen
                squigglySolver = new SquigglySolver(scheme);
                unique = squigglySolver.SolveGrid((SquigglyGrid)tempGrid.Clone(), false);         // will it solve uniquely?
                if (!unique)
                {
                    tempGrid = (SquigglyGrid)saveCopy.Clone();
                    tries++;
                }
            } while ((totalBlanks < desiredBlanks) && (tries < 1000));
            solvedGrid = tempGrid;
            solvedGrid.Finish();
            return solvedGrid;
        }

        public SquigglyGrid RandomlyBlank(SquigglyGrid tempGrid, int sym, ref int blankCount)
        {
            //blank one or two squares(depending on if on center line) randomly
            Random rnd = new Random(); //allow random number generation
            int row = rnd.Next(0, 8); //choose randomly the row
            int column = rnd.Next(0, 8); //and column of cell to blank
            while (tempGrid.Grid[row, column] == 0) //don't blank a blank cell
            {
                row = rnd.Next(0, 8);
                column = rnd.Next(0, 8);
            }
            tempGrid.InitSetCell(row, column, 0); //clear chosen cell
            blankCount++; //increment the count of blanks
            switch (sym)
            {
                //based on symmetry, blank a second cell
                case 0: //vertical symmetry
                    if (tempGrid.Grid[row, 8 - column] != 0) //if not already blanked
                        blankCount++; //increment blank counter
                    tempGrid.InitSetCell(row, 8 - column, 0); //blank opposite cell
                    break;
                case 1: //horizontal symmetry
                    if (tempGrid.Grid[8 - row, column] != 0)
                        blankCount++;
                    tempGrid.InitSetCell(8 - row, column, 0);
                    break;
                case 2: //diagonal symmetry
                    if (tempGrid.Grid[column, row] != 0)
                        blankCount++;
                    tempGrid.InitSetCell(column, row, 0);
                    break;
                default: //diagonal symmetry
                    if (tempGrid.Grid[row, 8 - column] != 0)
                        blankCount++;
                    tempGrid.InitSetCell(column, row, 0);
                    break;
            }
            return tempGrid;
        }

    }
}
