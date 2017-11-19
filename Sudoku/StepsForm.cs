using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Sudoku
{
    public partial class StepsForm : Form
    {
        private readonly int SUDOKU_SIZE = 9;
        public int[,] isRed;
        ArrayList tableStack;
        int i = 0;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        int[,] sudoku = new int[9, 9];

        public StepsForm(ArrayList tableStack, int[,] isRed)
        {
            InitializeComponent();
            this.tableStack = tableStack;
            this.isRed = isRed;

            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = (1000) * (1);
            timer.Enabled = true;                       
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if(i < tableStack.Count)
            {
                SudokuStatus status = (SudokuStatus)tableStack[i];
                sudoku = status.getTable();
                i++;
                panel1.Refresh();
            }
            else
            {
                timer.Stop();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int start = panel1.Width * 5 / 100;
            int sudokuSize = panel1.Width * 90 / 100;
            int cellSize = panel1.Width * 10 / 100;

            Pen widePen = new Pen(Color.FromArgb(255, 0, 0, 0), 5);

            Rectangle rectangle = new Rectangle(start, start, sudokuSize, sudokuSize);
            e.Graphics.DrawRectangle(widePen, rectangle);

            Font drawFont = new Font("Arial", panel1.Width / 20);
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
                        e.Graphics.DrawString(numberString, drawFont, drawBrush, (panel1.Width * (10 * (j + 1)) / 100), (panel1.Height * (10 * (i + 1)) / 100), drawFormat);
                    }
                }
            }
        }
    }
}
