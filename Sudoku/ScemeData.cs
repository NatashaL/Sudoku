using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public void schemeBuilder()
        {
            int [,]Scheme = new int[9,9];
            Scheme[0, 0] = 
            Scheme[0, 1] =
            Scheme[0, 2] =
            Scheme[1, 0] = 
            Scheme[1, 1] = 
            Scheme[2, 0] = 
            Scheme[2, 1] = 
            Scheme[3, 0] = 
            Scheme[3, 1] = 1;

            Scheme[1, 2] =
            Scheme[1, 3] =
            Scheme[1, 4] =
            Scheme[2, 2] =
            Scheme[3, 2] =
            Scheme[4, 2] =
            Scheme[5, 2] =
            Scheme[4, 0] =
            Scheme[4, 1] = 2;

            Scheme[5, 0] =
            Scheme[5, 1] =
            Scheme[6, 0] =
            Scheme[6, 1] =
            Scheme[6, 2] =
            Scheme[7, 0] =
            Scheme[7, 1] =
            Scheme[8, 0] =
            Scheme[8, 1] = 3;

            Scheme[0, 3] =
            Scheme[0, 4] =
            Scheme[0, 5] =
            Scheme[0, 6] =
            Scheme[1, 5] =
            Scheme[1, 6] =
            Scheme[2, 5] =
            Scheme[3, 4] =
            Scheme[3, 5] = 4;

            Scheme[2, 3] =
            Scheme[2, 4] =
            Scheme[3, 3] =
            Scheme[4, 3] =
            Scheme[4, 4] =
            Scheme[4, 5] =
            Scheme[5, 5] =
            Scheme[6, 5] =
            Scheme[6, 4] = 5;

            Scheme[5, 3] =
            Scheme[5, 4] =
            Scheme[6, 3] =
            Scheme[7, 2] =
            Scheme[7, 3] =
            Scheme[8, 2] =
            Scheme[8, 3] =
            Scheme[8, 4] =
            Scheme[8, 5] = 6;

            Scheme[0, 7] =
            Scheme[0, 8] =
            Scheme[1, 7] =
            Scheme[1, 8] =
            Scheme[2, 6] =
            Scheme[2, 7] =
            Scheme[2, 8] =
            Scheme[3, 7] =
            Scheme[3, 8] = 7;

            Scheme[3, 6] =
            Scheme[4, 6] =
            Scheme[4, 7] =
            Scheme[4, 8] =
            Scheme[5, 6] =
            Scheme[6, 6] =
            Scheme[7, 6] =
            Scheme[7, 4] =
            Scheme[7, 5] = 8;

            Scheme[8, 6] =
            Scheme[5, 7] =
            Scheme[6, 7] =
            Scheme[7, 7] =
            Scheme[8, 7] =
            Scheme[5, 8] =
            Scheme[6, 8] =
            Scheme[7, 8] =
            Scheme[8, 8] = 9;

            Schemes.Add(Scheme);
        }
    }
}