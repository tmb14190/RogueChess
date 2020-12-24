﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace RogueChess.Pieces
{
    class Rook : IPiece
    {
        Texture2D texture;
        static string name = "ROOK";
        string colour;
        List<int> moves;
        string moveType;
        List<string> buffs;

        public Rook(ContentManager content, string colour, (int, int) size)
        {
            this.colour = colour;
            moves = new List<int>();
            buffs = new List<string>();
            moveType = "LINEAR";

            string folder = "";
            if (size == (80, 80))
                folder = "80 x 80\\";
            else if (size == (132, 132))
                folder = "132 x 132\\";
            else
                Debug.WriteLine("No piece of that size");

            if (colour == "WHITE")
            {
                this.texture = content.Load<Texture2D>(folder + "White Rook");
            }
            else if (colour == "BLACK")
            {
                this.texture = content.Load<Texture2D>(folder + "Black Rook");
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
            int i = index;
            while (i > 7)
            {
                futureMoves.Add(i - 8);
                i += -8;
            }
            futureMoves.Add(-1);

            // right 
            i = index;
            while (i % 8 != 7)
            {
                futureMoves.Add(i + 1);
                i += 1;
            }
            futureMoves.Add(-1);

            // down
            i = index;
            while (i < 56)
            {
                futureMoves.Add(i + 8);
                i += 8;
            }
            futureMoves.Add(-1);

            // left 
            i = index;
            while (i % 8 != 0)
            {
                futureMoves.Add(i - 1);
                i += -1;
            }
            futureMoves.Add(-1);

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