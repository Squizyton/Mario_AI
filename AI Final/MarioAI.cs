 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace AI_Final
{
    class MarioAI
    {
        private bool initialized = false;

        private NeuralNetwork net;

        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        public Rectangle rectangle;
        private bool hasJumped = false;
        bool HitGround = false;
        int[,] TheMap;


       

        enum state {StandStill, runleft, runright, jumping }
        state movementState;

        public Vector2 Position
        {
            get { return position; }
        }

        public MarioAI(Vector2 startPosition, int[,] tMap)
        {
            this.position = startPosition;

            TheMap = tMap;

        }

        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/player");
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            if (initialized == true && HitGround == true)
            {
               

                int ViewWidth = 10;
                int ViewHeight = 10;

                float[] inputs = new float[ViewWidth * ViewHeight];

                int MarioPositionX = (int)Math.Round(position.X / 32);
                int MarioPositionY = (int)Math.Round(position.Y / 32);

                for (int x = 0; x < ViewWidth; x++)
                {
                    for (int y = 0; y < ViewHeight; y++)
                    {
                        //Grabs the current Tile position
                        int TileX = x + MarioPositionX - (ViewWidth / 2);
                        int TileY = y + MarioPositionY - (ViewHeight / 2);

                        //Encodes the x and y into a single value
                        int index = y * ViewWidth + x;

                        //Sets it to 0 by default
                        inputs[index] = .1f;

                        //If outside the array skip this iteration
                        if (TileX < 0 || TileY < 0 || TileX >= TheMap.GetLength(0) || TileY >= TheMap.GetLength(1))
                        {
                            continue;
                        }

                        //If on the map there is not a 0, set it to solid
                        if (TheMap[TileX, TileY] != 0)
                            inputs[index] = 1;
                        // if (inputs[index] != 0)
                        //  Console.WriteLine(index);
                    }
                }



                float[] output = net.FeedForward(inputs);

                float GoLeft = output[0];
                float GoRight = output[1];
                float Jump = output[2];
                float StandStill = output[3];

                if (GoLeft > GoRight)
                {
                    movementState = state.runleft;

                }
                else if (GoRight > GoLeft)

                {
                    movementState = state.runright;


                } else if (StandStill > GoRight || StandStill > GoLeft)
                {
                    movementState = state.StandStill;
                }

                if (Jump > .5f)
                {
                    if (hasJumped == false)
                    {
                        position.Y -= 5f;
                        velocity.Y = -9f;
                        hasJumped = true;
                    }
                }
                net.SetFitness(position.X);

                Movement();

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                    Console.WriteLine("[{0}]", string.Join(", ", output));
            }






            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width * 2, texture.Height * 2);


            if (velocity.Y < 10)
            {
                velocity.Y += 0.4f;
            }

        }

        public void Movement()
        {
            switch (movementState)
            {
                case state.runleft:

                    velocity.X = -4;

                    break;

                case state.runright:
                    velocity.X = 4;

                    break;
                         case state.StandStill:

                    break;
            }
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }


        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {

            if (rectangle.TouchTopOf(newRectangle))
            {
                HitGround = true;
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if (rectangle.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - rectangle.Width - 2;
            }

            if (rectangle.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }

            if (rectangle.TouchBottomOf(newRectangle))
            {
                velocity.Y = 1f;
            }

            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - rectangle.Width) position.X = xOffset - rectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            //if (position.Y > yOffset - rectangle.Height) position.Y = yOffset - rectangle.Height;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public void Init(NeuralNetwork net, Vector2 startPosition)
        {
            this.position = startPosition;
            this.net = net;
            initialized = true;
        }

    }
}
