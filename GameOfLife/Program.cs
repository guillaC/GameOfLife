using System;

namespace GameOfLife
{
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
                        game.UserCursor.setPos(game.UserCursor.posX, game.UserCursor.posY - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        game.UserCursor.setPos(game.UserCursor.posX, game.UserCursor.posY + 1);
                        break;

                    case ConsoleKey.LeftArrow:
                        game.UserCursor.setPos(game.UserCursor.posX - 1, game.UserCursor.posY);
                        break;

                    case ConsoleKey.RightArrow:
                        game.UserCursor.setPos(game.UserCursor.posX + 1, game.UserCursor.posY);
                        break;

                    case ConsoleKey.Spacebar:
                        game.SetCellOn(game.UserCursor.posX, game.UserCursor.posY);
                        break;

                    case ConsoleKey.Enter:
                        game.Paused = !game.Paused;
                        break;
                }
            }
        }
    }
}