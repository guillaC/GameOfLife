namespace GameOfLife
{
    using System;
    using System.Collections;
    using System.Threading;

    internal class Game
    {
        public Cursor UserCursor;

        public bool Paused { get; set; }

        private const int Width = 69;
        private const int Height = 19;
        private int iteration, surroundingCellsResult, population = 0;
        private CellState[,] world = new CellState[Height, Width];
        private Stack delCoord = new Stack();
        private Stack addCoord = new Stack();

        private enum CellState
        {
            Dead,
            Alive,
            AliveSinceLastIteration
        }

        public Game()
        {
            this.UserCursor = new Cursor(5, 5);

            Thread threadRefresh = new Thread(() =>
            {
                while (true)
                {
                    this.population = 0;
                    ConsoleUpdate();
                    Thread.Sleep(50);
                }
            });

            Thread threadRules = new Thread(() =>
            {
                while (true)
                {
                    if (Paused == false)
                    {
                        ApplyRules();
                    }

                    Thread.Sleep(200);
                }
            });
            threadRules.Start();
            threadRefresh.Start();
        }

        public int GetCellState(int x, int y)
        {
            if (x < Width && x >= 0 && y < Height && y >= 0 && this.world[y, x] == CellState.Alive)
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
            this.world[y, x] = (int)CellState.Dead;
        }

        public void SetCellOn(int x, int y)
        {
            if (this.world[y, x] == CellState.Dead)
            {
                this.world[y, x] = CellState.AliveSinceLastIteration;
            }
            else
            {
                this.world[y, x] = CellState.Alive;
            }
        }

        public void RandomCell()
        {
            Random r = new Random();
            for (int i = 0; i != Height; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    if (r.Next(2) == 0)
                    {
                        this.SetCellOn(j, i);
                    }
                }
            }
        }

        public void ApplyRules()
        {
            for (int i = 0; i != Height; i++)
            {
                for (int j = 0; j != Width; j++)
                {
                    if (this.world[i, j] == CellState.AliveSinceLastIteration)
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
                    // Any live cell with two or three live neighbours lives on to the next generation.

                    if (this.surroundingCellsResult != 2 || this.surroundingCellsResult != 3)
                    {
                        if (this.GetCellState(i, j) == (int)CellState.Dead)
                        { // Any dead cell with exactly three live neighbours becomes a live cell
                            if (this.surroundingCellsResult == 3)
                            {
                                int[] coords = new int[2];
                                coords[0] = i;
                                coords[1] = j;
                                this.addCoord.Push(coords);
                            }
                        }
                        else
                        { // Any live cell with fewer than two live neighbours and any live cell with more than three live neighbours dies
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
                    if (this.UserCursor.PosY == i && this.UserCursor.PosX == j)
                    { // Display the cursor
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    if (this.world[i, j] == CellState.Alive || this.world[i, j] == CellState.AliveSinceLastIteration)
                    { // Count the living cells
                        this.population++;
                    }

                    if (this.world[i, j] == CellState.Alive)
                    { // Alive
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (this.world[i, j] == CellState.AliveSinceLastIteration)
                    { // Alive since last iteration
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    { // Dead
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Console.Write("■");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("X:" + this.UserCursor.PosX + "; Y:" + this.UserCursor.PosY + "; generation: " + this.iteration + " population: " + this.population);
            if (this.Paused)
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