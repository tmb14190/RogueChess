using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RogueChess.Pieces
{
    interface IPiece
    {

        public void Update();
        public void Draw();
        public void Move();
        public Texture2D GetTexture();

    }
}
