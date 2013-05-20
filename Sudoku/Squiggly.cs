using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Squiggly : Sudoku
    {

        SquigglyGrid squigglyGrid;
        SquigglySolver squigglySolver;
        SquigglyGenerator squigglyGenerator;

        Random random;
        private bool foundit;

        public Squiggly(Difficulty diff,int[,] scheme):base(diff)
        {
            random = new Random();
            base.scheme = scheme;
            foundit = false;
            base.init();
            dfs(0, 0);

            squigglyGenerator = new SquigglyGenerator(solution, scheme, diff);
            squigglyGrid = new SquigglyGrid();
            squigglyGrid.Grid = solution;
            SquigglyGrid blanked = squigglyGenerator.Blanker(squigglyGrid);
            mask = blanked.Grid;

        }
        public void dfs(int i, int j)
        {

            if (foundit) return;

            if (i == 9)
            {
                foundit = true;

                /*string rez = "$$$$$$ try" + tries + "\n";
                for (int ii = 0; ii < 9; ii++)
                {
                    for (int jj = 0; jj < 9; jj++)
                    {
                        rez += solution[ii, jj] + " ";
                    }
                    rez += "\n";
                }
                MessageBox.Show(rez);*/
                solution = (int[,])solution.Clone();
                return;
            }

            int ni = i;
            int nj = j + 1;

            if (nj == 9)
            {
                ni = i + 1;
                nj = 0;
            }


            if (solution[i, j] != 0)
            {
                dfs(ni, nj);
            }
            else
            {
                for (int val = 1; !foundit && val <= 9; val++)
                {
                    if (!invalid(i, j, val))
                    {
                        solution[i, j] = val;
                        rows[i, val] = 1;
                        cols[j, val] = 1;
                        groups[base.scheme[i, j], val] = 1;
                        dfs(ni, nj);
                        solution[i, j] = 0;
                        rows[i, val] = 0;
                        cols[j, val] = 0;
                        groups[base.scheme[i, j], val] = 0;
                    }

                }
            }

        }
        
    }
}
