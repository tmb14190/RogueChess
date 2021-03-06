﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using RogueChess.Pieces;
using RogueChess.AI;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RogueChess
{
    /*
     * Game clas holds the basic gameplay loop
     */
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Board board;

        List<string> buffs;

        IPiece holdingPiece;
        int holdingIndex;
        List<int> holdingMoves;
        int[] lastMove;
        string turn;

        bool AIWHITE;
        bool AIBLACK;

        int lastTime;

        private Vector2 cursorPos;

        /*
         * Game constructor intialising
         */
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set Window Size
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 960;
        }

        /*
         * Initialises all import variables
         */
        protected override void Initialize()
        {
            board = new Board(Content);
            holdingPiece = null;
            holdingIndex = -1;
            holdingMoves = new List<int>();
            buffs = new List<string>();
            lastMove = new int[2];
            turn = "WHITE";

            // set if we want AI playing
            AIWHITE = false;
            AIBLACK = false;

            base.Initialize();
        }

        /*
         * loads graphicsdevice, and calls the setup for piece placement
         */
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitialiseBoard();

        }

        /*
         * Update loop, checks for mouse interaction and AI moves
         */
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // update mouse position
            cursorPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            // AI moves only does it every 2 seconds. thread sleep 2 seconds hard fucked the graphics
            if (((int)gameTime.TotalGameTime.TotalSeconds % 2 == 0) && ((int)gameTime.TotalGameTime.TotalSeconds != lastTime))
            {
                lastTime = (int)gameTime.TotalGameTime.TotalSeconds;
                if ((turn == "WHITE" && AIWHITE) || (turn == "BLACK" && AIBLACK))
                {
                    int[] move;
                    move = AIMain.GetMove(turn, board, lastMove);
                    board.MovePiece(move[0], move[1]);
                    board.boardPieces = Rules.ApplyRulesNewGameState(board.boardPieces, move[1], move[0], Content);

                    lastMove[0] = move[0];
                    lastMove[1] = move[1];

                    if (turn == "WHITE")
                        turn = "BLACK";
                    else
                        turn = "WHITE";
                }
            }

            // if piece is clicked
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && holdingPiece is null && IsMouseInsideWindow())
                FindPieceOnClick();
            // if piece is released
            if (Mouse.GetState().LeftButton == ButtonState.Released && holdingPiece is object && IsMouseInsideWindow())
                FindDestinationOnRelease();

            base.Update(gameTime);
        }

        /*
         * Draw any piece currently held by the mouse
         */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            board.Draw(_spriteBatch);

            if (holdingPiece is object)
            {
                foreach (int move in holdingMoves)
                    board.DrawMove(_spriteBatch, move);

                _spriteBatch.Draw(holdingPiece.GetTexture(), new Rectangle((int)cursorPos.X - 50, (int)cursorPos.Y - 50, 100, 100), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /*
         * Setup the board, currently hardcoded to basic chess
         */
        private void InitialiseBoard()
        {

            // load pawns
            for (int i = 8; i < 16; i++)
                board.AddPiece(i, new Pawn(Content, "BLACK", (132, 132)));
            for (int i = 48; i < 56; i++)
                board.AddPiece(i, new Pawn(Content, "WHITE", (132, 132)));

            // load rooks
            board.AddPiece(0, new Rook(Content, "BLACK", (132, 132)));
            board.AddPiece(7, new Rook(Content, "BLACK", (132, 132)));

            board.AddPiece(56, new Rook(Content, "WHITE", (132, 132)));
            board.AddPiece(63, new Rook(Content, "WHITE", (132, 132)));

            // load knights
            board.AddPiece(1, new Knight(Content, "BLACK", (132, 132)));
            board.AddPiece(6, new Knight(Content, "BLACK", (132, 132)));

            board.AddPiece(57, new Knight(Content, "WHITE", (132, 132)));
            board.AddPiece(62, new Knight(Content, "WHITE", (132, 132)));
            //board.AddPiece(57, new Elephant(Content, "WHITE", (132, 132)));
            //board.AddPiece(62, new Elephant(Content, "WHITE", (132, 132)));

            // load bishops
            board.AddPiece(2, new Bishop(Content, "BLACK", (132, 132)));
            board.AddPiece(5, new Bishop(Content, "BLACK", (132, 132)));

            board.AddPiece(58, new Bishop(Content, "WHITE", (132, 132)));
            board.AddPiece(61, new Bishop(Content, "WHITE", (132, 132)));

            // load queens
            board.AddPiece(3, new Queen(Content, "BLACK", (132, 132)));

            board.AddPiece(59, new Queen(Content, "WHITE", (132, 132)));

            // load kings
            board.AddPiece(4, new King(Content, "BLACK", (132, 132)));

            board.AddPiece(60, new King(Content, "WHITE", (132, 132)));

            // load buffs white
            AddBuff(60, "CASTLE");
            for (int i = 48; i < 56; i++)
            {
                AddBuff(i, "QUEEN UPGRADE");
                AddBuff(i, "EN PASSANT");
            }

            // load buffs black
            AddBuff(4, "CASTLE");
            for (int i = 8; i < 16; i++)
            {
                AddBuff(i, "QUEEN UPGRADE");
                AddBuff(i, "EN PASSANT");
            }

        }
        /*
         * Add new buff to piece
         */
        private void AddBuff(int index, string buff)
        {
            board.boardPieces[index] = Buffs.AddBuff(board.boardPieces[index], buff);
        }

        /*
         * Find the pece from mouse click, if a piece, pick it and with the mouse, and draw all possible legal moves
         */
        public void FindPieceOnClick()
        {
            bool success = false;
            int index = board.GetSquareIndexFromXY(Mouse.GetState().X, Mouse.GetState().Y);
            if (index != -1)
                success = true;

            if (success)
            {
                IPiece piece = board.GetPiece(index);

                if (piece != null && piece.GetColour() == turn)
                {
                    // piece successfully picked up
                    holdingPiece = piece;
                    holdingIndex = index;
                    Debug.WriteLine("Picked up " + piece.GetColour() + " " + piece.GetName());

                    // get possible moves
                    Task<List<int>>.Factory.StartNew(() => Rules.LegalMoves(index, piece, board.boardPieces, lastMove, true));
                    holdingMoves = Rules.LegalMoves(index, piece, board.boardPieces, lastMove, true);

                    // remove piece while its hovering
                    board.RemovePiece(index);
                }
                else
                {
                    Debug.WriteLine("No piece on this square");
                }

            }

        }

        /*
         * When mouse released find the destination for the piece, and apply any neccesary rules from the new gamestate
         */
        public void FindDestinationOnRelease()
        {
            bool success = false;
            int destinationIndex = board.GetSquareIndexFromXY(Mouse.GetState().X, Mouse.GetState().Y);

            if (destinationIndex != -1)
                success = true;

            if (success && holdingMoves.Contains(destinationIndex))
            {
                // gotta simulate the move to check if player checks themselves

                // add piece to new square
                board.AddPiece(destinationIndex, holdingPiece);

                // Apply possible rules to new gamestate
                board.boardPieces = Rules.ApplyRulesNewGameState(board.boardPieces, destinationIndex, holdingIndex, Content);

                // hold last move info (used by en passant alone atm)
                lastMove[0] = holdingIndex;
                lastMove[1] = destinationIndex;

                Debug.WriteLine(holdingPiece.GetName() + " moved from " + holdingIndex.ToString() + " to " + destinationIndex.ToString());

                // lets test checks
                bool whiteCheck = Rules.IsCheck("WHITE", board.boardPieces, lastMove);
                bool blackCheck = Rules.IsCheck("BLACK", board.boardPieces, lastMove);
                if (whiteCheck)
                    Debug.WriteLine("White Checked");
                if (blackCheck)
                    Debug.WriteLine("Black Checked");

                if (turn == "WHITE")
                    turn = "BLACK";
                else
                    turn = "WHITE";

            } else
            {
                // invalid move return piece
                board.ReturnPiece(holdingIndex, holdingPiece);
            }

            holdingPiece = null;
            holdingIndex = -1;
            holdingMoves = new List<int>();
        }

            private bool IsMouseInsideWindow()
        {
            MouseState ms = Mouse.GetState();
            Point pos = new Point(ms.X, ms.Y);
            return (GraphicsDevice.Viewport.Bounds.Contains(pos) && this.IsActive);
        }


    }
}
