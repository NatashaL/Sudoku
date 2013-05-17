using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Standard
    {
        public int[,] mask;
        public int[,] solution;
        public int[,] rows;
        public int[,] cols;
        public int[,] groups;
        public Difficulty diff;
        public Random random = new Random();
        public int[,] getGroup;

        public Standard(Difficulty diff)
        {
            this.diff = diff;
            /*
            initGetGroup();
            pickColors();
            init();
            mask();*/
        }
        public void init()
        {
            rows = new int[9, 10];
            cols = new int[9, 10];
            groups = new int[9, 10];                    //Randomly fill in the first row and column of puzzlegrid
            int[,] tempGrid = new int[9, 9];            
            bool[] used = new bool[10];                    			
            int val = 0;                        	            

            for (int r = 0; r < 9; r++)
            {
                val = random.Next() % 139 % 37 % 9 + 1;
                used[val] = true;
                initCell(r, 0, val);
                rows[r, val]++;
                cols[0, val]++;
                groups[getGroup[r, 0], val]++;

            }
            for (int c = 8; c >= 1; c--)
            {
                val = solution[0, 9 - c] = solution[c, 0];
                rows[0, val]++;
                cols[9 - c, val]++;
                groups[getGroup[0, 9 - c], val]++;
            }


            solution = solve();
            mask = Blanker(SolutionGrid);       //call Blanker to carry out the
            //return mask;         //blanking of fileds,then return the grid to user to solve
        }
        public void initCell(int r, int c, int val)
        {

        }
        public int[,] solve()
        {
            Solver solver = new Solver();
        }


        public void initGetGroup()
        {
            int[] temp = {1,1,1,2,2,2,3,3,3,
                         1,1,1,2,2,2,3,3,3,
                         1,1,1,2,2,2,3,3,3,
                         4,4,4,5,5,5,6,6,6,
                         4,4,4,5,5,5,6,6,6,
                         4,4,4,5,5,5,6,6,6,
                         7,7,7,8,8,8,9,9,9,
                         7,7,7,8,8,8,9,9,9,
                         7,7,7,8,8,8,9,9,9};
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    getGroup[i, j] = temp[i * 9 + j] - 1; // - 1, za da bidat grupite od 0 do 8
                }
            }
        }

        public int[,] Blanker(int[,] solvedGrid)
        {                          //enable blanking of squares based on difficulty
            int[,] tempGrid;
            int[,] saveCopy;
            //temporary grids to save between tests
            bool unique = true;          //flag for if blanked form has unique soln
            int totalBlanks = 0;	                      //count of current blanks
            int tries = 0;                  //count of tries to blank appropriately
            int desiredBlanks;            //amount of blanks desired via difficulty
            int symmetry = 0;                                       //symmetry type
            tempGrid = (PuzzleGrid)solvedGrid.Clone();
            //cloned input grid (no damage)
            Random rnd = new Random();         //allow for random number generation

            switch (difficulty)           //set desiredBlanks via chosen difficulty
            {
                case Difficulty.Easy: //easy difficulty
                    desiredBlanks = 5;
                    break;
                case Difficulty.Medium: //medium difficulty
                    desiredBlanks = 50;
                    break;
                case Difficulty.Hard: //hard difficulty
                    desiredBlanks = 60;
                    break;
                default: //easy difficulty
                    desiredBlanks = 40;
                    break;
            }

            symmetry = rnd.Next(0, 2);                   //Randomly select symmetry
            do
            {          //call RandomlyBlank() to blank random squares symmetrically
                saveCopy = (PuzzleGrid)tempGrid.Clone();     // in case undo needed
                tempGrid = RandomlyBlank(tempGrid, symmetry, ref totalBlanks);
                //blanks 1 or 2 squares according to symmetry chosen
                puzzleSolver = new PuzzleSolver();
                unique = puzzleSolver.SolveGrid((PuzzleGrid)tempGrid.Clone(), true);         // will it solve uniquely?
                if (!unique)
                {
                    tempGrid = (PuzzleGrid)saveCopy.Clone();
                    tries++;
                }
            } while ((totalBlanks < desiredBlanks) && (tries < 1000));
            solvedGrid = tempGrid;
            solvedGrid.Finish();
            return solvedGrid;
        }


    }
}
