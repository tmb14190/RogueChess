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
        static string name = "King";
        string colour;
        List<int> moves;
        string moveType;

        public King(ContentManager content, string colour, (int, int) size)
        {
            this.colour = colour;
            moves = new List<int>();
            moveType = "single";

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

        public void AddMove(int move)
        {
            moves.Add(move);
        }
        public List<int> AllowedMoves(int index)
        {
            List<int> futureMoves = new List<int>();

            // vertical 
            if (index > 7)
                futureMoves.Add(index - 8);
            if (index < 56)
                futureMoves.Add(index + 8);

            // left side
            if (index % 8 != 0)
            {
                if (index > 7)
                    futureMoves.Add(index - 9);
                if (index < 56)
                    futureMoves.Add(index + 7);
                futureMoves.Add(index - 1);
            }

            // right side
            if (index % 8 != 7)
            {
                futureMoves.Add(index + 1);
                if (index > 7)
                    futureMoves.Add(index - 7);
                if (index < 56)
                    futureMoves.Add(index + 9);
            }

            // Castle

            return futureMoves;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }
        public string GetName()
        {
            return name;
        }

        public string GetColour()
        {
            return colour;
        }
        public string GetMoveType()
        {
            return moveType;
        }
    }
}