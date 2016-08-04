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
                        game.cursor.setPos(game.cursor.posX, game.cursor.posY - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        game.cursor.setPos(game.cursor.posX, game.cursor.posY + 1);
                        break;

                    case ConsoleKey.LeftArrow:
                        game.cursor.setPos(game.cursor.posX - 1, game.cursor.posY);
                        break;

                    case ConsoleKey.RightArrow:
                        game.cursor.setPos(game.cursor.posX + 1, game.cursor.posY);
                        break;

                    case ConsoleKey.Spacebar:
                        game.setCellOn(game.cursor.posX, game.cursor.posY);
                        break;

                    case ConsoleKey.Enter:
                        game.pause();
                        break;
                }
            }
        }
    }
}