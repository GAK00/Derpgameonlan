using System;
namespace DerpGame.Model
{
    [Serializable()]
    public class Point
    {
        public int x;
        public int y;
        public float theta;
        public int id;

        public Point(int x, int y, float theta, int id)
        {

            this.x = x;
            this.y = y;
            this.theta = theta;
            this.id = id;
        }
    }
}
