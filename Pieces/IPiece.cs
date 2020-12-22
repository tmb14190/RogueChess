using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RogueChess.Pieces
{
    interface IPiece
    {

        public void AddMove(int move);
        public List<int> AllowedMoves(int index);
        public Texture2D GetTexture();
        public string GetName();
        public string GetColour();

        public string GetMoveType();

    }
}
