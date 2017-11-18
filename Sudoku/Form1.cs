﻿using System;
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
        int SUDOKU_SIZE;
        int THREAD_COUNT;
        int[,] sudoku;

        public Form1(int[,] sudoku, int SUDOKU_SIZE, int THREAD_COUNT)
        {
            InitializeComponent();
            panel1.Location = new Point(this.Width * 5 / 100, this.Height * 5 / 100);
            this.SUDOKU_SIZE = SUDOKU_SIZE;
            this.sudoku = sudoku;
            this.THREAD_COUNT = THREAD_COUNT;
        }

        private void sudokuPaintEventHandler(object sender, PaintEventArgs e, int[,] sudoku)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel1);
        }

        private void drawSudoku(PaintEventArgs e, int[,] sudoku, Panel panel)
        {
            int start = panel.Width * 5 / 100;
            int sudokuSize = panel.Width * 90 / 100;
            int cellSize = panel.Width * 10 / 100;

            Pen widePen = new Pen(Color.FromArgb(255, 0, 0, 0), 5);

            Rectangle rectangle = new Rectangle(start, start, sudokuSize, sudokuSize);
            e.Graphics.DrawRectangle(widePen, rectangle);

            Font drawFont = new Font("Arial", panel.Width / 20);
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.LineAlignment = StringAlignment.Center;

            for (int i = 1; i < SUDOKU_SIZE; i++)
            {
                if (i % 3 == 0)
                {
                    e.Graphics.DrawLine(widePen, start, start + (cellSize * i), start + sudokuSize, start + (cellSize * i));
                    e.Graphics.DrawLine(widePen, start + (cellSize * i), start, start + (cellSize * i), start + sudokuSize);
                }
                else
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(start, start + (cellSize * i)), new Point(start + sudokuSize, start + (cellSize * i)));
                    e.Graphics.DrawLine(Pens.Black, start + (cellSize * i), start, start + (cellSize * i), start + sudokuSize);
                }
            }

            for (int i = 0; i < SUDOKU_SIZE; i++)
            {
                for (int j = 0; j < SUDOKU_SIZE; j++)
                {
                    string numberString = sudoku[i, j].ToString();
                    if (numberString != "0")
                        e.Graphics.DrawString(numberString, drawFont, drawBrush, (panel.Width * (10 * (j + 1)) / 100), (panel.Height * (10 * (i + 1)) / 100), drawFormat);
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            ThreadContext ctx = new ThreadContext();
            Threads threads = new Threads(sudoku, THREAD_COUNT);
            ctx = threads.ctx;

            Console.WriteLine();

            /*for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(ctx.sudoku[i, j] + " ");
                }
                Console.WriteLine();
            }*/

            //Console.Write(ctx.time);

            this.Refresh();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel2);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel3);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel4);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel5);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel6);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, sudoku, panel7);
        }
    }
}
