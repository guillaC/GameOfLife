namespace GameOfLife
{
    internal class Cursor
    {
        public int posX;
        public int posY;

        public Cursor(int x, int y)
        {
            this.posX = x;
            this.posY = y;
        }

        public void setPos(int x, int y)
        {
            this.posX = x;
            this.posY = y;
        }
    }
}