using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Puck_Duck
{
    // tracking possible game states
    enum GameState
    {
        MainMenu,
        Gameplay,
        LevelClear,
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont defaultFont;

        private GameState currentState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            currentState = GameState.MainMenu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            defaultFont = this.Content.Load<SpriteFont>("Default");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            KeyboardState kbState = Keyboard.GetState();

            // switching game state
            switch(currentState)
            {
                // switching to gameplay
                case GameState.MainMenu:
                    if (kbState.IsKeyDown(Keys.G))
                    {
                        currentState = GameState.Gameplay;
                    }

                    break;

                case GameState.Gameplay:
                    // switching to main menu
                    if (kbState.IsKeyDown(Keys.M))
                    {
                        currentState = GameState.MainMenu;
                    }

                    // switching to level clear
                    if (kbState.IsKeyDown(Keys.C))
                    {
                        currentState = GameState.LevelClear;
                    }

                    break;

                case GameState.LevelClear:
                    // switching to main menu
                    if (kbState.IsKeyDown(Keys.M))
                    {
                        currentState = GameState.MainMenu;
                    }

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // displaying different screens for each state
            switch (currentState)
            {
                case GameState.MainMenu:
                    _spriteBatch.DrawString(defaultFont, "Now in main menu\n" +
                        "Press G to switch to gameplay", new Vector2(10, 10), Color.Black);

                    break;

                case GameState.Gameplay:
                    _spriteBatch.DrawString(defaultFont, "Now in gameplay\n" +
                        "Press M to switch to main menu\n" +
                        "Press C to switch to level clear", new Vector2(10, 10), Color.Black);

                    break;

                case GameState.LevelClear:
                    _spriteBatch.DrawString(defaultFont, "Now in level clear\n" +
                        "Press M to switch to main menu", new Vector2(10, 10), Color.Black);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
