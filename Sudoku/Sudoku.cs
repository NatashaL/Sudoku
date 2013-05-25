using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sudoku
{
    [Serializable]
    public class Sudoku
    {
        /// <summary>
        /// Describes which fields of the grid are to be shown to the player.
        /// Starting grid to be solved by player.
        /// </summary>
        public int[,] mask;
        /// <summary>
        /// Solution of the Sudoku
        /// </summary>
        public int[,] solution;
        /// <summary>
        /// The current playing grid, as filled by player.
        /// </summary>
        public int[,] userGrid;
        /// <summary>
        /// Determines the look of the grid
        /// </summary>
        public int[,] scheme;
        /// <summary>
        /// Difficulty of game
        /// </summary>
        public Difficulty diff;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;
        /// <summary>
        /// number of seconds the player has been playing
        /// </summary>
        public int ticks;

        /// <summary>
        /// Returns an Object of type Sudoku
        /// </summary>
        /// <param name="diff">Difficulty of Sudoku</param>
        public Sudoku(Difficulty diff)
        {
            mask = new int[9, 9];
            solution = new int[9, 9];
            scheme = new int[9, 9];
            userGrid = new int[9, 9];

            rows = new int[10, 10];
            cols = new int[10, 10];
            groups = new int[10, 10];

            ticks = 0;
            this.diff = diff;
        }
        /// <summary>
        /// Randomly initializes the starting grid.
        /// </summary>
        public void init()
        {
            int newVal = -1;
            List<int> valueSet = new List<int>(Enumerable.Range(1, 9));

            Random rnd = new Random();
            int randIndex = 0;
            randIndex = rnd.Next(0, 8);
            newVal = valueSet[randIndex];

            userGrid[0, 0] = newVal;
            valueSet.Remove(newVal);

            for (int i = 1; i < 9; i++)
            {
                randIndex = rnd.Next(0, valueSet.Count);
                newVal = valueSet[randIndex];
                valueSet.Remove(newVal);
                userGrid[i, 0] = newVal;
                rows[i, newVal] = 1;
                cols[0, newVal] = 1;
                groups[scheme[i, 0], newVal] = 1;
            }


            for (int i = 8; i >= 1; i--)
            {
                newVal = userGrid[0, 9 - i] = userGrid[i, 0];
                rows[0, newVal] = 1;
                cols[9 - i, newVal] = 1;
                groups[scheme[0, 9 - i], newVal] = 1;
            }



        }
        /// <summary>
        /// Checks if the value "value" is invalid for the field [i,j]
        /// </summary>
        /// <param name="i">Row index</param>
        /// <param name="j">Column index</param>
        /// <param name="value">Value</param>
        /// <returns>True if invalid, False if valid</returns>
        public bool invalid(int i, int j, int value)
        {
            if (rows[i, value] == 1)
                return true;

            if (cols[j, value] == 1)
                return true;

            if (groups[scheme[i, j], value] == 1)
                return true;

            return false;
        }
        /// <summary>
        /// Checks to see if [,]grid is fully solved.
        /// </summary>
        /// <param name="grid">Grid that needs to be checked.</param>
        /// <param name="scheme">The scheme describing the grid.</param>
        /// <returns>True if solved, False if not solved.</returns>
        public static bool isSolved(int[,] grid,int[,] scheme)
        {
            int[,] r = new int[9, 10];
            int[,] c = new int[9, 10];
            int[,] g = new int[9, 10];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid[i, j] == 0) return false;

                    if (r[i, grid[i, j]] == 1 || c[j, grid[i, j]] == 1 || g[scheme[i, j], grid[i, j]] == 1)
                    {
                       // MessageBox.Show(r[i, grid[i, j]] + " " + c[j, grid[i, j]] + " " + g[scheme[i, j], grid[i, j]]);
                        return false;
                    }

                    if (r[i, grid[i, j]] == 0)
                    {
                        r[i, grid[i, j]] = 1;
                    }
                    if (c[j, grid[i, j]] == 0)
                    {
                        c[j, grid[i, j]] = 1;
                    }
                    if (g[scheme[i, j], grid[i, j]] == 0)
                    {
                        g[scheme[i, j], grid[i, j]] = 1;
                    }
                }
            }
            return true;
        }
    }
}
