using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Serializable]
    public class Scores
    {   
        /// <summary>
        /// Represent a HashMap with 2 entries, that maps gameType to an array of HighScores Objects.
        /// Each HighScore Object represents the High scores for 
        /// the different gameType and Difficulty combination.
        /// </summary>
        public Dictionary<gameType, HighScores[]> HS;

        public Scores()
        {
            HS = new Dictionary<gameType, HighScores[]>();
            HS.Add(gameType.Standard,new HighScores[3]);
            HS.Add(gameType.Squiggly, new HighScores[3]);

            for (int i = 0; i < 3; i++)
            {
                HS[gameType.Standard][i] = new HighScores();
                HS[gameType.Squiggly][i] = new HighScores();
            }

        }
        /// <summary>
        /// Adds an entry to the map, according to the gameType and Difficulty parameters
        /// </summary>
        /// <param name="item">HighScoreItem to be added.</param>
        /// <param name="type">Standard or Squiggly</param>
        /// <param name="diff">Easy, Medium or Hard</param>
        /// <returns></returns>
        public bool add(HighScoreItem item, gameType type, Difficulty diff)
        {
            int d = diff == Difficulty.Easy ? 0 : (diff == Difficulty.Medium ? 1 : 2);

            return HS[type][d].add(item);
        }


    }
}
