using System;
using System.Collections.Generic;
using System.Linq;

using Models;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FallingSkies
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int vel = 5;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D cannon, blueBall, redBall, greenBall, bullet;

        float cannonAngle, bulletAngle;
        int nroBullets, score;
        bool hasShot, isMoving;
        Vector2 cannonRot, cannonVec, cannonEnd, bulletVec, bulletPos;
        Vector2[] balls;

        Cannon cannonModel;
        Bullet bulletModel;
        Balls[] ballsModel;

        MouseState mouseState;
        KeyboardState kbState, prevKbState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            cannonModel = new Cannon();
            bulletModel = new Bullet(vel);
            ballsModel = new Balls[3];

            balls = new Vector2[3];

            nroBullets = 0;
            score = 0;
            hasShot = false;
            isMoving = false;
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cannon = Content.Load<Texture2D>("Images/cannon");
            //Console.WriteLine(cannon.Width + " " + cannon.Height);
            blueBall = Content.Load<Texture2D>("Images/blueBall"); 
            redBall = Content.Load<Texture2D>("Images/redBall");
            greenBall = Content.Load<Texture2D>("Images/greenBall");
            bullet = Content.Load<Texture2D>("Images/bullet");

            ballsModel[0] = new Balls();
            ballsModel[1] = new Balls();
            ballsModel[2] = new Balls();

            cannonRot = new Vector2(33, 34);
            cannonVec = new Vector2(33, 446);
            bulletVec = new Vector2(bullet.Width, bullet.Height) / 2;
            bulletPos = new Vector2(bullet.Width, bullet.Height) / 2;

            mouseState = Mouse.GetState();
            cannonAngle = cannonModel.angle(mouseState.X, mouseState.Y);
            kbState = Keyboard.GetState();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mouseState = Mouse.GetState();
            cannonAngle = cannonModel.angle(mouseState.X, mouseState.Y);
            cannonEnd = cannonModel.cannonEnd(cannonAngle); 

            prevKbState = kbState;
            kbState = Keyboard.GetState();

            hasShot = cannonModel.hasShot(kbState, prevKbState);
            updateBullet(); checkBullet();
            moveBalls();  updateBalls();
            checkHit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(cannon, cannonVec, null, Color.White, cannonAngle, cannonRot, 1.0f, SpriteEffects.None, 0);
            drawShot(spriteBatch);
            drawBalls(spriteBatch);
            drawScore(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        //LoadContent
        public void initializeBalls() {
            
            for (int i = 0; i < 3; i++)
            {
                float x = ballsModel[i].vector.X;
                float y = ballsModel[i].vector.Y;

                balls[i] = new Vector2(x, y); 

            }
        }

        //update
        public void updateBullet() {
            Vector2 steps = bulletModel.steps(bulletAngle);
            Vector2 aux = new Vector2((bulletPos.X + steps.X), (bulletPos.Y + steps.Y));
            bulletPos = aux;
        }

        public void checkBullet() {
            if (bulletModel.checkBullet(bulletPos)) {
                nroBullets = 0;
            }
        }

        public void checkHit() {
            for (int i = 0; i < 3; i++) {
                if (ballsModel[i].isHit(bulletPos, 8)) {
                    score++;
                    ballsModel[i].reset();
                }
                if (ballsModel[i].checkGround()) {
                    score--;
                    ballsModel[i].reset();
                }
            }
        }

        public void moveBalls() {
            ballsModel[0].moveDown();
            ballsModel[1].moveDown();
            ballsModel[2].moveDown();
        }

        public void updateBalls() {
            
            for (int i = 0; i < 3; i++)
            {
                float x = ballsModel[i].vector.X;
                float y = ballsModel[i].vector.Y;

                balls[i] = new Vector2(x, y); ;

            }
        }

        //draw
        private void drawShot(SpriteBatch sb) 
        {
            if (hasShot)// && nroBullets == 0)
            { 
                bulletAngle = cannonAngle;
                bulletPos = cannonEnd;
                spriteBatch.Draw(bullet, bulletPos, null, Color.White, bulletAngle, bulletVec, 1.0f, SpriteEffects.None, 0);
                MediaPlayer.Play(Content.Load<Song>("Sounds/shoot"));
                nroBullets = 1;
                isMoving = true;
                hasShot = false;
            }
            if(isMoving){
                spriteBatch.Draw(bullet, bulletPos, null, Color.White, bulletAngle, bulletVec, 1.0f, SpriteEffects.None, 0);
            }
        }

        public void drawBalls(SpriteBatch sb)
        {

            sb.Draw(greenBall, balls[0], Color.White);
            sb.Draw(redBall, balls[1], Color.White);
            sb.Draw(blueBall, balls[2], Color.White);
        }

        public void drawScore(SpriteBatch sb) {
            String str = " Your score is: " + score;
            Vector2 position = new Vector2(400, 455);
            SpriteFont font = Content.Load<SpriteFont>("scoreFont");
            sb.DrawString(font, str, position, Color.Black);
        }
    }
}
