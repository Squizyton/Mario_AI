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
    class CollisionTiles : Tiles

    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {
            ///1 - Ground Tile
            ///
            texture = Content.Load<Texture2D>("Tiles/"+i);
            this.Rectangle = newRectangle;

        }
    }
}

    

