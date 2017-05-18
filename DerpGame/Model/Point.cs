using System;
namespace DerpGame.Model
{
    [Serializable()]
    public class SPoint
    {
        public float x;
        public float y;
        public float theta;
        public int id;

        public SPoint(float x, float y, float theta, int id)
        {

            this.x = x;
            this.y = y;
            this.theta = theta;
            this.id = id;
        }
    }
}
