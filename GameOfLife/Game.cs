using System;
using System.Collections;
using System.Threading;

namespace GameOfLife
{
    internal class Game
    {
        public Cursor cursor;
        private Boolean paused = true;

        private int iteration, surroundingCellsResult = 0;
        private const int width = 69;
        private const int height = 19;
        private int[,] plateau = new int[height, width];

        private Stack delCoord = new Stack();
        private Stack addCoord = new Stack();

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
            if (x < width && x >= 0 && y < height && y >= 0 && plateau[y, x] == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void setCellOff(int x, int y)
        {
            plateau[y, x] = 0;
        }

        public void setCellOn(int x, int y)
        {
            if (plateau[y, x] == 0)
            {
                plateau[y, x] = 2; // on depuis 1 itération
            }
            else
            {
                plateau[y, x] = 1; // on
            }
        }

        public void applyRules()
        {
            for (int i = 0; i != height; i++)
            {
                for (int j = 0; j != width; j++)
                {
                    if (plateau[i, j] == 2)
                    {
                        setCellOn(j, i);
                    }
                }
            }

            for (int i = 0; i != width; i++)
            {
                for (int j = 0; j != height; j++)
                {
                    surroundingCellsResult = getCellState(i, j - 1) + getCellState(i, j + 1) + getCellState(i + 1, j) + getCellState(i - 1, j) + getCellState(i - 1, j - 1) + getCellState(i + 1, j + 1) + getCellState(i - 1, j + 1) + getCellState(i + 1, j - 1);
                    //Si une cellule a exactement deux voisines vivantes, elle reste dans son état actuel à l’étape suivante.
                    if (surroundingCellsResult != 2)
                    {
                        if (getCellState(i, j) == 0)
                        { //Si une cellule a exactement trois voisines vivantes, elle est vivante à l’étape suivante.
                            if (surroundingCellsResult == 3)
                            {
                                int[] coords = new int[2];
                                coords[0] = i;
                                coords[1] = j;
                                addCoord.Push(coords);
                            }
                        }
                        else
                        { //Si une cellule a strictement moins de deux ou strictement plus de trois voisines vivantes, elle est morte à l’étape suivante.
                            if (surroundingCellsResult < 2 || surroundingCellsResult > 3)
                            {
                                int[] coords = new int[2];
                                coords[0] = i;
                                coords[1] = j;
                                delCoord.Push(coords);
                            }
                        }
                    }
                }
            }

            //J'ajoute/delete par la suite certains éléments
            while (addCoord.Count > 0)
            {
                var coords = (int[])addCoord.Pop();
                setCellOn(coords[0], coords[1]);
            }

            while (delCoord.Count > 0)
            {
                var coords = (int[])delCoord.Pop();
                setCellOff(coords[0], coords[1]);
            }

            iteration++;
        }

        private void consolUpdate() //rendering
        {
            for (int i = 0; i != height; i++)
            {
                Console.SetCursorPosition(0, i);
                for (int j = 0; j != width; j++)
                {
                    if (cursor.posY == i && cursor.posX == j)
                    { //Affiche le curseur de l'utilisateur
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    if (plateau[i, j] == 1)
                    { //Vivante
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (plateau[i, j] == 2)
                    { //Vivante depuis peu
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    { //Morte
                        Console.ForegroundColor = ConsoleColor.Gray;
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