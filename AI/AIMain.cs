using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using System.Linq;
using System.Collections;
using System.Threading;

namespace RogueChess.AI
{
    public static class AIMain
    {

        public static int[] GetMove(string colour, IPiece[] boardPieces, int[] lastMove)
        {
            int[] move = new int[2];

            IDictionary<int, List<int>> allMoves = new Dictionary<int, List<int>>();

            for (int i = 0; i <= 63; i++)
            {
                if (boardPieces[i]?.GetColour() == colour)
                {
                    List<int> moves = Rules.LegalMoves(i, boardPieces[i], boardPieces, lastMove);
                    if (moves.Count > 0)
                        allMoves.Add(i, moves);
                }
            }

            // now pick a random piece and a random move
            var rand = new Random();
            int rando = rand.Next(0, allMoves.Count - 1);

            List<int> value = allMoves.Values.ElementAt(rando);
            int key = allMoves.Keys.ElementAt(rando);

            rando = rand.Next(0, value.Count - 1);

            move[0] = key;
            move[1] = value[rando];

            return move;
        }

    }
}
