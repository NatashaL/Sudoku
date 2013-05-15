using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
namespace Sudoku
{
    class CustomSquiggly
    {
        public int[,] scheme;
        public static int[,] solution;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;
        public int[,] Grid;
        public Difficulty difficulty;
        public bool foundit = false;

        public int tries = 1;
        public CustomSquiggly(int[,] scheme, Difficulty diff)
        {
            this.difficulty = diff;
            this.scheme = scheme;
            tries++;
            this.Grid = new int[9, 9];
            this.rows = new int[10, 10];
            this.cols = new int[10, 10];
            this.groups = new int[10, 10];

            solution = new int[9, 9];
            Random rand = new Random();

            int newVal;
            List<int> valueSet = new List<int>(Enumerable.Range(1, 9));

            Random rnd = new Random();
            int randIndex = 0;
            randIndex = rnd.Next(0, 8);
            newVal = valueSet[randIndex];

            solution[0, 0] = newVal;
            valueSet.Remove(newVal);

            for (int i = 1; i < 9; i++)
            {
                randIndex = rnd.Next(0, valueSet.Count);
                newVal = valueSet[randIndex];
                valueSet.Remove(newVal);
                solution[i, 0] = newVal;
                rows[i, newVal] = 1;
                cols[0, newVal] = 1;
                groups[getGroup(i, 0), newVal] = 1;
            }


            for (int i = 8; i >= 1; i--)
            {
                newVal = solution[0, 9 - i] = solution[i, 0];
                rows[0, newVal] = 1;
                cols[9 - i, newVal] = 1;
                groups[getGroup(0, 9 - i), newVal] = 1;
            }

            //easy
            /*int []r = {0,0,0,1,1,1,1,1,1,2,2,2,3,3,3,4,4,4,4,4,5,5,5,6,6,6,7,7,7,7,7,7,8,8,8};
            int []c = {1,5,6,0,1,2,4,6,7,0,4,5,1,6,8,0,2,4,6,8,0,2,7,3,4,8,1,2,4,6,7,8,2,3,7};
            int[] v = { 2, 9, 6, 7, 6, 3, 9, 1, 2, 9, 2, 7, 4, 7, 5, 1, 7, 6, 8, 2, 8, 2, 5, 6, 7, 9, 7, 9, 1, 5, 8, 3, 8, 3, 4 };
            */
            //hard
            /*int[] r = { 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4, 5 };
            int[] c = { 1, 5, 6, 0, 1, 2, 4, 6, 7, 0, 4, 5, 1, 6, 8, 0, 2, 4, 6, 8, 0 };
            int[] v = { 2, 9, 6, 7, 6, 3, 9, 1, 2, 9, 2, 7, 4, 7, 5, 1, 7, 6, 8, 2, 8 };

            /*int [] r = {0,0,0,0,0,1,2,2,2,3,3,5,5,6,6,6,7,8,8,8,8,8};
            int [] c = {1,2,3,4,5,0,2,6,8,1,4,4,7,0,2,6,8,3,4,5,6,7};
            int[] v = { 4, 7, 3, 5, 6, 3, 5, 4, 3, 9, 6, 1, 8, 4, 3, 6, 9, 1, 9, 5, 8, 3 };*/
            /*for (int i = 0; i < r.Length; i++)
            {
                solution[r[i], c[i]] = v[i];
                rows[r[i], v[i]] = 1;
                cols[c[i], v[i]] = 1;
                groups[getGroup(r[i], c[i]), v[i]] = 1;
            }*/
            dfs(0, 0);

            SquigglyGenerator gen = new SquigglyGenerator(Grid,scheme,difficulty);
            SquigglyGrid grid = new SquigglyGrid();
            grid.Grid = Grid;
            SquigglyGrid blanked = gen.Blanker(grid);
            solution = (int[,])Grid.Clone();
            Grid = blanked.Grid;


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
                Grid = (int[,])solution.Clone();
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
                        groups[getGroup(i, j), val] = 1;
                        dfs(ni, nj);
                        solution[i, j] = 0;
                        rows[i, val] = 0;
                        cols[j, val] = 0;
                        groups[getGroup(i, j), val] = 0;
                    }

                }
            }

        }
        public bool invalid(int i, int j, int value)
        {
            if (rows[i, value] == 1)
                return true;

            if (cols[j, value] == 1)
                return true;

            if (groups[getGroup(i, j), value] == 1)
                return true;

            return false;
        }
        public int getGroup(int i, int j)
        {
            return scheme[i, j];
        }
    }
}
