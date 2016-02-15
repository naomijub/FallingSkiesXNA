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
    public class Balls
    {
        public Vector2 vector { get; set; }
        public int radius { get; set; }
        private GameTime gameTime;
        private int speed;
        public static int range = 0;

        public Balls() {
            Random rg = new Random();
            gameTime = new GameTime();
            radius = 10;
            speed = ((gameTime.TotalGameTime.Milliseconds + rg.Next(1, 11)) % 5) + 1;           
            vector = new Vector2(placeX(), 0);
        }

        public int placeX() {
            Random rg = new Random();
            int x = ((gameTime.TotalGameTime.Milliseconds + range) % 760) + 35;
            range = range + rg.Next(-100, 100);
            if (x < 36) {
                x = ((gameTime.TotalGameTime.Milliseconds) % 760) + 35;
            }
            return x;
        }

        public void moveDown() {
            int auxY =(int)vector.Y + speed;
            int auxX = (int)vector.X;

            vector = new Vector2(auxX, auxY);
 
        }

        public bool isHit(Vector2 aux, int rad) {
            int x = (int)aux.X;
            int y = (int)aux.Y;
            
            int vecX = (int)vector.X;
            int vecY = (int)vector.Y;

            if (((y + rad) >= (vecY - radius)) && ((y - rad) <= (vecY + radius)))
            {
                if (((x + rad) >= (vecX - radius)) && ((x - rad) <= (vecX + radius)))
                {
                    return true;
                }
                if (x == vecX) {
                    return true;
                }
            }
            return false;
        }

        public void reset() {
            speed = (gameTime.TotalGameTime.Milliseconds % 5) + 1;
            vector = new Vector2(placeX(), 0);
        }

        public bool checkGround() {
            if(vector.Y == 480){
                return true;
            }else{
                return false;
            }
        }

    }
}
