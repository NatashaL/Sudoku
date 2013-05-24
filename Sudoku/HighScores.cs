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
        /// <summary>
        /// Represents a List of top 5 high scores achieved.
        /// </summary>
        public List<HighScoreItem> highScores { get; set; }
        
        public HighScores()
        {
            highScores = new List<HighScoreItem>();
        }
        /// <summary>
        /// Sort in decreasing order.
        /// </summary>
        public void sort()
        {
            highScores.Sort();
        }
        /// <summary>
        /// Add item to the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if item made it in the top 5, False in not.</returns>
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
