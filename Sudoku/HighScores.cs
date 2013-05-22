using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Serializable]
    public class HighScores
    {
        public List<HighScoreItem> highScores { get; set; }
        
        public HighScores()
        {
            highScores = new List<HighScoreItem>();
        }
        public void sort()
        {
            highScores.Sort();
        }
        public bool add(HighScoreItem item)
        {
            highScores.Add(item);
            highScores.Sort();

            if (highScores.Count > 5)
            {
                bool madeit = false;

                HighScores temp = new HighScores();
                for (int i = 0; i < 5; i++)
                {
                    temp.highScores[i] = this.highScores[i];
                    if (temp.highScores[i].Equals(item))
                    {
                        madeit = true;
                    }
                }
                this.highScores = temp.highScores;

                return madeit;
            }
            else
            {
                return true;
            }
        }
    }
}
