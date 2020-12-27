using System;
using System.Collections.Generic;
using System.Text;
using RogueChess.Pieces;
using System.Diagnostics;

namespace RogueChess
{
    public static class Buffs
    {

        public static IPiece[] Castle(IPiece[] boardPieces)
        {
            foreach (IPiece piece in boardPieces)
            {
                if (piece?.GetName() == "KING")
                {
                    piece.ApplyBuff("CASTLE");
                }
            }

            return boardPieces;
        }

        public static List<int> CheckBuffedMoves(int index, IPiece piece, IPiece[] boardPieces, List<int> movements)
        {
            Debug.WriteLine("old");
            foreach (int move in movements)
                Debug.WriteLine(move);
            // if piece castle buffed
            foreach (string buff in piece.GetBuffs())
            {

                if (buff == "CASTLE")
                {
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
                }



            }

            Debug.WriteLine("new");
            foreach (int move in movements)
                Debug.WriteLine(move);

            return movements;
        }

        public static IPiece[] ApplyBuffedMoves(IPiece[] boardPieces, int destinationIndex, int originIndex, string buff)
        {
            if (buff == "CASTLE")
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

            return boardPieces;
        }

    }
}
