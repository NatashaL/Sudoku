using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Game
    {
        public int[,] startState{get; set;}
        public int[,] solution { get; set; }

        public Game(int[,] startState, int[,] solution)
        {
            this.startState = startState;
            this.solution = solution;
        }
        public Game(Game game)
        {
            this.startState = game.startState;
            this.solution = game.solution;
        }

    }
    
}
