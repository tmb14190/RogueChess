using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RogueChess.Pieces
{
    class Knight : IPiece
    {
        Texture2D texture;
        static string name = "KNIGHT";
        string colour;
        List<int> moves;
        string moveType;
        List<string> buffs;

        public Knight(ContentManager content, string colour, (int, int) size)
        {
            this.colour = colour;
            moves = new List<int>();
            buffs = new List<string>();
            moveType = "SINGULAR";

            string folder = "";
            if (size == (80, 80))
                folder = "80 x 80\\";
            else if (size == (132, 132))
                folder = "132 x 132\\";
            else
                Debug.WriteLine("No piece of that size");

            if (colour == "WHITE")
            {
                this.texture = content.Load<Texture2D>(folder + "White Knight");
            }
            else if (colour == "BLACK")
            {
                this.texture = content.Load<Texture2D>(folder + "Black Knight");
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
        public List<int> GetMoveHistory()
        {
            return moves;
        }
        public List<int> AllowedMoves(int index)
        {
            List<int> futureMoves = new List<int>();

            // up
            if (index % 8 != 7 && index > 16)
                futureMoves.Add(index - 15);
            if (index % 8 != 0 && index > 16)
                futureMoves.Add(index - 17);

            // left
            if (index % 8 != 0 && index % 8 != 1)
            {
                if (index > 7)
                    futureMoves.Add(index - 10);
                if (index < 56)
                    futureMoves.Add(index + 6);
            }

            // right
            if (index % 8 != 7 && index % 8 != 6)
            {
                if (index > 7)
                    futureMoves.Add(index - 6);
                if (index < 56)
                    futureMoves.Add(index + 10);
            }

            // down
            if (index % 8 != 0 && index < 48)
                futureMoves.Add(index + 15);
            if (index % 8 != 7 && index < 48)
                futureMoves.Add(index + 17);

            return futureMoves;
        }
        public void ApplyBuff(string buff)
        {
            if (buff == "CASTLE")
                buffs.Add("CASTLE");
        }

        public List<string> GetBuffs()
        {
            return buffs;
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
        public bool MatchColour(string colour)
        {
            return this.colour == colour;
        }
        public string GetMoveType()
        {
            return moveType;
        }
    }
}