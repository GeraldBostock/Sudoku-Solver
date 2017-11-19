using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private String time;
        private Stopwatch sw = new Stopwatch();
        private bool keepGoing = true;
        private int[,] sudoku;
        public int[,] lastSudoku = new int[9,9];
        Helper helper = new Helper();
        List<int>[] candidates = new List<int>[81];
        int size = 9;
        private ArrayList tableStack;
        ThreadContext ctx;

        internal bool KeepGoing
        {
            get { return keepGoing; }
            set { keepGoing = value; }
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

        public ThreadContext solveSudoku(int[,] sudoku, int threadID)
        {
            tableStack = new ArrayList();

            Console.WriteLine("Thread #" + threadID + " is entering");

            this.sudoku = sudoku;
            ctx = new ThreadContext(sudoku);
            this.threadID = threadID;

            helper.writeToFile(sudoku, threadID);
            Stopwatch sw = new Stopwatch();

            sw.Start();
            while (calculateLastDigit())
            {
                populateCells();
                helper.writeToFile(sudoku, threadID);
            }

            if (isDone())
            {
                ctx.sudoku = sudoku;
                ctx.time = sw.Elapsed;
                ctx.threadID = threadID;
                return ctx;
            }

            tableStack.Add(new SudokuStatus(sudoku, -1, -1));

            if (solve())
            {
                ctx.sudoku = ((SudokuStatus)tableStack[tableStack.Count - 1]).getTable();
            }
            sw.Stop();

            ctx.setTableStack(tableStack);
            ctx.sudoku = ((SudokuStatus)tableStack[tableStack.Count - 1]).getTable();
            ctx.time = sw.Elapsed;
            ctx.threadID = threadID;

            Console.WriteLine("Thread #" + threadID + " is exiting");
            return ctx;
        }

        public bool calculateLastDigit()
        {
            for (int i = 0; i < 81; i++)
            {
                candidates[i] = new List<int>();
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        for (int k = 1; k <= 9; k++)
                        {
                            if (!isInRow(i, k) && !isInColumn(j, k) && !isInBox(i - i % 3, j - j % 3, k))
                            {
                                candidates[i * 9 + j].Add(k);
                            }
                        }

                        if (candidates[i * 9 + j].Count == 1) return true; // sudoku[i, j] = candidates[i * 9 + j].First();
                    }
                }
            }

            return false;
        }

        public void populateCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        if (candidates[i * 9 + j].Count == 1) sudoku[i, j] = candidates[i * 9 + j].First();
                    }
                }
            }
        }

        public bool isInRow(int row, int n)
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

        public bool isInColumn(int col, int n)
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

        public bool isInBox(int boxStartRow, int boxStartCol, int n)
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

        public bool solve()
        {
            int[] nextBlankCoord = nextRandomCoord();

            //helper.writeToFile(((SudokuStatus)tableStack[tableStack.Count - 1]).getTable(), threadID);

            for (int i = 1; i <= 9; i++)
            {
                if (!isValidValue(i, nextBlankCoord[0], nextBlankCoord[1])) continue;

                addValue(i, nextBlankCoord[0], nextBlankCoord[1]);

                if (isSolved() || !keepGoing)
                {
                    //ctx.tableStack = tableStack;
                    helper.writeToFile(tableStack, threadID);
                    return true;
                }
                if (solve() || !keepGoing)
                {
                    //helper.writeToFile(((SudokuStatus)tableStack[tableStack.Count - 1]).getTable(), threadID);
                    return true;
                }

                removeValue();
            }

            return false;
        }

        public bool isDone()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0) return false;
                }
            }

            return true;
        }

        public void removeValue()
        {
            tableStack.RemoveAt(tableStack.Count - 1);
        }

        public bool isSolved()
        {
            int[,] lastTable = getLastTable();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (lastTable[i, j] == 0) return false;
                }
            }
            return true;
        }

        private int[,] createTable()
        {
            int[,] table = new int[size, size];
            return table;
        }

        private int[,] createTable(int value, int x, int y)
        {
            int[,] lastTable = getLastTable();
            int[,] table = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    table[i, j] = lastTable[i, j];
                }
            }
            table[x, y] = value;
            return table;
        }

        public void addValue(int value, int x, int y)
        {
            SudokuStatus status = new SudokuStatus(createTable(value, x, y), x, y);
            tableStack.Add(status);
        }

        public bool isValidValue(int value, int x, int y)
        {
            int[,] table = getLastTable();

            for (int i = 0; i < size; i++)
            {
                if (table[x, i] == value) return false;
                if (table[i, y] == value) return false;
            }

            for (int i = (x / 3) * 3; i < ((x / 3) + 1) * 3; i++)
            {
                for (int j = (y / 3) * 3; j < ((y / 3) + 1) * 3; j++)
                {
                    if (table[i, j] == value) return false;
                }
            }
            return true;
        }

        public int[] nextRandomCoord()
        {
            /*int[] emptyCell = new int[2];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0)
                    {
                        emptyCell[0] = i;
                        emptyCell[1] = j;
                        return emptyCell;
                    }
                }
            }

            return emptyCell;*/

            int[] coord = new int[2];

            int[,] lastTable = getLastTable();
            Random rand = new Random();

            do
            {
                coord[0] = rand.Next(size);
                coord[1] = rand.Next(size);
            } while (lastTable[coord[0], coord[1]] != 0);

            return coord;
        }

        public int[,] getLastTable()
        {
            return ((SudokuStatus)tableStack[tableStack.Count - 1]).getTable();
        }
    }
}
