using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Serializable]
    public class HighScoreItem : IComparable<HighScoreItem>
    {
        public string player { get; set; }
        public int time { get; set; }

        public int CompareTo(HighScoreItem other)
        {
            return this.time.CompareTo(other.time);
        }
        public bool Equals(HighScoreItem other)
        {
            return this.player.Equals(other.player) && this.time.Equals(other.time);
        }
    }
}
