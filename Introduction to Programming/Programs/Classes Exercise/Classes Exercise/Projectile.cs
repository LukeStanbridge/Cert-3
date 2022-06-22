using System;
using System.Collections.Generic;
using System.Text;

namespace Classes_Exercise
{
    class Projectile
    {
        public float speed;
        public Vector2 position = new Vector2();
        public Vector2 direction = new Vector2();

        public Projectile(Vector2 pos, Vector2 dir)
        {
            position = pos;
            direction = dir;
        }
        
        public void Launch()
        {
            position = new Vector2((position.xPosition + direction.xPosition * speed) + (position.yPosition + direction.yPosition * speed));
        }

    }
}
