using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Solver
    {
        public int[,] SolutionGrid;
        List<int> list;                                  //List of possible values
        int[][,] final;                          //Array of found solutions
        int numSolns;                               //Number of solutions found
        bool stoplooking;                      //Stop trying to find a solution
        int recursions;                       // count the number of recursions
        const int MaxDepth = 1000;   // upper limit on the number of recursions
        public int[,] grid;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;



        public Solver(int[,] grid)
        {
            this.grid = grid;
            SolutionGrid = new int[9, 9];
            rows = new int[9, 10];
            cols = new int[9, 10];
            groups = new int[9, 10];

            list = new List<int>();
            final = new int[2][,];
            for (int i = 0; i < 2; i++)
            {
                final[i] = new int[9, 9];
            }
        }
        /// <summary>
        /// Checks to see if the grid is solved / solved correctly.
        /// </summary>\
        /// <returns>TRUE: Puzzle is solved, FALSE: Not solved</returns>
        public bool IsSolved()
        {
            for (int val = 1; val <= 9; val++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (rows[i, val] != 1 || cols[i, val] != 1 || groups[i, val] != 1)
                    {
                        //MessageBox.Show("rows [" + i + "," + val + "] = " + rows[i, val] + "  cols[" + i + "," + val + "] = " + cols[i, val] + " groups [" + i + "," + val + "] = " + groups[i, val]);
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// FirstTrue finds the lowest possible value that can be inserted in an
        /// empty cell then returns that value
        /// </summary>
        /// <returns>Integer value of smallest possible cell value</returns>
        public int FirstTrue()
        {
            if (list.Count > 0)
                return list[0];
            else
                return 0;
        }
        /// <summary>
        /// PickOneTrue randomly chooses a value to try. If random number does
        /// not have a value of "true", increment until true value is found.
        /// </summary>
        /// <returns>Integer to insert in puzzle</returns>
        public int PickOneTrue()
        {
            if (list.Count > 0)
            {
                Random r = new Random();
                return list[r.Next(list.Count)];
            }
            else return 0;


        }
        /// <summary>
        /// IsInRow checks if given value occurs in the given row
        /// </summary>
        /// <param name="row">Row to check</param>
        /// <param name="value">Value to look for</param>
        /// <returns></returns>
        public bool IsInRow(int row, int value)
        {
            return rows[row, value] > 0;
        }

        /// <summary>
        /// IsInColumn checks if given value occurs in the given row.
        /// </summary>
        /// <param name="col">Column being check</param>
        /// <param name="value">Value being sought</param>
        /// <returns></returns>
        public bool IsInCol(int col, int value)
        {
            return cols[col, value] > 0;
        }
        /// <summary>
        /// GroupNum determines which third of the grid the given row or 
        /// column number is in. R/C 0, 1, 2 appear in third 0; R/C 3, 4, 5
        /// appear in third 1; R/C 6, 7, 8 appear in third 2.
        /// </summary>
        /// <param name="rc">Number of the row or column being checked</param>
        /// <returns>Integer denoting which third (0, 1, or 2)</returns>
        public int GroupNum(int rc)
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
        /// <param name="row">Row of cell being checked</param>
        /// <param name="col">Column of cell being checked</param>
        /// <param name="value">Value being sought</param>
        /// <returns>True if Value is found, false if not</returns>
        public bool IsIn3X3(int row, int col, int value)
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
                    if (Math.Abs(grid[i, j]) == value)
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
        /// <param name="row">row of target cell</param>
        /// <param name="col">column of target cell</param>
        /// <param name="value">value being sought</param>
        /// <returns>True if value can occupy cell at [row, col]</returns>
        public bool IsPossible(int row, int col, int value)
        {
            return !IsInRow(row, value) && !IsInCol(col, value) && !IsIn3X3(row, col, value);
        }
        /// <summary>
        /// ListPossible populates list[] with "true" at each index
        /// representing that value is a possible value for cell at [row, col].
        /// It returns the total count of possible values in that square
        /// </summary>
        /// <param name="row">Row of target cell</param>
        /// <param name="col">Column of target cell</param>
        /// <returns>Integer count of possible values for given cell</returns>
        public int ListPossible(int row, int col)
        {
            list.Clear();
            for (int i = 1; i < 10; i++)          //For i = 1..9 (valid values)
            {
                if (IsPossible(row, col, i))   //If i can go in cell
                {
                    list.Add(i);
                }
            }
            return list.Count;
        }
        /// <summary>
        /// FillSingleChoices iterates through empty cells. For any empty cell
        /// with only one possible value, it fills in that value.
        /// </summary>
        /// <param name="grid">Current state of grid</param>
        public void FillSingleChoices()
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
                        if (grid[i, j] == 0)
                        {       //Get number of choices available in grid[i, j]
                            numChoices = ListPossible(i, j);
                            if (numChoices == 1) //If only one choice set value
                            {
                                grid[i, j] = FirstTrue();
                                //Changes made, anyChanges = true
                                anyChanges = (grid[i, j] != 0);
                            }
                        }
                    }
                }
            } while (anyChanges == true && !IsSolved());
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
        public bool FindFewestChoices(out int r, out int c, out int numChoices)
        {
            List<int> minList = new List<int>();
            int numCh, minR, minC, minChoice, i, j;
            bool bad, result;
            minChoice = 10;
            minR = 0;
            minC = 0;
            bad = false;
            i = 0;
            while (!bad && i < 9) //While not a bad solutn and trying valid row
            {
                j = 0;
                while (!bad && j < 9) //not a bad solutn and trying valid column
                {
                    if (grid[i, j] == 0)
                    {
                        numCh = ListPossible(i, j);    //Get # of choices
                        if (numCh == 0)     //If no choices found, bad solution
                        {
                            bad = true;
                        }
                        else                             //If not bad solutn...
                        {
                            if (numCh < minChoice)   //If less than current min
                            {
                                minChoice = numCh;          //Set new min value

                                //Save list of possible
                                foreach (int ii in list)
                                {
                                    minList.Add(ii);
                                }
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
                list.Clear();
                foreach (int ii in minList)
                {
                    list.Add(ii);
                }
            }
            return result;
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
        public bool SolveGrid(bool checkUnique)
        {
            int i, choice, r, c, numChoices;
            bool done, got_one, solved, result;
            got_one = false;
            recursions++;
            FillSingleChoices();  //First, fill in all single choice values
            if (IsSolved())                        //If it's already solved
            {
                if (numSolns > 0)               //If another soln already found
                {
                    stoplooking = true;                   //Don't look for more
                    result = false;              //Return false, no UNIQUE soln
                }
                else                               //If no other soln found yet
                {
                    numSolns++;
                    final[numSolns] = (int[,])grid.Clone();  //Save found soln
                    result = true;
                    SolutionGrid = grid;
                }
            }
            else                                            //If not solved yet
            {
                if (!FindFewestChoices(out r, out c, out numChoices))
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
                        list.Remove(choice);    //Won't want to use it again
                        grid[r, c] = choice;

                        if (recursions < MaxDepth)
                        {
                            //-----------We must go deeper. SUDCEPTION!-----------//
                            solved = (SolveGrid(checkUnique)); //Recurse
                            list.Add(choice);
                            grid[r, c] = 0;
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


    }
}
