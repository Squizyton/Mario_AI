using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace AI_Final
{
    class Map
    {
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        private List<Goomba> listGoomba = new List<Goomba>();
        public List<CollisionTiles> CollisionTiles
        {
            get { return collisionTiles; }
        }


        public List<Goomba> ListGoombas
        {
            get { return listGoomba; }
        }
        private int width, height;
        public int Width
        {
            get { return width; }
        }


        public int Height
        {
            get { return height; }
        }



        public Map() { }

        public void Generate(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];
                   
                    //Ignores Number 3
                    if (number == 3)
                    {
                        number = 0;
                    }
                    if (number > 0)
                    {
                        CollisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));
                    }
                    if (number > 12)
                    {
                        CollisionTiles.Add(new CollisionTiles(number, new Rectangle(2, 16, size, size)));
                    }



                    width = (x + 1) * size;
                    height = (y + 1) * size;


                }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (CollisionTiles tile in collisionTiles)
            {
                tile.Draw(spriteBatch);
            }
        }

    }
}
