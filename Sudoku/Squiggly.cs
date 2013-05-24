using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sudoku
{
    [Serializable]
    public class Squiggly : Sudoku
    {

        SquigglyGrid squigglyGrid;
        SquigglyGenerator squigglyGenerator;

        Random random;
        private bool foundit;
        
        public Squiggly(Difficulty diff,int[,] scheme):base(diff)
        {
            random = new Random();
            base.scheme = scheme;
            foundit = false;
            base.init();
            solve(0, 0);

            squigglyGenerator = new SquigglyGenerator(solution, scheme, diff);
            squigglyGrid = new SquigglyGrid();
            squigglyGrid.Grid = solution;
            SquigglyGrid blanked = squigglyGenerator.Blanker(squigglyGrid);
            mask = blanked.Grid;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mask[i, j] = -mask[i, j];
                    userGrid[i, j] = mask[i, j];
                }
            }

        }
        /// <summary>
        /// Solve the starting grid
        /// </summary>
        /// <param name="i">Row index</param>
        /// <param name="j">Column index</param>
        public void solve(int i, int j)
        {
            if (foundit) return;

            if (i == 9)
            {
                foundit = true;
                solution = (int[,])userGrid.Clone();
                return;
            }

            int ni = i;
            int nj = j + 1;
            if (nj == 9)
            {
                ni = i + 1;
                nj = 0;
            }
            if (userGrid[i, j] != 0)
            {
                solve(ni, nj);
            }
            else
            {
                for (int val = 1; !foundit && val <= 9; val++)
                {
                    if (!invalid(i, j, val))
                    {
                        userGrid[i, j] = val;
                        rows[i, val] = 1;
                        cols[j, val] = 1;
                        groups[base.scheme[i, j], val] = 1;
                        solve(ni, nj);
                        userGrid[i, j] = 0;
                        rows[i, val] = 0;
                        cols[j, val] = 0;
                        groups[base.scheme[i, j], val] = 0;
                    }
                }
            }

        }
        
    }
}
