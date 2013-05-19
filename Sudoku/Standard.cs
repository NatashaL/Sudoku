using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Standard : Sudoku
    {
        public PuzzleGrid puzzleGrid;
        public PuzzleSolver puzzleSolver;
        public PuzzleGenerator puzzleGenerator;

        public Standard(Difficulty diff): base (diff)
        {
            puzzleGrid = new PuzzleGrid();
            puzzleSolver = new PuzzleSolver();
            puzzleGenerator = new PuzzleGenerator(diff);

            puzzleGenerator.InitGrid();
            base.solution = puzzleGenerator.SolutionGrid.Grid;
            base.mask = puzzleGenerator.PermaGrid.Grid;//check for negative values 


        }

    }
}
