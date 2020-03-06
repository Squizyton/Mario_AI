using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AI_Final
{
    class Goomba
    {
        private Texture2D texture;

        private Vector2 position = new Vector2(577, 483);
       public Vector2 velocity;
        public Rectangle rectangle;


        public Vector2 Position
        {
            get { return position; }
        }

        public Goomba()
        {
            velocity.X = -3;
        }

        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Sprites/goomba");

        }


        public void Update(GameTime gameTime)
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width * 2, texture.Height * 2);

            Input(gameTime);

            if (velocity.Y < 10)
            {
                velocity.Y += 0.4f;
            }
        }

        private void Input(GameTime gameTime)
        {
          

        }
        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {

            if (rectangle.TouchTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
               
            }

            if (rectangle.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - rectangle.Width - 2;
            }

            if (rectangle.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }

     
        }

        public void Dead(Texture2D deadTexture)
        {
            texture = deadTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

    }
}
