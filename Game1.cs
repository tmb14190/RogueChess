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

            FindPieceOnClick();
            FindDestinationOnRelease();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            board.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void InitialiseBoard()
        {

            // load pawns
            for (int i = 97; i < 105; i++)
                board.AddPiece((char)i + "7", new Pawn(Content, "BLACK", (132, 132)));
            for (int i = 97; i < 105; i++)
                board.AddPiece((char)i + "2", new Pawn(Content, "WHITE", (132, 132)));

            // load rooks
            board.AddPiece("a8", new Rook(Content, "BLACK", (132, 132)));
            board.AddPiece("h8", new Rook(Content, "BLACK", (132, 132)));

            board.AddPiece("a1", new Rook(Content, "WHITE", (132, 132)));
            board.AddPiece("h1", new Rook(Content, "WHITE", (132, 132)));

            // load knights
            board.AddPiece("b8", new Knight(Content, "BLACK", (132, 132)));
            board.AddPiece("g8", new Knight(Content, "BLACK", (132, 132)));

            board.AddPiece("b1", new Knight(Content, "WHITE", (132, 132)));
            board.AddPiece("g1", new Knight(Content, "WHITE", (132, 132)));

            // load bishops
            board.AddPiece("c8", new Bishop(Content, "BLACK", (132, 132)));
            board.AddPiece("f8", new Bishop(Content, "BLACK", (132, 132)));

            board.AddPiece("c1", new Bishop(Content, "WHITE", (132, 132)));
            board.AddPiece("f1", new Bishop(Content, "WHITE", (132, 132)));

            // load queens
            board.AddPiece("d8", new Queen(Content, "BLACK", (132, 132)));

            board.AddPiece("d1", new Queen(Content, "WHITE", (132, 132)));

            // load kings
            board.AddPiece("e8", new King(Content, "BLACK", (132, 132)));

            board.AddPiece("e1", new King(Content, "WHITE", (132, 132)));



        }

        public void FindPieceOnClick()
        {
            bool success = false;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && IsMouseInsideWindow() && this.IsActive)
            {
                int index = board.GetSquareIndexFromXY(Mouse.GetState().X, Mouse.GetState().Y);
                if (index != -1)
                    success = true;

                if (success)
                {
                    IPiece piece = board.GetPiece(index);

                    // connect texture to mouse
                }
            }

            bool IsMouseInsideWindow()
            {
                MouseState ms = Mouse.GetState();
                Point pos = new Point(ms.X, ms.Y);
                return GraphicsDevice.Viewport.Bounds.Contains(pos);
            }

        }

        public void FindDestinationOnRelease() {

        }


    }
}
