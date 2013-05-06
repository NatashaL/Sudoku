using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    /// <summary>
    /// Puzzle Solver contains the public interface SolveGrid, which takes a 
    /// puzzle grid as input, sets the solution grid if solvable, and returns
    /// a bool. The other methods in this class are helper methods to serve
    /// SolveGrid.
    /// </summary>
    class PuzzleSolver
    {
        public PuzzleGrid SolutionGrid;   //The solution with all values filled
        int[] gridRow;                                //holds a row of the grid
        bool[] list;                                  //List of possible values
        PuzzleGrid[] final;                          //Array of found solutions
        int numSolns;                               //Number of solutions found
        bool stoplooking;                      //Stop trying to find a solution
        int recursions;                       // count the number of recursions
        const int MaxDepth = 1000;   // upper limit on the number of recursions


        public PuzzleSolver()
        {
            //Initialize arrays
            list = new bool[10];             //Naturally indexed for simplicity
            gridRow = new int[9];
            final = new PuzzleGrid[2];
        }
        /// <summary>
        /// IsSolved checks to see if all cells in the grid contain a value.
        /// If so, due to how the solve algorithm solves, the puzzle must be 
        ///  solved correctly.
        /// </summary>
        /// <param name="grid">Current state of the puzzle grid</param>
        /// <returns>TRUE: Puzzle is solved, FALSE: Not solved</returns>
        private bool IsSolved(PuzzleGrid grid)
        {
            bool result = true;                       //Assume puzzle is solved
            int r, c;
            r = 0;
            while (result == true && r < 9)                   //Check every row
            {
                c = 0;
                while (result == true && c < 9)            //Check every column
                {                //If an empty cell is found, result gets FALSE
                    result = (result && grid.Grid[r, c] != 0);
                    c++;
                }
                r++;
            }
            return result;
        }
        /// <summary>
        /// FirstTrue finds the lowest possible value that can be inserted in an
        /// empty cell then returns that value
        /// </summary>
        /// <returns>Integer value of smallest possible cell value</returns>
        private int FirstTrue()
        {
            int i = 1;
            int result = 0;
            while (result == 0 && i < 10)//iter list of possibles until 1 found
            {
                if (list[i] == true) //As soon as one is found, set it and stop
                {
                    result = i;
                }
                i++;
            }
            return result;                       //Return lowest possible value
        }
        /// <summary>
        /// PickOneTrue randomly chooses a value to try. If random number does
        /// not have a value of "true", increment until true value is found.
        /// </summary>
        /// <returns>Integer to insert in puzzle</returns>
        private int PickOneTrue()
        {
            int i;
            int result = 0;
            Random r = new Random();
            if (FirstTrue() != 0)                   //If valid true values exist
            {
                i = r.Next(1, 9); //get value 0 - 8, grid not naturally indexed
                while (result == 0)
                {
                    if (list[i] == true)     //if random number is true, use it
                    {
                        result = i;
                    }
                    else                  //else, increment to first true value
                    {
                        i++;
                        if (i > 9)      //If end of list reached, wrap to front
                        {
                            i = 1;
                        }
                    }
                }
            }
            else
            {
                result = 0;//No valid true values exist, puzzle can't be solved
            }
            return result;
        }
        /// <summary>
        /// IsInRow checks if given value occurs in the given row
        /// </summary>
        /// <param name="grid">Current state of puzzle grid</param>
        /// <param name="row">Row to check</param>
        /// <param name="value">Value to look for</param>
        /// <returns></returns>
        private bool IsInRow(PuzzleGrid grid, int row, int value)
        {
            bool result = false;
            for (int i = 0; i < 9; i++)                   //Iterate through row
            {                          //check if cell holds value being sought
                result = result || (Math.Abs(grid.Grid[row, i]) == value);
            }
            return result;
        }
        /// <summary>
        /// IsInColumn checks if given value occurs in the given row.
        /// </summary>
        /// <param name="grid">Current state of the grid</param>
        /// <param name="col">Column being check</param>
        /// <param name="value">Value being sought</param>
        /// <returns></returns>
        private bool IsInCol(PuzzleGrid grid, int col, int value)
        {
            bool result = false;
            for (int i = 0; i < 9; i++)                //Iterate through column
            {                          //check if cell holds value being sought
                result = result || (Math.Abs(grid.Grid[i, col]) == value);
            }
            return result;
        }
        /// <summary>
        /// GroupNum determines which third of the grid the given row or 
        /// column number is in. R/C 0, 1, 2 appear in third 0; R/C 3, 4, 5
        /// appear in third 1; R/C 6, 7, 8 appear in third 2.
        /// </summary>
        /// <param name="rc">Number of the row or column being checked</param>
        /// <returns>Integer denoting which third (0, 1, or 2)</returns>
        private int GroupNum(int rc)
        {
            //return 0 for rc = 0..2; 1 for rc = 3..5; 2 for rc = 6..9
            int result;
            result = (int)(rc / 3); //Truncate division to whole number value
            return result;
        }
        /// <summary>
        /// IsIn3x3 determines which 3x3 grid cell is in using GroupNum. 
        /// Values are:
        /// [0, 0]  [0, 1]  [0, 2]
        /// [1, 0]  [1, 1]  [1, 2]
        /// [2, 0]  [2, 1]  [2, 2]
        /// And then checks if given value occurs in the 3x3 grid.
        /// </summary>
        /// <param name="g">Current state of the grid</param>
        /// <param name="row">Row of cell being checked</param>
        /// <param name="col">Column of cell being checked</param>
        /// <param name="value">Value being sought</param>
        /// <returns>True if Value is found, false if not</returns>
        private bool IsIn3X3(PuzzleGrid g, int row, int col, int value)
        {
            int rLow;
            int cLow;
            rLow = 3 * GroupNum(row);    //Index of smallest number row in grid
            cLow = 3 * GroupNum(col);//Index of smallest number columin in grid
            bool result = false;
            for (int i = rLow; i < rLow + 3; i++) //Check all 3 rows in subgrid
            {
                for (int j = cLow; j < cLow + 3; j++)     //Check all 3 columns
                {               //Compare value of cell with value being sought
                    if (Math.Abs(g.Grid[i, j]) == value)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// IsPossible returns true if IsInRow, IsInCol & IsIn3x3 return false
        /// </summary>
        /// <param name="g">Current state of the grid</param>
        /// <param name="row">row of target cell</param>
        /// <param name="col">column of target cell</param>
        /// <param name="value">value being sought</param>
        /// <returns>True if value can occupy cell at [row, col]</returns>
        private bool IsPossible(PuzzleGrid g, int row, int col, int value)
        {                     //Return true if value can go into [row, col] now
            bool result;
            result = (!IsInRow(g, row, value) && !IsInCol(g, col, value) &&
                !IsIn3X3(g, row, col, value));
            return result;
        }
        /// <summary>
        /// ListPossible populates list[] with "true" at each index
        /// representing that value is a possible value for cell at [row, col].
        /// It returns the total count of possible values in that square
        /// </summary>
        /// <param name="row">Row of target cell</param>
        /// <param name="col">Column of target cell</param>
        /// <param name="g">current state of grid</param>
        /// <returns>Integer count of possible values for given cell</returns>
        private int ListPossible(int row, int col, PuzzleGrid g)
        {
            int count = 0;
            ClearList();              //Create a fresh list for bool population
            for (int i = 1; i < 10; i++)          //For i = 1..9 (valid values)
            {
                if (IsPossible(g, row, col, i) == true)   //If i can go in cell
                {
                    list[i] = true;
                    count++;
                }
                else                     //Value of i found in Row, Col, or 3x3
                {
                    list[i] = false;
                }
            }
            return count;
        }
        /// <summary>
        /// FillSingleChoices iterates through empty cells. For any empty cell
        /// with only one possible value, it fills in that value.
        /// </summary>
        /// <param name="grid">Current state of grid</param>
        private void FillSingleChoices(PuzzleGrid grid)
        {
            bool anyChanges = false;                    //Set if cell value set
            int numChoices;                           //Number of choices found
            do            //While changes have been made AND grid is not solved
            {
                anyChanges = false;
                for (int i = 0; i < 9; i++)         //Iterate through every row
                {
                    for (int j = 0; j < 9; j++)  //Iterate through every column
                    {
                        if (grid.Grid[i, j] == 0)
                        {       //Get number of choices available in grid[i, j]
                            numChoices = ListPossible(i, j, grid);
                            if (numChoices == 1) //If only one choice set value
                            {
                                grid.UserSetCell(i, j, FirstTrue());
                                //Changes made, anyChanges = true
                                anyChanges = (grid.Grid[i, j] != 0);
                            }
                        }
                    }
                }
            } while (anyChanges == true && !IsSolved(grid));
        }
        /// <summary>
        /// FindFewestChoices finds the first cell having the smallest number
        /// of available choices, and sets the row and column of that cell for
        /// use in SolveGrid
        /// </summary>
        /// <param name="grid">Current state of the grid</param>
        /// <param name="r">OUTPUT sets r for use in caller</param>
        /// <param name="c">OUTPUT sets c for use in caller</param>
        /// <param name="numChoices">OUTPUT sets var for use in caller</param>
        /// <returns>Returns true if valid cell found, false if not</returns>
        private bool FindFewestChoices(PuzzleGrid grid, out int r, out int c,
            out int numChoices)
        {
            bool[] minList = new bool[10];
            int numCh, minR, minC, minChoice, i, j;
            bool bad, result;
            minChoice = 10;
            minR = 0;
            minC = 0;
            for (i = 1; i < 10; i++)              //Initialize minList to FALSE
            {
                minList[i] = false;
            }
            bad = false;
            i = 0;
            while (!bad && i < 9) //While not a bad solutn and trying valid row
            {
                j = 0;
                while (!bad && j < 9) //not a bad solutn and trying valid column
                {
                    if (grid.Grid[i, j] == 0)
                    {
                        numCh = ListPossible(i, j, grid);    //Get # of choices
                        if (numCh == 0)     //If no choices found, bad solution
                        {
                            bad = true;
                        }
                        else                             //If not bad solutn...
                        {
                            if (numCh < minChoice)   //If less than current min
                            {
                                minChoice = numCh;          //Set new min value
                                list.CopyTo(minList, 0);//Save list of possible
                                minR = i;          //set row of cell with least
                                minC = j;          //set col of cell with least
                            }
                        }
                    }
                    j++;
                }
                i++;
            }
            if (bad || minChoice == 10)  //If bad solutn or minChoice never set
            {
                result = false;                    //No fewest possible choices
                r = 0;
                c = 0;
                numChoices = 0;
            }
            else
            {
                result = true; //Valid cell found, return information to caller
                r = minR;
                c = minC;
                numChoices = minChoice;
                minList.CopyTo(list, 0);
            }
            return result;
        }
        /// <summary>
        /// ClearList clears the values currently held in list[] by setting all
        /// values to false.
        /// </summary>
        private void ClearList()
        {
            for (int i = 0; i < 10; i++)
            {
                list[i] = false;
            }
        }
        /// <summary>
        /// SolveGrid attempts to solve a puzzle by checking through all
        /// possible values for each cell, discarding values that do not lead
        /// to a valid solution. On a try, it recursively calls itself to
        /// maintain previous states of the grid to back track to if the
        /// current path fails. It creates a local version of the grid to
        /// facilitate this. It also checks if the puzzle is uniquely solvable.
        /// </summary>
        /// <param name="g">Current state of the grid</param>
        /// <param name="checkUnique">Do we care if it has unique soln?</param>
        /// <returns></returns>
        public bool SolveGrid(PuzzleGrid g, bool checkUnique)
        {
            PuzzleGrid grid = new PuzzleGrid();
            grid = (PuzzleGrid)g.Clone();                 //Copy the input grid
            int i, choice, r, c, numChoices;
            bool done, got_one, solved, result;
            got_one = false;
            recursions++;
            FillSingleChoices(grid);  //First, fill in all single choice values
            if (IsSolved(grid))                        //If it's already solved
            {
                if (numSolns > 0)               //If another soln already found
                {
                    stoplooking = true;                   //Don't look for more
                    result = false;              //Return false, no UNIQUE soln
                }
                else                               //If no other soln found yet
                {
                    numSolns++;
                    final[numSolns] = (PuzzleGrid)g.Clone();  //Save found soln
                    result = true;
                    SolutionGrid = grid;
                }
            }
            else                                            //If not solved yet
            {
                if (!FindFewestChoices(grid, out r, out c, out numChoices))
                {
                    result = false;                          //Invalid solution
                }
                else                                 //Current grid still valid
                {
                    i = 1;
                    done = false;
                    got_one = false;
                    while (!done && i <= numChoices)
                    {
                        choice = PickOneTrue();         //Pick a possible value
                        list[choice] = false;      //Won't want to use it again
                        grid.UserSetCell(r, c, choice);

                        if (recursions < MaxDepth)
                        {
                            //-----------We must go deeper. SUDCEPTION!-----------//
                            solved = (SolveGrid(grid, checkUnique)); //Recurse
                        }
                        else
                        {
                            solved = false;
                        }
                        if (stoplooking == true)
                        {
                            done = true;
                            got_one = true;
                        }
                        else
                        {
                            got_one = (got_one || solved);
                            if (!checkUnique)  //If not looking for unique soln
                            {
                                done = got_one;       //Then we have a solution
                            }
                        }
                        i++;
                    }
                    result = got_one;
                }
            }
            return result;
        }
    }//End CLASS
}