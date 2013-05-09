using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class Field
    {
        public int i { get; set; }
        public int j { get; set; }
        public Field(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }
}
