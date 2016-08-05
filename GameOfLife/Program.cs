namespace GameOfLife
{
    using System;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Game game = new Game();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        game.UserCursor.SetPos(game.UserCursor.PosX, game.UserCursor.PosY - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        game.UserCursor.SetPos(game.UserCursor.PosX, game.UserCursor.PosY + 1);
                        break;

                    case ConsoleKey.LeftArrow:
                        game.UserCursor.SetPos(game.UserCursor.PosX - 1, game.UserCursor.PosY);
                        break;

                    case ConsoleKey.RightArrow:
                        game.UserCursor.SetPos(game.UserCursor.PosX + 1, game.UserCursor.PosY);
                        break;

                    case ConsoleKey.Spacebar:
                        game.SetCellOn(game.UserCursor.PosX, game.UserCursor.PosY);
                        break;

                    case ConsoleKey.R:
                        game.RandomCell();
                        break;

                    case ConsoleKey.Enter:
                        game.Paused = !game.Paused;
                        break;
                }
            }
        }
    }
}