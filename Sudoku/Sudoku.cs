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

        public Sudoku(Difficulty diff)
        {
            mask = new int[9, 9];
            solution = new int[9, 9];
            scheme = new int[9, 9];
            this.diff = diff;
        }
        public static bool isSolved(int[,] grid)
        {
            return true;
        }

    }
}
