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
        public ThreadContext ctx = new ThreadContext();
        public ThreadContext[] ctxs;
        Sudoku[] objects;

        public Threads(int[,] sudoku, int THREAD_COUNT)
        {
            this.sudoku = sudoku;
            this.THREAD_COUNT = THREAD_COUNT;
            threads = new Thread[THREAD_COUNT];
            tasks = new Sudoku[THREAD_COUNT];
            objects = new Sudoku[THREAD_COUNT];
            ctxs = new ThreadContext[THREAD_COUNT];
            Sudoku sudokuSolver = new Sudoku();

            for (int i = 0; i < THREAD_COUNT; i++)
            {
                int copy = i;
                objects[copy] = new Sudoku();
                ctxs[i] = new ThreadContext(sudoku);
                threads[i] = new Thread(() => { ctxs[copy] = objects[copy].solveSudoku((int[,])sudoku.Clone(), copy); });
                //ctx.sudoku = this.sudoku;
                //Thread thread = new Thread(() => { ctx = sudokuSolver.solveSudoku(sudoku); });
            }

            for(int i = 0; i < THREAD_COUNT; i++)
            {
                threads[i].Start();
            }

            /*ctx.sudoku = this.sudoku;
            Thread thread = new Thread(() => { ctx = sudokuSolver.solveSudoku(sudoku); });
            thread.Start();*/
        }

        public ThreadContext[] getContexts()
        {
            bool run = true;

            while (run)
            {
                for (int i = 0; i < THREAD_COUNT; i++)
                {
                    if (!threads[i].IsAlive)
                    {
                        Console.WriteLine("Thread #" + i + " is done");

                        ctxs[i].winner = true;

                        for (int j = 0; j < THREAD_COUNT; j++)
                        {
                            objects[j].KeepGoing = false;
                        }
                        run = false;
                        break;
                    }
                }
            }

            for(int i = 0; i < THREAD_COUNT; i++)
            {
                threads[i].Join();
            }

            return ctxs;
        }
    }
}
