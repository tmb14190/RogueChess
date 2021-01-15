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
    [Serializable]
    class Elephant : IPiece
    {
        Texture2D texture;
        static string name = "ELEPHANT";
        string colour;
        List<int> moves;
        string moveType;
        List<string> buffs;

        public Elephant(ContentManager content, string colour, (int, int) size)
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
                this.texture = content.Load<Texture2D>("Elephant Knight");
            }
            else if (colour == "BLACK")
            {
                this.texture = content.Load<Texture2D>("Elephant Knight");
            }
            else
            {
                Debug.WriteLine("Piece colour neither black or white");
            }
        }
        public Elephant(string colour, List<int> moves, string moveType, List<string> buffs)
        {
            this.texture = null;
            this.colour = colour;
            this.moves = moves;
            this.moveType = moveType;
            this.buffs = buffs;
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
            if (index % 8 != 7 && index % 8 != 6)
                futureMoves.Add(index - 14);
            if (index % 8 != 0 && index % 8 != 1)
                futureMoves.Add(index - 18);

            // down
            if (index % 8 != 0 && index % 8 != 1)
                futureMoves.Add(index + 14);
            if (index % 8 != 7 && index % 8 != 6)
                futureMoves.Add(index + 18);

            foreach (int move in futureMoves.ToList())
            {
                if (move < 0 || move > 63)
                    futureMoves.Remove(move);
            }

            return futureMoves;
        }
        public Elephant(Texture2D texture, string colour, List<int> moves, string moveType, List<string> buffs)
        {
            this.texture = texture;
            this.colour = colour;
            this.moves = moves;
            this.moveType = moveType;
            this.buffs = buffs;
        }

        public Elephant Clone()
        {
            return new Elephant(texture, colour, moves, moveType, buffs);

        }
        public void AddBuff(string buff)
        {
            buffs.Add(buff);
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