using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku
{
    public delegate void SudokuCompleted(string sudokuID, int threadID, String time, String isDone);

    class Threads
    {
        private int[,] sudoku;
        int THREAD_COUNT;
        Thread[] threads;
        Sudoku[] tasks;
        String[] returns = new String[7];
        String[] threadIDs = new String[7];
        public ThreadContext ctx = new ThreadContext();

        public Threads(int[,] sudoku, int THREAD_COUNT)
        {
            this.sudoku = sudoku;
            this.THREAD_COUNT = THREAD_COUNT;
            threads = new Thread[THREAD_COUNT];
            tasks = new Sudoku[THREAD_COUNT];
            Sudoku sudokuSolver = new Sudoku();

            ctx.sudoku = this.sudoku;
            Thread thread = new Thread(() => { ctx = sudokuSolver.solveSudoku(sudoku); });
            thread.Start();
        }
    }
}
