using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using System.Linq;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RogueChess.AI
{
    public static class AIMain
    {

        public static int[] GetMove(string colour, Board board, int[] lastMove)
        {
            int[] move = new int[2];
            IPiece[] boardPieces = board.boardPieces;

            IDictionary<int, List<int>> allMoves = GetCurrentPossibleMoves(board.boardPieces, colour, lastMove);

            List<int> scores = new List<int>();

            // check to see if any piece can be taken, and if so, take the first move we find
            bool moveFound = false;
            int bestMove = 0;
            // loop through instance to find first move that can take a piece
            foreach (KeyValuePair<int, List<int>> entry in allMoves)
            {
                // do something with entry.Value or entry.Key
                foreach (int m in entry.Value)
                {
                    int newMove = OneMoveEvaluation(boardPieces, entry.Key, m);
                    if (newMove > bestMove)
                    {
                        move[0] = entry.Key;
                        move[1] = m;
                        moveFound = true;
                        bestMove = newMove;
                    }
                }
            }

            // else now pick a random piece and a random move
            if (!moveFound)
            {
                var rand = new Random();

                int rando = rand.Next(0, allMoves.Count - 1);

                List<int> value = allMoves.Values.ElementAt(rando);
                int key = allMoves.Keys.ElementAt(rando);

                rando = rand.Next(0, value.Count - 1);

                move[0] = key;
                move[1] = value[rando];
            }


            return move;
        }

        /* 
         * Returns map where key = position of piece, and value is the set of moves they can make
         */
        public static IDictionary<int, List<int>> GetCurrentPossibleMoves(IPiece[] boardPieces, string colour, int[] lastMove)
        {
            IDictionary<int, List<int>> allMoves = new Dictionary<int, List<int>>();

            for (int i = 0; i <= 63; i++)
            {
                if (boardPieces[i]?.GetColour() == colour)
                {
                    List<int> moves = Rules.LegalMoves(i, boardPieces[i], boardPieces, lastMove, false);
                    if (moves.Count > 0)
                        allMoves.Add(i, moves);
                }
            }

            return allMoves;
        }

        public static void BuildTree(IPiece[] boardPieces, int depth)
        {

        }

        public static int EvaluateBoardDifference()
        {
            return 0;
        }

        public static int OneMoveEvaluation(IPiece[] boardPieces, int origin, int destination)
        {
            int numPiecesOld = boardPieces.Count(s => s != null);

            IPiece[] newPieces = SimulateMovePiece(boardPieces, origin, destination);

            int numPiecesNew = newPieces.Count(s => s != null);
            string pieceTaken = "";
            if (boardPieces[destination] != null)
            {
                pieceTaken = boardPieces[destination].GetName();
            }

            switch (pieceTaken) 
            {
                case "PAWN":
                    return 1;
                case "KNIGHT":
                    return 3;
                case "BISHOP":
                    return 3;
                case "ROOK":
                    return 5;
                case "QUEEN":
                    return 9;
                case "KING":
                    return 99;
            }

            

            return 0;
        }

        public static IPiece[] SimulateMovePiece(IPiece[] boardPieces, int origin, int destination)
        {
            IPiece[] fakeBoard = new IPiece[64];
            for (int i = 0; i < 64; i++)
            {
                if (boardPieces[i] == null)
                    fakeBoard[i] = null;
                else
                {
                    Type type = boardPieces[i].GetType();
                    List<int> moveHistory = new List<int>();
                    List<string> buffList = new List<string>();
                    // this is necessary to make sure the list of moves is passed by value and not reference
                    foreach (int m in boardPieces[i].GetMoveHistory())
                        moveHistory.Add(m);
                    foreach (string b in boardPieces[i].GetBuffs())
                        buffList.Add(b);
                    IPiece instance = (IPiece)Activator.CreateInstance(type, boardPieces[i].GetColour(), moveHistory, boardPieces[i].GetMoveType(), buffList);
                    fakeBoard[i] = instance;
                }
            }

            if (origin >= 0 && origin <= 63 && destination >= 0 && destination <= 63)
            {
                if (fakeBoard[origin] != null)
                {
                    fakeBoard[destination] = fakeBoard[origin];
                    fakeBoard[destination].AddMove(destination);
                    fakeBoard[origin] = null;
                }
            }

            return fakeBoard;
        }

    }
}
