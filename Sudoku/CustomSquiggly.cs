using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Sudoku
{
    class CustomSquiggly
    {
        public int[,] scheme;
        public static int[,] solution;
        public HashSet<int>[] rows;
        public HashSet<int>[] cols;
        public HashSet<int>[] groups;
        public int[,] Grid;
        public Difficulty difficulty;

        public CustomSquiggly(int[,] scheme, Difficulty diff)
        {
            this.difficulty = diff;
            this.scheme = scheme;

            string rez = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    rez += scheme[i, j] + " ";
                }
                rez += "\n";
            }
            MessageBox.Show(rez);


            this.Grid = new int[9, 9];
            this.rows = new HashSet<int>[9];
            this.cols = new HashSet<int>[9];
            this.groups = new HashSet<int>[9];

            for (int i = 0; i < 9; i++)
            {
                rows[i] = new HashSet<int>();
                cols[i] = new HashSet<int>();
                groups[i] = new HashSet<int>();
            }

            solution = new int[9, 9];

            Random rand = new Random();
            HashSet<int> used = new HashSet<int>();
            
            for (int i = 0; i < 15; i++)
            {
                int ii = rand.Next(0, 70)%9;
                int jj = rand.Next(0, 90)%9; 
                if (solution[ii,jj] == 0)
                {
                    int candidate = rand.Next(1, 9);
                    if (!used.Contains(candidate))
                    {
                        solution[ii, jj] = candidate;
                        rows[ii].Add(solution[ii, jj]);
                        cols[jj].Add(solution[ii, jj]);
                        groups[getGroup(ii, jj)].Add(solution[ii, jj]);
                        used.Add(candidate);
                    }
                    else
                    {
                        i = Math.Max(i-1,0);
                    }
                }
            }
            //solution[0, 0] = 3;
            rez = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    rez += solution[i, j] + " ";
                }
                rez += "\n";
            }
            MessageBox.Show(rez);
            enumerate(0, 0);
            
            SquigglyGrid squigglyGrid = new SquigglyGrid();
            SquigglyGenerator squigglyGen = new SquigglyGenerator(diff);
            SquigglySolver squigglySolver = new SquigglySolver();
            
            squigglyGrid.Grid = solution;
            SquigglyGrid permaGrid = squigglyGen.Blanker(squigglyGrid);
            Grid = permaGrid.Grid;
            
            

        }


        public void enumerate(int i, int j)
        {

            if ((i == 9) && (j == 0))
            {
                process();
                return;
            }

            // we try to find the next considered field
            int pi, pj;
            pi = i;
            pj = j + 1;
            if (pj == 9)
            {
                pi++;
                pj = 0;
            }

            if (solution[i, j] != 0)
            {
                enumerate(pi, pj);
            }
            else
            {
                int r;

                for (r = 1; r <= 9; r++)
                {

                    // we try to place r here
                    solution[i, j] = r;

                    if (!backtrack(i, j))
                    {

                        rows[i].Add(r);
                        cols[j].Add(r);
                        groups[getGroup(i, j)].Add(r);

                        enumerate(pi, pj);

                        rows[i].Remove(r);
                        cols[j].Remove(r);
                        groups[getGroup(i, j)].Remove(r);

                    }
                    solution[i, j] = 0;
                }

            }
        }
        // we check whether the value on the position (i, j) is valid
        public bool backtrack(int i, int j)
        {
            if (rows[i].Contains(solution[i, j]))
                return true;

            if (cols[j].Contains(solution[i, j]))
                return true;

            if (groups[getGroup(i, j)].Contains(solution[i, j]))
                return true;

            return false;
        }

        public void process()
        {/*
            int i, j;
            //Console.write("SOLUTION");
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    //System.out.print(solution[i,j]+" ");
                }
                //System.out.println();
            }
           
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Grid[i, j] = solution[i,j];
                }
            }
            string rez = "";
            for(int i = 0; i<9 ;i++){
                for(int j = 0; j<9 ;j++){
                    rez += Grid[i,j]+" ";
                }
                rez += "\n";
            }
            MessageBox.Show(rez);*/
        }

        public int getGroup(int i, int j)
        {
            return scheme[i, j]-1;
        }
    }
}
