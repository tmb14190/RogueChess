using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace RogueChess
{
    public static class Rules
    {
        /*
         * 
         */
        public static IPiece[] ApplyRulesNewGameState(IPiece[] boardPieces, int destinationIndex, int originIndex, ContentManager content)
        {
            foreach (string buff in boardPieces[destinationIndex].GetBuffs())
            {
                switch (buff)
                {
                    case "CASTLE":
                        boardPieces = Buffs.ApplyBuffedMoves(content, boardPieces, destinationIndex, originIndex, "CASTLE");
                        break;
                    case "QUEEN UPGRADE":
                        boardPieces = Buffs.ApplyBuffedMoves(content, boardPieces, destinationIndex, originIndex, "QUEEN UPGRADE");
                        break;

                    default:
                        Debug.WriteLine("Unknown buff: " + buff);
                        break;
                }
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

            switch (piece.GetMoveType())
            {
                case "SINGULAR":
                    foreach (int move in movements)
                    {
                        // make sure destination square is empty or filled with opponent piece
                        if (boardPieces[move] is null || colour != boardPieces[move].GetColour())
                            moves.Add(move);
                    }
                    break;
                case "LINEAR":
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
                    break;
                case "PAWN": // the pawn exception, need to be remodelled as a buff
                    if (movements.Count > 0) 
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
                    break;
                default:
                    Debug.WriteLine("Error " + piece.GetName() + " has no move type");
                    break;
            }

            return moves;
        }



    }
}
