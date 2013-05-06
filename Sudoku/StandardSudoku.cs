using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class StandardSudoku : Game
    {
        public StandardSudoku(int[][] startState, int[][] solution)
            : base(startState, solution)
        {
        }
        public StandardSudoku(Game game)
            : base(game)
        {
        }
    }
   
}
