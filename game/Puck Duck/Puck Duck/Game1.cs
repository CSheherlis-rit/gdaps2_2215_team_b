using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        private Texture2D wall;
        private Texture2D empty;
        private Texture2D downPiston;
        private Texture2D upPiston;
        private Texture2D leftPiston;
        private Texture2D rightPiston;
        private Texture2D goal;
        private Texture2D pistonHead;

        private GameState currentState;
 
        private const int windowWidth = 800;
        private const int windowHeight = 800;
        private Rectangle tilePos;
        private Rectangle headPos;

        private TileMap tileMap = new TileMap(windowWidth / 32, windowHeight / 32);
        private Piston pistons;
        List<Tile> pistonsToExtend; // list of extended pistons

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

            //window size
            _graphics.PreferredBackBufferWidth = windowWidth;
            _graphics.PreferredBackBufferHeight = windowHeight;
            _graphics.ApplyChanges();

            //generate the tile map
            tileMap.GenerateTileMap();
            pistons = new Piston(tileMap);

            //initialize pistonsToExtend list
            pistonsToExtend = new List<Tile>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            wall = Content.Load<Texture2D>("WallFiller");
            empty = Content.Load<Texture2D>("EmptyFiller");
            downPiston = Content.Load<Texture2D>("PistonDownFiller");
            upPiston = Content.Load<Texture2D>("PistonUpFiller");
            leftPiston = Content.Load<Texture2D>("PistonLeftFiller");
            rightPiston = Content.Load<Texture2D>("PistonRightFiller");
            goal = Content.Load<Texture2D>("GoalFiller");
            pistonHead = Content.Load<Texture2D>("PistonHead-export");

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
                    
                    // if no level has been generated, generate it
                    if (tileMap.Level == null)
                    {
                        //make the TileMap the size of the window divided by tile size
                        tileMap.GenerateTileMap();
                    }

                    //check if pistons are being extended
                    pistonsToExtend = pistons.checkInput();

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

                    //draw the tiles for the level
                    for (int i = 0; i < tileMap.Level.GetLength(0); i++)
                    {
                        for (int j = 0; j < tileMap.Level.GetLength(1); j++)
                        {
                            tilePos = new Rectangle( i * 32, j * 32, wall.Width, wall.Height);

                            // drawing different tiles for the current type of tile
                            switch (tileMap.Level[i, j].Type)
                            {
                                // empty tiles
                                case Type.Empty:
                                    _spriteBatch.Draw(empty, tilePos, Color.White);
                                    break;

                                // walls
                                case Type.Wall:
                                    _spriteBatch.Draw(wall, tilePos, Color.White);
                                    break;

                                // the goal
                                case Type.Goal:
                                    _spriteBatch.Draw(goal, tilePos, Color.White);
                                    break;

                                // upward facing pistons
                                case Type.UpPiston:
                                    _spriteBatch.Draw(upPiston, tilePos, Color.White);
                                    break;

                                // downward facing pistons
                                case Type.DownPiston:
                                    _spriteBatch.Draw(downPiston, tilePos, Color.White);
                                    break;

                                // right facing pistons
                                case Type.RightPiston:
                                    _spriteBatch.Draw(rightPiston, tilePos, Color.White);
                                    break;

                                // left facing pistons
                                case Type.LeftPiston:
                                    _spriteBatch.Draw(leftPiston, tilePos, Color.White);
                                    break;

                                // start tile
                                case Type.Start:
                                    _spriteBatch.Draw(empty, tilePos, Color.Yellow);
                                    break;
                            }

                            // setting the current tile position as a property for the current tile
                            tileMap.Level[i, j].Position = tilePos;
                        }
                    }

                    //draw piston heads
                    if (pistonsToExtend != null)
                    {
                        //draw extended piston heads in front of all pistons
                        for (int i = 0; i < pistonsToExtend.Count; i++)
                        {
                            if (pistonsToExtend[0].Type == Type.UpPiston)
                            {
                                //set position of the piston head infront of the piston
                                headPos = pistonsToExtend[i].Position;
                                headPos.Y = headPos.Y + 32;
                                _spriteBatch.Draw(pistonHead, headPos, Color.White);
                            }
                            if (pistonsToExtend[0].Type == Type.DownPiston)
                            {
                                //draw extended piston head in front of piston
                            }
                            if (pistonsToExtend[0].Type == Type.LeftPiston)
                            {
                                //draw extended piston head in front of piston
                            }
                            if (pistonsToExtend[0].Type == Type.RightPiston)
                            {
                                //draw extended piston head in front of piston
                            }
                        }
                    }

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
