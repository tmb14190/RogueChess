using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace RogueChess.Pieces
{
    public interface IPiece 
    {
        /*
         * Keep running total of pieces moved to, when piece initiliased to a square that starting square is their first move
         */
        public void AddMove(int move);

        /*
         * Indivdual pieces are liable to figure all possible moves on the board. Board Class is the one to block moves when other pieces get in the way
         * When multiple move directions are returned moves are returned in clockwise order. E.g. a Queen first top line mvoes, then top right etc. Bishop first top right. Moves seperated by -1s.
         */
        public List<int> AllowedMoves(int index);

        /*
         * Add a string with the buff key classifier to the string list of buffs
         */
        public void AddBuff(string buff);

        /*
         * Get the string list of buffs
         */
        public List<string> GetBuffs();

        /*
         * Get the texture for the peice
         */
        public Texture2D GetTexture();

        /*
         * Get the piece name
         */
        public string GetName();

        /*
         * Get the piece colour
         */
        public string GetColour();

        /*
         * If piece colour matches input variable return true
         */
        public bool MatchColour(string colour);

        /*
         * Get piece move type
         */
        public string GetMoveType();

        /*
         * Get list of moves
         */
        public List<int> GetMoveHistory();


    }
}
