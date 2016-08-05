namespace GameOfLife
{
    internal class Cursor
    {
        public int PosX;
        public int PosY;

        public Cursor(int x, int y)
        {
            this.PosX = x;
            this.PosY = y;
        }

        public void SetPos(int x, int y)
        {
            this.PosX = x;
            this.PosY = y;
        }
    }
}