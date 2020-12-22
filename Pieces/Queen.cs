using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RogueChess.Pieces
{
    class Queen : IPiece
    {
        Texture2D texture;
        static string name = "Queen";
        string colour;
        List<int> moves;
        string moveType;

        public Queen(ContentManager content, string colour, (int, int) size)
        {
            this.colour = colour;
            moves = new List<int>();
            moveType = "recursive";

            string folder = "";
            if (size == (80, 80))
                folder = "80 x 80\\";
            else if (size == (132, 132))
                folder = "132 x 132\\";
            else
                Debug.WriteLine("No piece of that size");

            if (colour == "WHITE")
            {
                this.texture = content.Load<Texture2D>(folder + "White Queen");
            }
            else if (colour == "BLACK")
            {
                this.texture = content.Load<Texture2D>(folder + "Black Queen");
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
            int i = index;
            while (i > 7)
            {
                futureMoves.Add(i - 8);
                i += -8;
            }
            i = index;
            while (i < 56)
            {
                futureMoves.Add(i + 8);
                i += 8;
            }

            // left side
            i = index;
            while (i % 8 != 0)
            {
                futureMoves.Add(i - 1);
                i += -1;
            }

            // right side
            i = index;
            while (i % 8 != 7)
            {
                futureMoves.Add(i + 1);
                i += 1;
            }

            // left side
            i = index;
            while (i % 8 != 0 && i > 7)
            {
                futureMoves.Add(i - 9);
                i += -9;
            }

            i = index;
            while (i % 8 != 0 && i < 56)
            {
                futureMoves.Add(i + 7);
                i += 7;
            }

            // right side
            i = index;
            while (i % 8 != 7 && i > 7)
            {
                futureMoves.Add(i - 7);
                i += -7;
            }

            i = index;
            while (i % 8 != 7 && i < 56)
            {
                futureMoves.Add(i + 9);
                i += 9;
            }

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