using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class ThreadContext
    {
        public int ThreadID { get; set; }
        public int[,] sudoku { get; set; }
        public TimeSpan time { get; set; }

        public ThreadContext(int[,] sudoku)
        {
            this.sudoku = sudoku;
        }

        public ThreadContext()
        {

        }
    }
}
