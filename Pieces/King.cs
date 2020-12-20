using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RogueChess.Pieces
{
    class King : IPiece
    {
        Texture2D texture;

        public King(ContentManager content, string colour, (int, int) size)
        {
            string folder = "";
            if (size == (80, 80))
                folder = "80 x 80\\";
            else if (size == (132, 132))
                folder = "132 x 132\\";
            else
                Debug.WriteLine("No piece of that size");

            if (colour == "WHITE")
            {
                this.texture = content.Load<Texture2D>(folder + "White King");
            }
            else if (colour == "BLACK")
            {
                this.texture = content.Load<Texture2D>(folder + "Black King");
            }
            else
            {
                Debug.WriteLine("Piece colour neither black or white");
            }
        }

        public void Update()
        {

        }
        public void Draw()
        {

        }
        public void Move()
        {

        }

        public Texture2D GetTexture()
        {
            return texture;
        }

    }
}