using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using RogueChess.Pieces;
using System.Diagnostics;

namespace RogueChess
{

    class Board
    {

        public List<Rectangle> squares;
        public Dictionary<string, IPiece> map;
        Texture2D purple;
        Texture2D grey;
        private static string[] notation = {"a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
                                            "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
                                            "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
                                            "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
                                            "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
                                            "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
                                            "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
                                            "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1" };

        public Board(ContentManager Content)
        {
            squares = new List<Rectangle>();
            squares = FillSquares(squares);

            purple = Content.Load<Texture2D>("Purple_square");
            grey = Content.Load<Texture2D>("Grey_square");

            map = new Dictionary<string, IPiece>();
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            DrawBoard();
            DrawPieces();

            void DrawBoard()
            {
                int flag = 0;
                for (int i = 0; i < 64; i++)
                {
                    if (i % 8 == 0)
                        flag += 1;

                    if (flag % 2 == 0)
                    {
                        if (i % 2 == 0)
                            sb.Draw(purple, squares[i], Color.White);
                        else
                            sb.Draw(grey, squares[i], Color.White);
                    }
                    else
                    {
                        if (i % 2 == 0)
                            sb.Draw(grey, squares[i], Color.White);
                        else
                            sb.Draw(purple, squares[i], Color.White);
                    }
                }
            }

            void DrawPieces()
            {

                foreach (KeyValuePair<string, IPiece> entry in map)
                {
                    // do something with entry.Value or entry.Key
                    int index = Array.IndexOf(notation, entry.Key);

                    sb.Draw(entry.Value.GetTexture(), squares[index], Color.White);
                }

            }

        }

        public List<Rectangle> FillSquares(List<Rectangle> squares)
        {

            int x = 240;
            int y = 80;

            for (int i=1; i < 65; i++)
            {

                squares.Add(new Rectangle(x, y, 100, 100));

                x += 100;
                if (i % 8 == 0)
                {
                    x = 240;
                    y += 100;
                }
            }

            return squares;
        }

        public void AddPiece(string position, IPiece p)
        {
            int original_size = map.Count;
            foreach (string n in notation)
            {
                if (position.Contains(n))
                    map.Add(position, p);
            }

            if (map.Count == original_size)
                Debug.WriteLine("No such square as " + position);
        }

        public IPiece GetPiece(int index)
        {
            return map[notation[index]];
        }

        public int GetSquareIndexFromXY(int x, int y)
        {
            // first rectangle x y at 240, 80, each size 100, 100, 64 rects
            // 0 = 0, 0 (0 - 99, 0 - 99 but we can round down) 
            // 1 = 1, 0 (100 - 199, 0 - 99 but we can round down)
            // 2 = 2, 0
            // 3 = 3, 0
            // 4 = 4, 0
            // 5 = 5, 0
            // 6 = 6, 0
            // 7 = 7, 0
            // 8 = 0, 1 (0 - 99, 100 - 199 but we can round down)
            // 63 = 7, 7 (700 - 799, 700 - 799 but we can round down)
            x = x - 240;
            y = y - 80;
            string sX = x.ToString();
            string sY = y.ToString();
            bool workX = true;
            bool workY = true;

            if (sX.Length == 2)
                x = 0;
            else
            {
                workX = Int32.TryParse(sX[0].ToString(), out x);
            }

            if (sY.Length == 2)
                y = 0;
            else
            {
                workY = Int32.TryParse(sY[0].ToString(), out y);
            }
            // now x, y = 0 - 7

            if (workX && workY && x >= 0 && x <= 7 && y >= 0 && y <= 7) {
                y = y * 8;
                int index = y + x;
                Debug.WriteLine(index);
                return index;
            }
            else
            {
                Debug.WriteLine("Not on the board");
                return -1;
            }
        }
    }
}
