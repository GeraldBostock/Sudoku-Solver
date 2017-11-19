using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Helper
    {

        public void writeToFile(ArrayList stack, int id)
        {
            string path = @"res\threads\thread#" + id.ToString() + ".txt";

            for (int x = 0; x < stack.Count; x++)
            {
                SudokuStatus status = (SudokuStatus)stack[x];
                int[,] sudoku = status.getTable();

                String[] lines = new String[9];

                for (int i = 0; i < 9; i++)
                {
                    char[] charLine = new char[9];

                    for (int j = 0; j < 9; j++)
                    {
                        if (sudoku[i, j] == 0) charLine[j] = '*';
                        else charLine[j] = (char)(sudoku[i, j] + 48);
                    }

                    lines[i] = new string(charLine);
                }

                lines[8] += Environment.NewLine;

                if (!File.Exists(path))
                {
                    File.WriteAllLines(path, lines);
                }
                else
                {
                    File.AppendAllLines(path, lines);
                }
            }
        }

        public void writeToFile(int[,] sudoku, int id)
        {
            string path = @"res\threads\thread#" + id.ToString() + ".txt";

            String[] lines = new String[9];

            for (int i = 0; i < 9; i++)
            {
                char[] charLine = new char[9];

                for (int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] == 0) charLine[j] = '*';
                    else charLine[j] = (char)(sudoku[i, j] + 48);
                }

                lines[i] = new string(charLine);
            }

            lines[8] += Environment.NewLine;

            if (!File.Exists(path))
            {
                File.WriteAllLines(path, lines);
            }
            else
            {
                File.AppendAllLines(path, lines);
            }
        }

        public static void deleteThreadFiles()
        {
            for(int i = 0; i < 9; i++)
            {
                String path = @"res\threads\thread#" + i.ToString() + ".txt";

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public int[,] getColor(int[,] sudoku)
        {
            int[,] isRed = new int[9, 9];
            
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (sudoku[i, j] != 0) isRed[i, j] = 0;
                    else isRed[i, j] = 1;
                }
            }

            return isRed;
        }

        public ThreadContext[] putInOrder(ThreadContext[] ctxs)
        {
            for(int i = 0; i < 7; i++)
            {
                if (ctxs[i].winner)
                {
                    if (i == 0) return ctxs;
                    else
                    {
                        ThreadContext temp;
                        temp = ctxs[i];
                        ctxs[i] = ctxs[0];
                        ctxs[0] = temp;
                        return ctxs;
                    }
                }
            }

            return ctxs;
        }

    }
}
