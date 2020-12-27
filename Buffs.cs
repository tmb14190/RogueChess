using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace RogueChess
{
    public static class Buffs
    {

        /* 
         * This is used as a middleman to make sure all buffs added to pieces are legit, removing a need for validation by individual pieces
        */
        public static IPiece AddBuff(IPiece piece, string buff)
        {
            switch (buff)
            {
                case "CASTLE":
                    piece.AddBuff("CASTLE");
                    break;
                case "QUEEN UPGRADE":
                    piece.AddBuff("QUEEN UPGRADE");
                    break;
                default:
                    Debug.WriteLine("No buff by name: " + buff);
                    break;
            }

            return piece;
        }

        /*
         * 
         */
        public static List<int> CheckBuffedMoves(int index, IPiece piece, IPiece[] boardPieces, List<int> movements)
        {
            // if piece castle buffed
            foreach (string buff in piece.GetBuffs())
            {

                switch (buff)
                {
                    case "CASTLE":
                        if (movements.Contains(index + 2))
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
                                Debug.WriteLine("Removing Right");
                                movements.Remove(index + 2);
                            }
                        }

                        if (movements.Contains(index - 2))
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
                                Debug.WriteLine("Removing Left");
                                movements.Remove(index - 2);
                            }
                        }
                        break;
                    case "QUEEN UPGRADE":
                        break;

                    default:
                        Debug.WriteLine("Unknown buff: " + buff);
                        break;
                }

            }

            return movements;
        }

        /*
         * 
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

                default:
                    Debug.WriteLine("Unknown buff: " + buff);
                    break;
            }

            return boardPieces;
        }

    }
}
