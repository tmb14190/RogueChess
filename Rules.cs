using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using Microsoft.Xna.Framework.Content;

namespace RogueChess
{
    public static class Rules
    {
        /*
         * 
         */
        public static IPiece[] ApplyRulesNewGameState(IPiece[] boardPieces, int destinationIndex, int originIndex, ContentManager content)
        {
            if (boardPieces[destinationIndex].GetName() == "PAWN")
                boardPieces = PawnTransform(boardPieces, destinationIndex, content);

            if (boardPieces[destinationIndex].GetName() == "KING" && boardPieces[destinationIndex].GetMoveHistory().Count == 2)
                boardPieces = Buffs.ApplyBuffedMoves(boardPieces, destinationIndex, originIndex, "CASTLE");

                return boardPieces;
        }

        /*
         * 
         */
        public static IPiece[] PawnTransform(IPiece[] boardPieces, int changedIndex, ContentManager content)
        {
                if ((changedIndex >= 0 && changedIndex <= 7) || (changedIndex >= 56 && changedIndex <= 63))
                {
                    boardPieces[changedIndex] = new Queen(content, boardPieces[changedIndex].GetColour(), (132, 132));
                }

            return boardPieces;
        }

        /*
         * 
         */
        public static List<int> LegalMoves(int index, IPiece piece, IPiece[] boardPieces)
        {
            List<int> movements = piece.AllowedMoves(index);
            string colour = piece.GetColour();
            List<int> moves = new List<int>();

            movements = Buffs.CheckBuffedMoves(index, piece, boardPieces, movements);

            if (piece.GetMoveType() == "SINGULAR")
            {
                foreach (int move in movements)
                {
                    // make sure destination square is empty or filled with opponent piece
                    if (boardPieces[move] is null || colour != boardPieces[move].GetColour())
                        moves.Add(move);
                }
            }
            else if (piece.GetMoveType() == "LINEAR")
            {

                bool removeBranch = false;

                foreach (int move in movements)
                {
                    // if piece blocking mvoement branch skip
                    if (removeBranch == true && move != -1)
                        continue;
                    else if (move == -1)
                    {
                        removeBranch = false;
                        continue;
                    }

                    // if piece not null then setup branch for removal. If is null then allow the move
                    if (boardPieces[move] != null)
                    {
                        if (colour == boardPieces[move].GetColour())
                        {
                            removeBranch = true;
                            continue;
                        }
                        else if (colour != boardPieces[move].GetColour())
                        {
                            moves.Add(move);
                            removeBranch = true;
                            continue;
                        }


                    }
                    else
                        moves.Add(move);

                }
            }
            else if (piece.GetMoveType() == "PAWN" && movements.Count > 0) // the pawn exception
            {
                // attack moves
                if (movements[0] - 1 >= 0 && movements[0] + 1 <= 63)
                {
                    if (boardPieces[movements[0] - 1]?.MatchColour(colour) == false)
                        moves.Add(movements[0] - 1);
                    if (boardPieces[movements[0] + 1]?.MatchColour(colour) == false)
                        moves.Add(movements[0] + 1);
                }

                foreach (int move in movements)
                {
                    // make sure destination square is empty
                    if (boardPieces[move] is null)
                        moves.Add(move);
                    else
                        break;
                }
            }

            return moves;
        }



    }
}
