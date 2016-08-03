using System;
using System.Threading;

namespace GameOfLife
{
    internal class Game
    {
        public Cursor cursor;
        public Boolean paused = false;
        private int iteration, surroundingCellsResult = 0;
        private int[,] plateau = new int[19, 19];

        public Game()
        {
            this.cursor = new Cursor(5, 5);

            Thread threadRefresh = new Thread(() =>
            {
                while (true)
                {
                    consolUpdate();
                    Thread.Sleep(50);
                }
            });

            Thread threadRules = new Thread(() =>
            {
                while (true)
                {
                    if (paused == false)
                    {
                        applyRules();
                    }
                    Thread.Sleep(200);
                }
            });
            threadRules.Start();
            threadRefresh.Start();
        }

        public void pause()
        {
            if (paused == false)
            {
                this.paused = true;
            }
            else
            {
                this.paused = false;
            }
        }

        public int getCellState(int x, int y)
        {
            if (x < 19 && x > 0 && y < 19 && y > 0)
            {
                return plateau[y, x];
            }
            else
            {
                return 0;
            }
        }

        public void deleteCell(int x, int y)
        {
            plateau[y, x] = 0;
        }

        public void addCell(int x, int y)
        {
            plateau[y, x] = 1;
        }

        public void applyRules()
        {
            System.Diagnostics.Debug.WriteLine("applyRules()");
            for (int i = 0; i != 19; i++)
            {
                for (int j = 0; j != 19; j++)
                {
                    surroundingCellsResult = getCellState(i, j - 1) + getCellState(i, j + 1) + getCellState(i + 1, j) + getCellState(i - 1, j) + getCellState(i - 1, j - 1) + getCellState(i + 1, j + 1) + getCellState(i - 1, j + 1) + getCellState(i + 1, j - 1);
                    //Si une cellule a exactement deux voisines vivantes, elle reste dans son état actuel à l’étape suivante.
                    if (surroundingCellsResult != 2)
                    {
                        if (getCellState(i, j) == 0)
                        { //Si une cellule a exactement trois voisines vivantes, elle est vivante à l’étape suivante.
                            if (surroundingCellsResult == 3)
                            {
                                addCell(i, j);
                            }
                        }
                        else
                        { //Si une cellule a strictement moins de deux ou strictement plus de trois voisines vivantes, elle est morte à l’étape suivante.
                            if (surroundingCellsResult < 2 || surroundingCellsResult > 3)
                            {
                                deleteCell(i, j);
                            }
                        }
                    }
                }
            }
            iteration++;
        }

        private void consolUpdate() //rendering
        {
            System.Diagnostics.Debug.WriteLine("consolUpdate()");
            for (int i = 0; i != 19; i++)
            {
                Console.SetCursorPosition(0, i);
                for (int j = 0; j != 19; j++)
                {
                    if (cursor.posY == i && cursor.posX == j)
                    { //Affiche le curseur de l'utilisateur
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    
                    if (plateau[i, j] == 1)
                    { //Vivante
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    { //Morte
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write("■");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("X:" + cursor.posX + "; Y:" + cursor.posY + "; itération: " + iteration + " ");
            if (paused)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[PAUSE] ");
            }
            else
            {
                Console.Write("        ");
            }
        }
    }
}