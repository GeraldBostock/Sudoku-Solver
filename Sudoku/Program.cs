using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    static class Program
    {
        private static readonly int THREAD_COUNT = 7;
        private static readonly String FILE_NAME = "sudoku.txt";
        private static int[,] sudoku = new int [9, 9];
        private static readonly int SUDOKU_SIZE = 9;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            readSudoku();
            Application.Run(new Form1(sudoku, SUDOKU_SIZE, THREAD_COUNT));
        }

        static void readSudoku()
        {
            string[] lines = File.ReadAllLines(@"res\" + FILE_NAME);

            int j = 0;
            foreach (string line in lines)
            {
                char[] array = line.ToCharArray();
                for(int i = 0; i < SUDOKU_SIZE; i++)
                {
                    if(array[i] == '*')
                    {
                        sudoku[j, i] = 0;
                    }
                    else sudoku[j, i] = array[i] - 48;
                }
                j++;
            }

            for(int i = 0; i < SUDOKU_SIZE; i++)
            {
                for(j = 0; j < SUDOKU_SIZE; j++)
                {
                    Console.Write(sudoku[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
