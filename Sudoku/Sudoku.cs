using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku
{
    class Sudoku
    {
        private String sudokuID;
        private int threadID;
        private SudokuCompleted completedCallback;
        private String time;
        private Stopwatch sw = new Stopwatch();
        private String random = "not done";
        private bool keepGoing = true;
        private int[,] sudoku;
        Random rnd = new Random();

        internal bool KeepGoing
        {
            get { return keepGoing; }
            set { keepGoing = value; }
        }

        internal String Random
        {
            get { return random; }
            set { random = value; }
        }

        internal int ThreadID
        {
            get { return threadID; }
            set { threadID = value; }
        }

        internal String Time
        {
            get { return time; }
            set { time = value;  }
        }

        internal String SudokuID
        {
            get { return sudokuID; }
            set { sudokuID = value; }
        }

        internal SudokuCompleted CompletedCallback
        {
            get { return completedCallback; }
            set { completedCallback = value; }
        }

        public ThreadContext solveSudoku(int[,] sudoku)
        {
            ThreadContext ctx = new ThreadContext(sudoku);
            this.sudoku = sudoku;

            Stopwatch sw = new Stopwatch();

            sw.Start();
            if (solve())
            {
                ctx.sudoku = this.sudoku;

                Console.WriteLine();
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            ctx.time = sw.Elapsed;

            return ctx;
        }

        public bool solve()
        {
            int[] emptyCell = findEmptyCell();
            int row = emptyCell[0];
            int col = emptyCell[1];

            if (row == -1)
            {
                return true;
            }

            for (int i = 1; i <= 9; i++)
            {
                if (isViable(row, col, i))
                {
                    sudoku[row, col] = i;

                    if (solve())
                    {
                        return true;
                    }

                    sudoku[row, col] = 0;
                }
            }
            return false;
        }

        public bool isViable(int row, int column, int n)
        {
            if (!UsedInRow(row, n) && !UsedInColumn(column, n)
                && !UsedInBox(row - row % 3, column - column % 3, n))
            {
                return true;
            }
            return false;
        }

        public bool UsedInRow(int row, int n)
        {
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[row, i] == n)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UsedInColumn(int col, int n)
        {
            for (int i = 0; i < 9; i++)
            {
                if (sudoku[i, col] == n)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UsedInBox(int boxStartRow, int boxStartCol, int n)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (sudoku[i + boxStartRow, j + boxStartCol] == n)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int[] findEmptyCell()
        {
            int[] emptyCell = new int[2];

            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if(sudoku[i, j] == 0)
                    {
                        emptyCell[0] = i;
                        emptyCell[1] = j;
                        return emptyCell;
                    }
                }
            }

            emptyCell[0] = -1;
            return emptyCell;
        }
    }
}
