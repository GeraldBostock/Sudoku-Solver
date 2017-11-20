using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        int SUDOKU_SIZE;
        int THREAD_COUNT;
        int[,] sudoku;
        ThreadContext[] ctxs;
        int[,] isRed;
        Helper helper;
        private int winnerThreadID;

        public Form1(int[,] sudoku, int SUDOKU_SIZE, int THREAD_COUNT)
        {
            InitializeComponent();
            panel1.Location = new Point(this.Width * 5 / 100, this.Height * 5 / 100);
            this.SUDOKU_SIZE = SUDOKU_SIZE;
            this.sudoku = sudoku;
            this.THREAD_COUNT = THREAD_COUNT;
            ctxs = new ThreadContext[THREAD_COUNT];
            for(int i = 0; i < THREAD_COUNT; i++)
            {
                ctxs[i] = new ThreadContext();
                ctxs[i].sudoku = this.sudoku;
            }
            label1.Visible = false;
            label8.Visible = false;

            helper = new Helper();

            isRed = new int[9, 9];

            isRed = helper.getColor(sudoku);
            this.Text = "Sudoku Solver";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
                    {
                        if (isRed[i, j] == 1) drawBrush.Color = Color.Red;
                        else drawBrush.Color = Color.Black;
                        e.Graphics.DrawString(numberString, drawFont, drawBrush, (panel.Width * (10 * (j + 1)) / 100), (panel.Height * (10 * (i + 1)) / 100), drawFormat);
                    }
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startButton.Visible = false;

            Threads threads = new Threads(sudoku, THREAD_COUNT);
            ctxs = threads.getContexts();
            //ctxs = helper.putInOrder(ctxs);

            for(int i = 0; i < THREAD_COUNT; i++)
            {
                if (ctxs[i].winner)
                {
                    sudoku = ctxs[i].sudoku;
                    label8.Text = "Winner is Thread #" + ctxs[i].threadID;
                    label8.Font = new Font(label1.Font, FontStyle.Bold);
                    label8.Visible = true;

                    label1.Text = "Completion time: " + ctxs[i].time.ToString();
                    label1.Font = new Font(label1.Font, FontStyle.Bold);
                    label1.Visible = true;

                    winnerThreadID = i;
                }
            }

            this.Refresh();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, ctxs[0].sudoku, panel2);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, ctxs[1].sudoku, panel3);
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, ctxs[2].sudoku, panel4);
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, ctxs[3].sudoku, panel5);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, ctxs[4].sudoku, panel6);
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            drawSudoku(e, ctxs[5].sudoku, panel7);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            StepsForm winnerForm = new StepsForm(ctxs[winnerThreadID].tableStack, isRed);
            winnerForm.Text = "Thread #" + winnerThreadID;
            winnerForm.Show();
        }

        private void panel2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StepsForm frm2 = new StepsForm(ctxs[0].tableStack, isRed);
            frm2.Text = "Thread #0";
            frm2.Show();
        }

        private void panel3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StepsForm frm2 = new StepsForm(ctxs[1].tableStack, isRed);
            frm2.Text = "Thread #1";
            frm2.Show();
        }

        private void panel4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StepsForm frm2 = new StepsForm(ctxs[2].tableStack, isRed);
            frm2.Text = "Thread #2";
            frm2.Show();
        }

        private void panel5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StepsForm frm2 = new StepsForm(ctxs[3].tableStack, isRed);
            frm2.Text = "Thread #3";
            frm2.Show();
        }

        private void panel6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StepsForm frm2 = new StepsForm(ctxs[4].tableStack, isRed);
            frm2.Text = "Thread #4";
            frm2.Show();
        }

        private void panel7_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            StepsForm frm2 = new StepsForm(ctxs[5].tableStack, isRed);
            frm2.Text = "Thread #5";
            frm2.Show();
        }
    }
}
