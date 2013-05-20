using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Sudoku
    {
        public int[,] mask;
        public int[,] solution;
        public int[,] scheme;
        public Difficulty diff;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;
        public Sudoku(Difficulty diff)
        {
            mask = new int[9, 9];
            solution = new int[9, 9];
            scheme = new int[9, 9];

            rows = new int[10, 10];
            cols = new int[10, 10];
            groups = new int[10, 10];


            this.diff = diff;
        }
        public void init()
        {
            int newVal = -1;
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
                groups[scheme[i, 0], newVal] = 1;
            }


            for (int i = 8; i >= 1; i--)
            {
                newVal = solution[0, 9 - i] = solution[i, 0];
                rows[0, newVal] = 1;
                cols[9 - i, newVal] = 1;
                groups[scheme[0, 9 - i], newVal] = 1;
            }



        }
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

        public static bool isSolved(int[,] grid,int[,] scheme)
        {
            int[,] r = new int[9, 9];
            int[,] c = new int[9, 9];
            int[,] g = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (r[i, grid[i, j]] == 0)
                    {
                        r[i, grid[i, j]] = 1;
                    }
                    else return false;
                    if (c[j, grid[i, j]] == 0)
                    {
                        c[j, grid[i, j]] = 1;
                    }
                    else return false;
                    if (g[scheme[i, j], grid[i, j]] == 0)
                    {
                        g[scheme[i, j], grid[i, j]] = 1;
                    }
                    else return false;
                }
            }
            return true;
        }

    }
}
