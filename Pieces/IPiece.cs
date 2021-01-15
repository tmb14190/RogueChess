using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RogueChess.Pieces
{
    public interface IPiece 
    {
        /*
         * 
         */
        public void AddMove(int move);

        /*
         * Indivdual pieces are liable to figure all possible moves on the board. Board Class is the one to block moves when other pieces get in the way
         * When multiple move directions are returned moves are returned in clockwise order. E.g. a Queen first top line mvoes, then top right etc. Bishop first top right. Moves seperated by -1s.
         */
        public List<int> AllowedMoves(int index);

        /*
         * 
         */
        public void AddBuff(string buff);

        /*
         * 
         */
        public List<string> GetBuffs();

        /*
         * 
         */
        public Texture2D GetTexture();

        /*
         * 
         */
        public string GetName();

        /*
         * 
         */
        public string GetColour();

        /*
         * 
         */
        public bool MatchColour(string colour);

        /*
         * 
         */
        public string GetMoveType();

        /*
         * 
         */
        public List<int> GetMoveHistory();


    }
}

public interface ICloneable<T>
{
    T Clone();
}
