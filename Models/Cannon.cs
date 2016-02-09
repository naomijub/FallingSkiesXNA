using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Models
{
    public class Cannon
    {
        int rotX, rotY;

        public Cannon() {
            rotX = 33;
            rotY = 446;
        }

        public float angle(int x, int y) {
            float angle = 0f;
            double opposite = y - rotY;
            double adjacent = x - rotX;

            angle = (float)Math.Atan2(opposite, adjacent);

            return angle;
        }

        public Vector2 cannonEnd(float angle) {
            double adj = Math.Abs(108 * Math.Cos(angle));
            double op = Math.Abs(108 * Math.Sin(angle));

            int adjAux = (int)adj + rotX;
            int opAux = 446 - (int)op;

            Vector2 aux = new Vector2(adjAux, opAux);
            return aux;
        }

        public bool hasShot(KeyboardState kbState, KeyboardState prevKbState) {

            if (kbState.IsKeyDown(Keys.Space) && prevKbState.IsKeyUp(Keys.Space))
            {
                return true;
            }
            else {
                return false;
            }
        }


    }
}
