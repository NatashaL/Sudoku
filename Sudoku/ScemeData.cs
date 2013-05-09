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
            List<Field> group1 = new List<Field>();
            group1.Add(new Field(0, 0));
            group1.Add(new Field(0, 1));
            group1.Add(new Field(0, 2));
            group1.Add(new Field(1, 0));
            group1.Add(new Field(1, 1));
            group1.Add(new Field(2, 0));
            group1.Add(new Field(2, 1));
            group1.Add(new Field(3, 0));
            group1.Add(new Field(3, 1));

            List<Field> group2 = new List<Field>();
            group2.Add(new Field(1, 2));
            group2.Add(new Field(1, 3));
            group2.Add(new Field(1, 4));
            group2.Add(new Field(2, 2));
            group2.Add(new Field(3, 2));
            group2.Add(new Field(4, 2));
            group2.Add(new Field(5, 2));
            group2.Add(new Field(4, 0));
            group2.Add(new Field(4, 1));

            List<Field> group3 = new List<Field>();
            group3.Add(new Field(5, 0));
            group3.Add(new Field(5, 1));
            group3.Add(new Field(6, 0));
            group3.Add(new Field(6, 1));
            group3.Add(new Field(6, 2));
            group3.Add(new Field(7, 0));
            group3.Add(new Field(7, 1));
            group3.Add(new Field(8, 0));
            group3.Add(new Field(8, 1));

            List<Field> group4 = new List<Field>();
            group4.Add(new Field(0, 3));
            group4.Add(new Field(0, 4));
            group4.Add(new Field(0, 5));
            group4.Add(new Field(0, 6));
            group4.Add(new Field(1, 5));
            group4.Add(new Field(1, 6));
            group4.Add(new Field(2, 5));
            group4.Add(new Field(3, 4));
            group4.Add(new Field(3, 5));

            List<Field> group5 = new List<Field>();
            group5.Add(new Field(2, 3));
            group5.Add(new Field(2, 4));
            group5.Add(new Field(3, 3));
            group5.Add(new Field(4, 3));
            group5.Add(new Field(4, 4));
            group5.Add(new Field(4, 5));
            group5.Add(new Field(5, 5));
            group5.Add(new Field(6, 5));
            group5.Add(new Field(6, 4));

            List<Field> group6 = new List<Field>();
            group6.Add(new Field(5, 3));
            group6.Add(new Field(5, 4));
            group6.Add(new Field(6, 3));
            group6.Add(new Field(7, 2));
            group6.Add(new Field(7, 3));
            group6.Add(new Field(8, 2));
            group6.Add(new Field(8, 3));
            group6.Add(new Field(8, 4));
            group6.Add(new Field(8, 5));

            List<Field> group7 = new List<Field>();
            group7.Add(new Field(0, 7));
            group7.Add(new Field(0, 8));
            group7.Add(new Field(1, 7));
            group7.Add(new Field(1, 8));
            group7.Add(new Field(2, 6));
            group7.Add(new Field(2, 7));
            group7.Add(new Field(2, 8));
            group7.Add(new Field(3, 7));
            group7.Add(new Field(3, 8));

            List<Field> group8 = new List<Field>();
            group8.Add(new Field(3, 6));
            group8.Add(new Field(4, 6));
            group8.Add(new Field(4, 7));
            group8.Add(new Field(4, 8));
            group8.Add(new Field(5, 6));
            group8.Add(new Field(6, 6));
            group8.Add(new Field(7, 6));
            group8.Add(new Field(7, 4));
            group8.Add(new Field(7, 5));

            List<Field> group9 = new List<Field>();
            group9.Add(new Field(8, 6));
            group9.Add(new Field(5, 7));
            group9.Add(new Field(6, 7));
            group9.Add(new Field(7, 7));
            group9.Add(new Field(8, 7));
            group9.Add(new Field(5, 8));
            group9.Add(new Field(6, 8));
            group9.Add(new Field(7, 8));
            group9.Add(new Field(8, 8));

            Scheme1.Add(group1);
            Scheme1.Add(group2);
            Scheme1.Add(group3);
            Scheme1.Add(group4);
            Scheme1.Add(group5);
            Scheme1.Add(group6);
            Scheme1.Add(group7);
            Scheme1.Add(group8);
            Scheme1.Add(group9);
        }
    }
}