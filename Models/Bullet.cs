using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Models
{
    public class Bullet
    {
        private int speed;
        private double acceleration;
        public static int timing;

        public Bullet(int speed)
        {
            this.speed = speed;
            timing = 0;
            acceleration = 0.0005;
        }

        public Vector2 steps(float angle) {
            timing++;
            double adj = speed * Math.Cos(angle);
            double op = speed * Math.Sin(angle);

            int adjAux = (int)adj;
            int opAux = (int)(op + (acceleration * timing));

            Vector2 aux = new Vector2(adjAux, opAux);
            return aux;
        }

        public bool checkBullet(Vector2 aux) {
            if (aux.X > 800 || aux.Y < 1)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
