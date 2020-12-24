using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Linq;

namespace RogueChess.Pieces
{
    class Pawn : IPiece
    {
        Texture2D texture;
        static string name = "PAWN";
        string colour;
        List<int> moves;
        string moveType;
        List<string> buffs;

        public Pawn(ContentManager content, string colour, (int,int) size)
        {
            this.colour = colour;
            moves = new List<int>();
            buffs = new List<string>();
            moveType = "PAWN";

            string folder = "";
            if (size == (80, 80))
                folder = "80 x 80\\";
            else if (size == (132, 132))
                folder = "132 x 132\\";
            else
                Debug.WriteLine("No piece of that size");

            if (colour == "WHITE")
            {
                this.texture = content.Load<Texture2D>(folder + "White Pawn");
            } else if (colour == "BLACK")
            {
                this.texture = content.Load<Texture2D>(folder + "Black Pawn");
            } else
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

            if (this.colour == "BLACK")
                futureMoves.Add(index + 8);
            else if (this.colour == "WHITE")
                futureMoves.Add(index - 8);

            if (moves.Count == 1)
            {
                if (this.colour == "BLACK")
                    futureMoves.Add(index + 16);
                else if (this.colour == "WHITE")
                    futureMoves.Add(index - 16);
            }

            foreach (int move in futureMoves.ToList())
            {
                if (move < 0 || move > 63)
                    futureMoves.Remove(move);
            }

            return futureMoves;
        }
        public void ApplyBuff(string buff)
        {

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
