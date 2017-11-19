using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SudokuStatus
    {
        private int[,] table;
        private int xCoord;
        private int yCoord;

        public SudokuStatus(int[,] table, int xCoord, int yCoord)
        {
            this.table = table;
            this.xCoord = xCoord;
            this.yCoord = yCoord;
        }

        public int[,] getTable()
        {
            return table;
        }

        public int getxCoord()
        {
            return xCoord;
        }

        public int getyCoord()
        {
            return yCoord;
        }
    }
}
