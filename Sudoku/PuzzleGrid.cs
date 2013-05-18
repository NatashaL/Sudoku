using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public class PuzzleGrid : ICloneable
    {
        /// <summary>
        /// This is the internal grid element.
        /// Reading of the private grid element is allowed. Setting it is not allowed.
        /// </summary>
        public int[,] Grid;
        /// <summary>
        /// Max number in any part of the array
        /// </summary>
        private const int Max = 9;
        public int[,] rows = new int[9, 10];
        public int[,] cols = new int[9, 10];
        public int[,] groups = new int[9, 10];
        public static int[] miniSquare = {0,0,0,1,1,1,2,2,2,
                                          0,0,0,1,1,1,2,2,2,
                                          0,0,0,1,1,1,2,2,2,
                                          3,3,3,4,4,4,5,5,5,
                                          3,3,3,4,4,4,5,5,5,
                                          3,3,3,4,4,4,5,5,5,
                                          6,6,6,7,7,7,8,8,8,
                                          6,6,6,7,7,7,8,8,8,
                                          6,6,6,7,7,7,8,8,8};


        /// <summary>
        /// This constructor creates the grid.
        /// </summary>
        public PuzzleGrid()
        {
            Grid = new int[9, 9];   


        }

        /// <summary>
        /// This sets a cell to any, not validating if it's setting it to a user settable value.
        /// </summary>
        /// <param name="rowA">The 0-based row number.</param>
        /// <param name="columnB">The 0-based column number.</param>
        /// <param name="value">The value to set the cell to.</param>
        /// <returns>1 for success, 0 for a failure.</returns>
        public int InitSetCell(int rowA, int columnB, int value)
        {
            
            int success = 0;
            bool validIndex = false;
            bool validNewVal = false;

            //confirm that a valid grid location is used
            if (rowA >= 0 && rowA < 9 && columnB >= 0 && columnB < 9)
            {
                validIndex = true;
            }

            //confirm new value is in range -9..9
            if (value >= -9 && value <= 9)
            {
                validNewVal = true;
            }

            // if everything is valid proceed
            if (validIndex && validNewVal)
            {
                Grid[rowA, columnB] = value;
                success = 1;
            }
            else
            {
                success = 0;
            }

            return (success);
        }
        public void initCell(int row, int col, int val)
        {
            Grid[row, col] = val;
            rows[row, val]++;
            cols[col, val]++;
            groups[miniSquare[row * 9 + col], val]++;
        }


        public bool solved()
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
        public int groupNum(int row, int col)
        {
            int result = 0;
            int r = row / 3;
            int c = col / 3;
                       
            return result;
        }
        public void emptyCell(int i, int j, int val){
            Grid[i, j] = 0;
            rows[i, val]--;
            cols[j, val]--;
            groups[groupNum(i, j), val]--;
        }
        public void init()
        {
            rows.Initialize();
            cols.Initialize();
            groups.Initialize();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (Grid[i, j] != 0)
                    {
                        int val = Math.Abs(Grid[i, j]);
                        try
                        {
                            
                            rows[i, val]++;
                            cols[j, val]++;

                            groups[miniSquare[i * 9 + j], val]++;
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(string.Format("i {0}\n j {1}\n i*9+j {2}\n val {3}\n minisquare[i*9 + j] {4}", i, j, i * 9 + j, val, miniSquare[i * 9 + j]));
                        }
                    }
                }
            }
        }



        /// <summary>
        /// This function also sets a cell, but validates that is within the user setable range.
        /// </summary>
        /// <param name="rowA">The 0-based row number.</param>
        /// <param name="columnB">The 0-based column number.</param>
        /// <param name="value">The value to set the cell to.</param>
        /// <returns>1 for success, 0 for a failure.</returns>
        public int UserSetCell(int rowA, int columnB, int value)
        {
            int success = 0;
            bool validIndex = false;
            bool validNewVal = false;
            bool canReplace = false;

            if (rowA >= 0 && rowA < 9 && columnB >= 0 && columnB < 9)
            {
                validIndex = true; //confirm that valid grid location is used
            }

            // confirm new value is in range 0..9
            if (value >= 0 && value <= 9)
            {
                validNewVal = true;
            }

            // confirm value in location is replaceable
            if (Grid[rowA, columnB] >= 0)
            {
                canReplace = true;
            }

            if (validIndex && validNewVal && canReplace)
            {
                Grid[rowA, columnB] = value;
                success = 1;
            }
            else
            {
                success = 0;
            }

            return success;
        }

        /// <summary>
        /// This function makes sure all this grid's values are non-editable
        /// </summary>
        public void Finish()
        {
            for (var i = 0; i < Max; i++)
            {
                for (var j = 0; j < Max; j++)
                {
                    Grid[i, j] = Math.Abs(Grid[i, j]) * -1;
                }
            }
        }

        /// <summary>
        /// This clones the object.
        /// </summary>
        /// <returns>A clone of itself.</returns>
        public object Clone()
        {
            //enable cloning for safe copying of the object
            PuzzleGrid p = new PuzzleGrid();
            for (int i = 0; i < Max; i++)
            {
                for (int j = 0; j < Max; j++)
                {
                    p.InitSetCell(i, j, Grid[i, j]);
                }
            }
            return p;
        }

        public bool IsBlank()
        {
            var isBlank = true;
            for (var i = 0; i < Max; i++)
            {
                for (var j = 0; j < Max; j++)
                {
                    if (Grid[i, j] != 0)
                    {
                        isBlank = false;
                    }
                }
            }

            return isBlank;
        }
    }

}
