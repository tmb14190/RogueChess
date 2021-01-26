using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace RogueChess
{
    public static class Buffs
    {

        private static int[] ENPASSANT;

        /* 
         * This is used as a middleman to make sure all buffs added to pieces are legit, removing a need for validation by individual pieces
        */
        public static IPiece AddBuff(IPiece piece, string buff)
        {
            switch (buff)
            {
                case "CASTLE":
                    if (piece.GetName() == "KING")
                        piece.AddBuff("CASTLE");
                    break;
                case "QUEEN UPGRADE":
                    piece.AddBuff("QUEEN UPGRADE");
                    break;
                case "EN PASSANT":
                    piece.AddBuff("EN PASSANT");
                    break;
                case "KNIGHT MOVES":
                    piece.AddBuff("KNIGHT MOVES");
                    break;
                default:
                    Debug.WriteLine("No buff by name: " + buff);
                    break;
            }

            return piece;
        }

        /*
         * Logic for buffed moves, currently in switch, but should be expanded to individual files for each buff for scalability
         */
        public static List<int> CheckBuffedMoves(int index, IPiece piece, IPiece[] boardPieces, List<int> moves, int[] lastMove)
        {
            // if piece castle buffed
            foreach (string buff in piece.GetBuffs())
            {
                switch (buff)
                {
                    case "CASTLE":
                        if (moves.Contains(index + 2))
                        {
                            // check rook on right
                            bool rook = false;
                            if (boardPieces[index + 3] != null)
                            {
                                if ((boardPieces[index + 3].GetName() == "ROOK" && boardPieces[index + 3].GetMoveHistory().Count == 1))
                                    rook = true;
                            }
                            // check path is clear
                            if (boardPieces[index + 2] != null || boardPieces[index + 1] != null || rook == false)
                            {
                                moves.Remove(index + 2);
                            }
                        }

                        if (moves.Contains(index - 2))
                        {
                            // check rook on left
                            bool rook = false;
                            if (boardPieces[index - 4] != null)
                            {
                                if ((boardPieces[index - 4].GetName() == "ROOK" && boardPieces[index - 4].GetMoveHistory().Count == 1))
                                    rook = true;
                            }
                            // check path is clear
                            if (boardPieces[index - 3] != null || boardPieces[index - 2] != null || boardPieces[index - 1] != null || rook == false)
                            {
                                moves.Remove(index - 2);
                            }
                        }
                        break;
                    case "QUEEN UPGRADE":
                        break;
                    case "EN PASSANT":
                        ENPASSANT = new int[2];
                        // code that figures new moves
                        if (piece.GetColour() == "WHITE" && index >=  24 && index <= 31)
                        {
                            if (lastMove[0] >= 8 && lastMove[0] <= 15 && lastMove[1] >= 24 && lastMove[1] <= 31)
                            {
                                if (boardPieces[lastMove[1]].GetName() == "PAWN")
                                {
                                    Debug.WriteLine("En Passantable");
                                    // en passant alive
                                    if (index - lastMove[1] == 1)
                                    {
                                        moves.Add(index - 9);
                                        ENPASSANT[0] = (index - 9);
                                    }
                                    else if (index - lastMove[1] == -1)
                                    {
                                        moves.Add(index - 7);
                                        ENPASSANT[1] = index - 7;
                                    }

                                }
                            }
                        }

                        if (piece.GetColour() == "BLACK" && index >= 32 && index <= 39)
                        {
                            if (lastMove[0] >= 48 && lastMove[0] <= 55 && lastMove[1] >= 32 && lastMove[1] <= 39)
                            {
                                if (boardPieces[lastMove[1]].GetName() == "PAWN")
                                {
                                    Debug.WriteLine("En Passantable");
                                    // en passant alive
                                    if (index - lastMove[1] == 1)
                                    {
                                        moves.Add(index + 7);
                                        ENPASSANT[0] = (index + 7);
                                    }
                                    else if (index - lastMove[1] == -1)
                                    {
                                        moves.Add(index + 9);
                                        ENPASSANT[1] = index + 9;
                                    }
                                }
                            }
                        }
                        break;
                    case "KNIGHT MOVES":
                        // up
                        if (index % 8 != 7 && index > 16)
                            moves.Add(index - 15);
                        if (index % 8 != 0 && index > 16)
                            moves.Add(index - 17);

                        // left
                        if (index % 8 != 0 && index % 8 != 1)
                        {
                            if (index > 7)
                                moves.Add(index - 10);
                            if (index < 56)
                                moves.Add(index + 6);
                        }

                        // right
                        if (index % 8 != 7 && index % 8 != 6)
                        {
                            if (index > 7)
                                moves.Add(index - 6);
                            if (index < 56)
                                moves.Add(index + 10);
                        }

                        // down
                        if (index % 8 != 0 && index < 48)
                            moves.Add(index + 15);
                        if (index % 8 != 7 && index < 48)
                            moves.Add(index + 17);

                        foreach (int move in moves.ToList())
                        {
                            if (piece.GetColour() == boardPieces[move]?.GetColour())
                                moves.Remove(move);
                        }

                        break;

                    default:
                        Debug.WriteLine("Unknown buff: " + buff);
                        break;
                }

            }

            return moves;
        }

        /*
         * Applies the further work needed after a buffed move played, e.g. moving the rook after a castle.
         */
        public static IPiece[] ApplyBuffedMoves(ContentManager content, IPiece[] boardPieces, int destinationIndex, int originIndex, string buff)
        {
            switch (buff)
            {
                case "CASTLE":
                    if (boardPieces[destinationIndex].GetMoveHistory().Count == 2)
                    {
                        if (originIndex - destinationIndex == 2)
                        {
                            boardPieces[originIndex - 1] = boardPieces[originIndex - 4];
                            boardPieces[originIndex - 1].AddMove(originIndex - 1);
                            boardPieces[originIndex - 4] = null;
                        }
                        if (originIndex - destinationIndex == -2)
                        {
                            boardPieces[originIndex + 1] = boardPieces[originIndex + 3];
                            boardPieces[originIndex + 1].AddMove(originIndex + 1);
                            boardPieces[originIndex + 3] = null;
                        }
                    }
                    break;
                case "QUEEN UPGRADE":
                    if ((destinationIndex >= 0 && destinationIndex <= 7) || (destinationIndex >= 56 && destinationIndex <= 63))
                    {
                        boardPieces[destinationIndex] = new Queen(content, boardPieces[destinationIndex].GetColour(), (132, 132));
                    }
                    break;
                case "EN PASSANT":
                    // code that gets rid of the  pawn that hasnt been deleted due to en passant pawn not taking a piece on the same square
                    if (ENPASSANT != null)
                    {
                        if (ENPASSANT[0] == destinationIndex || ENPASSANT[1] == destinationIndex)
                        {
                            if (boardPieces[destinationIndex].GetColour() == "WHITE")
                                boardPieces[destinationIndex + 8] = null;
                            else if (boardPieces[destinationIndex].GetColour() == "BLACK")
                                boardPieces[destinationIndex - 8] = null;
                        }
                    }
                    break;

                default:
                    Debug.WriteLine("Unknown buff: " + buff);
                    break;
            }

            return boardPieces;
        }

    }
}
