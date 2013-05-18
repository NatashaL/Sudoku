using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    abstract public class Sudoku
    {
        
        public int[,] mask;
        public int[,] solution;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;
        public Difficulty diff;
        public Random random = new Random();
        public int[,] getGroup;

        public Sudoku(Difficulty diff)
        {
            this.diff = diff;
            rows = new int[9, 10];
            cols = new int[9, 10];
            groups = new int[9, 10];
            getGroup = new int[9, 10];
            solution = new int[9, 9];
            initGetGroup();
            init();
        }
        public void init()
        {
            //Randomly fill in the first row and column of puzzlegrid          
            bool[] used = new bool[10];                    			
            int val = 0;                        	            

            for (int r = 0; r < 9; r++)
            {
                val = random.Next() % 139 % 37 % 9 + 1;
                if (used[val] == false)
                {
                    used[val] = true;
                    initCell(r, 0, val);
                    rows[r, val]++;
                    cols[0, val]++;
                    groups[getGroup[r, 0], val]++;
                }
                else
                {
                    r = Math.Max(r-1,0);
                }

            }
            for (int c = 8; c >= 1; c--)
            {
                val = solution[0, 9 - c] = solution[c, 0];
                rows[0, val]++;
                cols[9 - c, val]++;
                groups[getGroup[0, 9 - c], val]++;
            }

        }
        public void initCell(int r, int c, int val)
        {
            solution[r, c] = val;
        }
        abstract public int[,] solve();
        public void initGetGroup()
        {
            int[] temp =   {0,0,0,1,1,1,2,2,2,
                            0,0,0,1,1,1,2,2,2,
                            0,0,0,1,1,1,2,2,2,
                            3,3,3,4,4,4,5,5,5,
                            3,3,3,4,4,4,5,5,5,
                            3,3,3,4,4,4,5,5,5,
                            6,6,6,7,7,7,8,8,8,
                            6,6,6,7,7,7,8,8,8,
                            6,6,6,7,7,7,8,8,8};
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    getGroup[i, j] = temp[i * 9 + j]; 
                }
            }
        }
    }
}
