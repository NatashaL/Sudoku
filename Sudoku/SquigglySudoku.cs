using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SquigglySudoku : Game
    {
        public List<List<Field>> groups;
        public SquigglySudoku(int[][] startState, int[][] solution, List<List<Field>> groups)
            : base(startState, solution)
        {
            this.groups = groups;
        }
        public SquigglySudoku(Game game, List<List<Field>> groups)
            : base(game)
        {
            this.groups = groups;
        }
    }
   
}
