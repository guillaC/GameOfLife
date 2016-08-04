namespace GameOfLife
{
    using System;
    using System.Collections;
    using System.Threading;

    internal class Game
    {
        public Cursor UserCursor;

        private const int Width = 69;
        private const int Height = 19;

        private bool paused = true;

        private int iteration, surroundingCellsResult = 0;
        private int[,] plateau = new int[Height, Width];

        private Stack delCoord = new Stack();
        private Stack addCoord = new Stack();

        public Game()
        {
            this.UserCursor = new Cursor(5, 5);

            Thread threadRefresh = new Thread(() =>
            {
                while (true)
                {
                    ConsoleUpdate();
                    Thread.Sleep(50);
                }
            });

            Thread threadRules = new Thread(() =>
            {
                while (true)
                {
                    if (paused == false)
                    {
                        ApplyRules();
                    }

                    Thread.Sleep(200);
                }
            });
            threadRules.Start();
            threadRefresh.Start();
        }

        private enum CellState
        {
            Dead,
            Alive,
            AliveSinceLastIteration
        }

        public void Pause()
        {
            if (this.paused == false)
            {
                this.paused = true;
            }
            else
            {
                this.paused = false;
            }
        }

        public int GetCellState(int x, int y)
        {
            if (x < Width && x >= 0 && y < Height && y >= 0 && this.plateau[y, x] == (int)CellState.Alive)
            {
                return (int)CellState.Alive;
            }
            else
            {
                return (int)CellState.Dead;
            }
        }

        public void SetCellOff(int x, int y)
        {
            this.plateau[y, x] = (int)CellState.Dead;
        }

        public void SetCellOn(int x, int y)
        {
            if (this.plateau[y, x] == (int)CellState.Dead)
            {
                this.plateau[y, x] = (int)CellState.AliveSinceLastIteration;
            }
            else
            {
                this.plateau[y, x] = (int)CellState.Alive;
            }
        }

        public void ApplyRules()
        {
            for (int i = 0; i != Height; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    if (this.plateau[i, j] == (int)CellState.AliveSinceLastIteration)
                    {
                        this.SetCellOn(j, i);
                    }
                }
            }

            for (int i = 0; i != Width; i++)
            {
                for (int j = 0; j != Height; j++)
                {
                    this.surroundingCellsResult = this.GetCellState(i, j - 1) + this.GetCellState(i, j + 1) + this.GetCellState(i + 1, j) + this.GetCellState(i - 1, j) + this.GetCellState(i - 1, j - 1) + this.GetCellState(i + 1, j + 1) + this.GetCellState(i - 1, j + 1) + this.GetCellState(i + 1, j - 1);
                    //// Si une cellule a exactement deux voisines vivantes, elle reste dans son état actuel à l’étape suivante.

                    if (this.surroundingCellsResult != 2)
                    {
                        if (this.GetCellState(i, j) == (int)CellState.Dead)
                        { // Si une cellule a exactement trois voisines vivantes, elle est vivante à l’étape suivante.
                            if (this.surroundingCellsResult == 3)
                            {
                                int[] coords = new int[2];
                                coords[0] = i;
                                coords[1] = j;
                                this.addCoord.Push(coords);
                            }
                        }
                        else
                        { // Si une cellule a strictement moins de deux ou strictement plus de trois voisines vivantes, elle est morte à l’étape suivante.
                            if (this.surroundingCellsResult < 2 || this.surroundingCellsResult > 3)
                            {
                                int[] coords = new int[2];
                                coords[0] = i;
                                coords[1] = j;
                                this.delCoord.Push(coords);
                            }
                        }
                    }
                }
            }

            // J'ajoute/delete par la suite certains éléments
            while (this.addCoord.Count > 0)
            {
                var coords = (int[])this.addCoord.Pop();
                this.SetCellOn(coords[0], coords[1]);
            }

            while (this.delCoord.Count > 0)
            {
                var coords = (int[])this.delCoord.Pop();
                this.SetCellOff(coords[0], coords[1]);
            }

            this.iteration++;
        }

        private void ConsoleUpdate() // rendering
        {
            for (int i = 0; i != Height; i++)
            {
                Console.SetCursorPosition(0, i);
                for (int j = 0; j != Width; j++)
                {
                    if (this.UserCursor.posY == i && this.UserCursor.posX == j)
                    { // Affiche le curseur de l'utilisateur
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    if (this.plateau[i, j] == (int)CellState.Alive)
                    { // Vivante
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (this.plateau[i, j] == (int)CellState.AliveSinceLastIteration)
                    { // Vivante depuis peu
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    { // Morte
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Console.Write("■");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("X:" + this.UserCursor.posX + "; Y:" + this.UserCursor.posY + "; itération: " + this.iteration + " ");
            if (this.paused)
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