using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using RogueChess.Pieces;
using System.Diagnostics;

namespace RogueChess
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Board board;

        IPiece holdingPiece;
        int holdingIndex;
        List<int> holdingMoves;

        private Vector2 cursorPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set Window Size
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 960;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            board = new Board(Content);
            holdingPiece = null;
            holdingIndex = -1;
            holdingMoves = new List<int>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            InitialiseBoard();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // update mouse position
            cursorPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            // if piece is clicked
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && holdingPiece is null && IsMouseInsideWindow())
                FindPieceOnClick();
            // if piece is released
            if (Mouse.GetState().LeftButton == ButtonState.Released && holdingPiece is object && IsMouseInsideWindow())
                FindDestinationOnRelease();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
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



        }

        public void FindPieceOnClick()
        {
            bool success = false;
            int index = board.GetSquareIndexFromXY(Mouse.GetState().X, Mouse.GetState().Y);
            if (index != -1)
                success = true;

            if (success)
            {
                IPiece piece = board.GetPiece(index);

                if (piece != null)
                {
                    // piece successfully picked up
                    holdingPiece = piece;
                    holdingIndex = index;
                    Debug.WriteLine("Picked up " + piece.GetColour() + " " + piece.GetName());

                    // get possible moves
                    holdingMoves = board.GetPossibleMoves(index, piece);

                    // remove piece while its hovering
                    board.RemovePiece(index);
                }
                else
                {
                    Debug.WriteLine("No piece on this square");
                }

            }

        }

        public void FindDestinationOnRelease()
        {
            bool success = false;
            int newIndex = board.GetSquareIndexFromXY(Mouse.GetState().X, Mouse.GetState().Y);

            if (newIndex != -1)
                success = true;

            if (success)
            {
                // check if move is aok
                if (holdingMoves.Contains(newIndex))
                    // add piece to new square
                    board.AddPiece(newIndex, holdingPiece);
                else
                    board.ReturnPiece(holdingIndex, holdingPiece);

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
