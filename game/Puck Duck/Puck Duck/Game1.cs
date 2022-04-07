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
        Instructions,
        Gameplay,
        LevelFail,
        LevelWon
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteEffects spriteEffects;
        private SpriteFont defaultFont;
        private Texture2D homeScreen;
        private Texture2D wall;
        private Texture2D empty;
        private Texture2D downPiston;
        private Texture2D upPiston;
        private Texture2D leftPiston;
        private Texture2D rightPiston;
        private Texture2D goal;
        private Texture2D pistonHeadUp;
        private Texture2D puck;
        private Texture2D pistonHeadLeft;
        private Texture2D pistonHeadRight;
        private Texture2D pistonHeadDown;
        private Texture2D collectible;
        private Duck duck;
        private Duck evilDuck;
        private List<Collectible> collectibles;

        private GameState currentState;
 
        private const int windowWidth = 800;
        private const int windowHeight = 800;
        private int collectedCount;
        private Rectangle tilePos;
        private Rectangle headPos;
        private Rectangle startPos;
        private Rectangle evilPos;

        private KeyboardState kbState;
        private KeyboardState prevKbState;

        private TileMap tileMap = new TileMap(windowWidth / 32, windowHeight / 32);
        private Piston pistons;
        private List<Tile> pistonsToExtend; // list of extended pistons
        private List<Rectangle> heads;

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
            pistons = new Piston(tileMap);

            //initialize pistonsToExtend list
            pistonsToExtend = new List<Tile>();
            heads = new List<Rectangle>();

            // initialize variables for duck spawning
            startPos = new Rectangle();
            evilPos = new Rectangle();

            // initialize list for collectibles
            collectibles = new List<Collectible>();
            collectedCount = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            homeScreen = Content.Load<Texture2D>("PuckDuckHome");
            wall = Content.Load<Texture2D>("WallFiller");
            empty = Content.Load<Texture2D>("EmptyFiller");
            downPiston = Content.Load<Texture2D>("PistonDownFiller");
            upPiston = Content.Load<Texture2D>("PistonUpFiller");
            leftPiston = Content.Load<Texture2D>("PistonLeftFiller");
            rightPiston = Content.Load<Texture2D>("PistonRightFiller");
            goal = Content.Load<Texture2D>("GoalFiller");
            pistonHeadUp = Content.Load<Texture2D>("PistonHead-export");
            pistonHeadRight = Content.Load<Texture2D>("PistonHeadRight");
            pistonHeadLeft = Content.Load<Texture2D>("PistonHeadLeft");
            pistonHeadDown = Content.Load<Texture2D>("PistonHeadDown");
            puck = Content.Load<Texture2D>("duckanimation");
            collectible = Content.Load<Texture2D>("collectible");

            defaultFont = this.Content.Load<SpriteFont>("Default");

            //create duck object
            duck = new Duck(puck, new Rectangle(0, 0, 20, 20), Direction.Stop, false);
            evilDuck = new Duck(puck, new Rectangle(0, 0, 20, 20), Direction.Stop, true);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //update duck animation
            duck.UpdateAnimation(gameTime);
            evilDuck.UpdateAnimation(gameTime);

            kbState = Keyboard.GetState();

            // switching game state
            switch(currentState)
            {
                
                case GameState.MainMenu:

                    duck.Moves = 0; //reset move count
                    collectedCount = 0;
                    collectibles.Clear();

                    if (kbState.IsKeyDown(Keys.G) && prevKbState.IsKeyUp(Keys.G))
                    {
                        // switch to instructions
                        currentState = GameState.Instructions;
                    }

                    break;

                case GameState.Instructions:
                    if (kbState.IsKeyDown(Keys.D1) && prevKbState.IsKeyUp(Keys.D1))
                    {
                        tileMap.GenerateTileMap("../../../test.csv");

                        //switch to gameplay
                        heads.Clear();
                        duck.Spawned = false;
                        currentState = GameState.Gameplay;
                    }

                    if (kbState.IsKeyDown(Keys.D2) && prevKbState.IsKeyUp(Keys.D2))
                    {
                        tileMap.GenerateTileMap("../../../test1.csv");

                        //switch to gameplay
                        heads.Clear();
                        duck.Spawned = false;
                        currentState = GameState.Gameplay;
                    }

                    if (kbState.IsKeyDown(Keys.D3) && prevKbState.IsKeyUp(Keys.D3))
                    {
                        tileMap.GenerateTileMap("../../../ctest.csv");

                        //switch to gameplay
                        heads.Clear();
                        duck.Spawned = false;
                        currentState = GameState.Gameplay;
                    }

                    break;

                case GameState.Gameplay:

                    if (kbState.IsKeyDown(Keys.M))
                    {
                        //switch to main menu
                        currentState = GameState.MainMenu;
                    }
                    
                    if (kbState.IsKeyDown(Keys.C))
                    {
                        //switch to level clear
                        currentState = GameState.LevelWon;
                    }

                    //check if pistons are being extended
                    pistonsToExtend = pistons.checkInput();

                    foreach (Collectible pickup in collectibles)
                    {
                        if (pickup.CheckCollision(duck))
                        {
                            pickup.IsActive = false;
                            collectedCount++;
                        }
                    }

                    // level is won
                    if (duck.CheckCollision(tileMap) == Direction.Stop && duck.Position != startPos)
                    {
                        currentState = GameState.LevelWon;
                    } 

                    // level is failed because of the evil duck
                    if (evilDuck.CheckCollision(tileMap) == Direction.Stop && evilDuck.Position != evilPos)
                    {
                        currentState = GameState.LevelFail;
                    }

                    // level is failed due to landing on a fail tile
                    if (duck.CheckCollision(tileMap) == Direction.Fail)
                    {
                        currentState = GameState.LevelFail;
                    }

                    //move puck duck
                    duck.Update(gameTime, tileMap, pistonsToExtend, heads);
                    evilDuck.Update(gameTime, tileMap, pistonsToExtend, heads);

                    break;

                case GameState.LevelFail:
                    //will be added later - when conditions are not met

                    if (kbState.IsKeyDown(Keys.M))
                    {

                        collectibles.Clear();

                        //switch to main menu
                        currentState = GameState.MainMenu;
                    }

                    break;
                case GameState.LevelWon:
                    //will be added later - when goal is reached

                    collectibles.Clear();

                    if (kbState.IsKeyDown(Keys.M))
                    {
                        //switch to main menu
                        currentState = GameState.MainMenu;
                    }

                    break;
            }

            // update prevKbState
            prevKbState = kbState;

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
                    _spriteBatch.Draw(homeScreen, new Vector2(0, 0), Color.White);
                    _spriteBatch.DrawString(defaultFont, "Now in main menu\n" +
                        "Press G to see the instructions", new Vector2(10, 10), Color.Black);

                    break;

                case GameState.Instructions:
                    _spriteBatch.DrawString(defaultFont, "Instructions:\n" +
                         "Press M to switch to main menu\n" +
                         "Press C to switch to level clear\n\n" +
                         "Press 1 to play level 1\n" +
                         "Press 2 to play level 2", new Vector2(10, 10), Color.Black);

                    break;

                case GameState.Gameplay:

                    _spriteBatch.DrawString(defaultFont, "Now in gameplay", new Vector2(10, 10), Color.Black);

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

                                // the fail tile
                                case Type.Fail:
                                    _spriteBatch.Draw(empty, tilePos, Color.Red);
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
                                    _spriteBatch.Draw(empty, tilePos, Color.White);
                                    startPos = tilePos;
                                    break;

                                // evilDuck start tile
                                case Type.EvilStart:
                                    _spriteBatch.Draw(empty, tilePos, Color.White);
                                    evilPos = tilePos;
                                    break;

                                // collectible tile
                                case Type.Collectible:
                                    _spriteBatch.Draw(empty, tilePos, Color.White);

                                    if (collectibles.Count < 3)
                                    {
                                        collectibles.Add(new Collectible(collectible, tilePos));
                                    }
                                    break;
                                    
                            }

                            // setting the current tile position as a property for the current tile
                            tileMap.Level[i, j].Position = tilePos;
                        }
                    }

                    // drawing collectibles at proper tiles
                    foreach (Collectible pickup in collectibles)
                    {
                        pickup.Draw(_spriteBatch);
                    }

                    //draw duck at location of start tile
                    duck.Draw(spriteEffects, _spriteBatch);
                    evilDuck.Draw(spriteEffects, _spriteBatch);
                    duck.Spawn(startPos);
                    evilDuck.Spawn(evilPos);
                   

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
                                headPos.Y = headPos.Y - 32;
                                _spriteBatch.Draw(pistonHeadUp, headPos, Color.White);
                                heads.Add(headPos);
                            }
                            if (pistonsToExtend[0].Type == Type.DownPiston)
                            {
                                headPos = pistonsToExtend[i].Position;
                                headPos.Y = headPos.Y + 32;
                                _spriteBatch.Draw(pistonHeadDown, headPos, Color.White);
                                heads.Add(headPos);
                            }
                            if (pistonsToExtend[0].Type == Type.LeftPiston)
                            {
                                headPos = pistonsToExtend[i].Position;
                                headPos.X = headPos.X - 32;
                                _spriteBatch.Draw(pistonHeadLeft, headPos, Color.White);
                                heads.Add(headPos);
                            }
                            if (pistonsToExtend[0].Type == Type.RightPiston)
                            {
                                headPos = pistonsToExtend[i].Position;
                                headPos.X = headPos.X + 32;
                                _spriteBatch.Draw(pistonHeadRight, headPos, Color.White);
                                heads.Add(headPos);
                            }
                        }

                        // clears the list of piston heads if it doesnt match the number of pistons extended
                        if (pistonsToExtend.Count != heads.Count)
                        {
                            heads.Clear();
                        }
                    }

                    _spriteBatch.DrawString(defaultFont, "Moves: " + duck.Moves, new Vector2(10, 10), Color.Black);

                    break;

                case GameState.LevelWon:
                    _spriteBatch.DrawString(defaultFont, "Level complete!\n" +
                        "Collectibles obtained: " + collectedCount +
                        "\nPress M to switch to main menu", new Vector2(10, 10), Color.Black);
                    break;
                case GameState.LevelFail:
                    _spriteBatch.DrawString(defaultFont, "You lost :(\n" +
                        "Press M to return to main menu", new Vector2(10, 10), Color.Black);
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
