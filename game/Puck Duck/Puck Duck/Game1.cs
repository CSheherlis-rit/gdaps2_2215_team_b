using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        LevelWon,
        LevelSelect
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteEffects spriteEffects;
        private SpriteFont defaultFont;
        private Texture2D homeScreen;
        private Texture2D wallRed;
        private Texture2D wallBlue;
        private Texture2D wallYellow;
        private Texture2D empty;
        private Texture2D downPiston;
        private Texture2D upPiston;
        private Texture2D leftPiston;
        private Texture2D rightPiston;
        private Texture2D downPistonOpen;
        private Texture2D upPistonOpen;
        private Texture2D leftPistonOpen;
        private Texture2D rightPistonOpen;
        private Texture2D goal;
        private Texture2D pistonHeadUp;
        private Texture2D puck;
        private Texture2D pistonHeadLeft;
        private Texture2D pistonHeadRight;
        private Texture2D pistonHeadDown;
        private Texture2D collectible;
        private Texture2D tableTexture;
        private Texture2D levelSelectScreen;
        private Texture2D clear0Star;
        private Texture2D clear1Star;
        private Texture2D clear2Star;
        private Texture2D clear3Star;
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

        private List<Button> buttons = new List<Button>();

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
            wallRed = Content.Load<Texture2D>("Wall_Red");
            wallBlue = Content.Load<Texture2D>("Wall_Blue");
            wallYellow = Content.Load<Texture2D>("Wall_Yellow");
            empty = Content.Load<Texture2D>("EmptyFiller");
            downPiston = Content.Load<Texture2D>("PistonDownClosed");
            upPiston = Content.Load<Texture2D>("PistonUpClosed");
            leftPiston = Content.Load<Texture2D>("PistonLeftClosed");
            rightPiston = Content.Load<Texture2D>("PistonRightClosed");
            downPistonOpen = Content.Load<Texture2D>("PistonDownOpen");
            upPistonOpen = Content.Load<Texture2D>("PistonUpOpen");
            leftPistonOpen = Content.Load<Texture2D>("PistonLeftOpen");
            rightPistonOpen = Content.Load<Texture2D>("PistonRightOpen");
            goal = Content.Load<Texture2D>("GoalFiller");
            pistonHeadUp = Content.Load<Texture2D>("PistonHeadUp");
            pistonHeadRight = Content.Load<Texture2D>("PistonHeadRight");
            pistonHeadLeft = Content.Load<Texture2D>("PistonHeadLeft");
            pistonHeadDown = Content.Load<Texture2D>("PistonHeadDown");
            puck = Content.Load<Texture2D>("duckanimation");
            collectible = Content.Load<Texture2D>("collectible");
            tableTexture = Content.Load<Texture2D>("AirHockeyTexture");
            levelSelectScreen = Content.Load<Texture2D>("LevelSelectBG");
            clear0Star = Content.Load<Texture2D>("clear0Star");
            clear1Star = Content.Load<Texture2D>("clear1Star");
            clear2Star = Content.Load<Texture2D>("clear2Star");
            clear3Star = Content.Load<Texture2D>("clear3Star");

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
            switch (currentState)
            {

                case GameState.MainMenu:

                    duck.Moves = 0; //reset move count
                    collectedCount = 0;
                    collectibles.Clear();
                    duck.Movement = Direction.Stop;

                    if (buttons.Count < 2)
                    {
                        /*buttons.Add(new Button(                     //button[0]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(125, 615, 240, 100),  // where to put the button/size of button
                                "PLAY",                             // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                  // button color

                        buttons[0].OnButtonClick += InstructionsButton;*/

                        buttons.Add(new Button(                     //button[1]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(430, 615, 240, 100),  // where to put the button/size of button
                                "LEVELS",                           // button label
                                defaultFont,                        // label font
                                Color.Blue));                       // button color

                        buttons[0].OnButtonClick += LevelSelectButton;
                    }

                    break;

                case GameState.Instructions:
                    if (buttons.Count < 3)
                    {
                        buttons.Add(new Button(                     //button[7]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(260, 200, 240, 100),  // where to put the button/size of button
                                "PLAY",                              // button label
                                defaultFont,                        // label font
                                Color.Blue));                       // button color

                        buttons[0].OnButtonClick += PlayButton;

                        buttons.Add(new Button(                     //button[8]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(260, 400, 240, 100),  // where to put the button/size of button
                                "MAIN MENU",                        // button label
                                defaultFont,                        // label font
                                Color.Fuchsia));                    // button color

                        buttons[1].OnButtonClick += MainMenuButton;
                    }
                    break;

                case GameState.Gameplay:

                    if (kbState.IsKeyUp(Keys.M) && prevKbState.IsKeyDown(Keys.M))
                    {
                        //switch to main menu
                        currentState = GameState.MainMenu;
                    }

                    if (kbState.IsKeyUp(Keys.C) && prevKbState.IsKeyDown(Keys.C))
                    {
                        //switch to level clear
                        currentState = GameState.LevelWon;
                    }

                    //check if pistons are being extended
                    pistonsToExtend = pistons.CheckInput();

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
                    duck.Movement = Direction.Stop;

                    if (kbState.IsKeyUp(Keys.M) && prevKbState.IsKeyDown(Keys.M))
                    {

                        collectibles.Clear();
                        buttons.Clear();

                        //switch to main menu
                        currentState = GameState.MainMenu;
                    }

                    buttons.Add(new Button(                     // button[2]
                            _graphics.GraphicsDevice,           // device to create a custom texture
                            new Rectangle(280, 600, 240, 100),  // where to put the button/size of button
                            "MAIN MENU",                        // button label
                            defaultFont,                        // label font
                            Color.Pink));                     // button color

                    buttons[0].OnButtonClick += MainMenuButton;

                    break;
                case GameState.LevelWon:
                    collectibles.Clear();
                    duck.Movement = Direction.Stop;

                    if (kbState.IsKeyUp(Keys.M) && prevKbState.IsKeyDown(Keys.M))
                    {
                        buttons.Clear();
                        //switch to main menu
                        currentState = GameState.MainMenu;
                    }

                    buttons.Add(new Button(                     // button[2]
                            _graphics.GraphicsDevice,           // device to create a custom texture
                            new Rectangle(280, 600, 240, 100),  // where to put the button/size of button
                            "MAIN MENU",                        // button label
                            defaultFont,                        // label font
                            Color.Pink));                     // button color

                    buttons[0].OnButtonClick += MainMenuButton;

                    break;

                case GameState.LevelSelect:
                    if (buttons.Count < 5)
                    {
                        buttons.Add(new Button(                     //button[3]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(300, 75, 200, 60),  // where to put the button/size of button
                                "LEVEL 1",                        // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                     // button color

                        buttons.Add(new Button(                     //button[4]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(300, 175, 200, 60),  // where to put the button/size of button
                                "LEVEL 2",                        // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                     // button color



                        buttons.Add(new Button(                     //button[5]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(300, 275, 200, 60),  // where to put the button/size of button
                                "LEVEL 3",                        // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                     // button color



                        buttons.Add(new Button(                     //button[6]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(300, 375, 200, 60),   // where to put the button/size of button
                                "LEVEL 4",                          // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                    // button color

                        buttons.Add(new Button(                     //button[6]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(300, 475, 200, 60),   // where to put the button/size of button
                                "LEVEL 5",                  // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                    // button color

                        buttons.Add(new Button(                     //button[6]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(300, 575, 200, 60),   // where to put the button/size of button
                                "LEVEL 6",                  // button label
                                defaultFont,                        // label font
                                Color.LimeGreen));                    // button color

                        buttons[0].OnButtonClick += Level1Button;

                        buttons[1].OnButtonClick += Level2Button;

                        buttons[2].OnButtonClick += Level3Button;

                        buttons[3].OnButtonClick += Level4Button;

                        buttons[4].OnButtonClick += Level5Button;

                        buttons[5].OnButtonClick += Level6Button;

                        buttons.Add(new Button(                     //button[2]
                                _graphics.GraphicsDevice,           // device to create a custom texture
                                new Rectangle(280, 685, 240, 100),  // where to put the button/size of button
                                "MAIN MENU",                        // button label
                                defaultFont,                        // label font
                                Color.MediumPurple));                     // button color

                        buttons[6].OnButtonClick += MainMenuButton;
                    }

                    break;
            }

            //update buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update();
            }

            // update prevKbState
            prevKbState = kbState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            // displaying different screens for each state
            switch (currentState)
            {
                case GameState.MainMenu:
                    //draw buttons
                    foreach(Button button in buttons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    //draw homescreen
                    _spriteBatch.Draw(homeScreen, new Vector2(0, 0), Color.White);


                    break;

                case GameState.Instructions:
                    _spriteBatch.DrawString(defaultFont, "How to play:\n" +
                         "Press M to switch to main menu\n" +
                         "Press C to switch to level clear\n\n",
                         new Vector2(265, 80), Color.Black);

                    //draw buttons
                    foreach (Button button in buttons)
                    {
                        button.Draw(_spriteBatch);
                    }

                    break;

                case GameState.Gameplay:

                    //draw table texture
                    _spriteBatch.Draw(tableTexture, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    //draw the tiles for the level
                    for (int i = 0; i < tileMap.Level.GetLength(0); i++)
                    {
                        for (int j = 0; j < tileMap.Level.GetLength(1); j++)
                        {
                            tilePos = new Rectangle(i * 32, j * 32, wallRed.Width, wallRed.Height);

                            // drawing different tiles for the current type of tile
                            switch (tileMap.Level[i, j].Type)
                            {

                                // walls
                                case Type.Wall:
                                    DrawWalls(tileMap.Level[i,j]);
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
                                    if (kbState.IsKeyUp(Keys.Up))
                                    {
                                        _spriteBatch.Draw(upPiston, tilePos, Color.White);
                                    }
                                    else
                                    {
                                        _spriteBatch.Draw(upPistonOpen, tilePos, Color.White);
                                    }
                                    
                                    break;

                                // downward facing pistons
                                case Type.DownPiston:
                                    if (kbState.IsKeyUp(Keys.Down))
                                    {
                                        _spriteBatch.Draw(downPiston, tilePos, Color.White);
                                    }
                                    else
                                    {
                                        _spriteBatch.Draw(downPistonOpen, tilePos, Color.White);
                                    }
                                    break;

                                // right facing pistons
                                case Type.RightPiston:
                                    if (kbState.IsKeyUp(Keys.Right))
                                    {
                                        _spriteBatch.Draw(rightPiston, tilePos, Color.White);
                                    }
                                    else
                                    {
                                        _spriteBatch.Draw(rightPistonOpen, tilePos, Color.White);
                                    }
                                    break;

                                // left facing pistons
                                case Type.LeftPiston:
                                    if (kbState.IsKeyUp(Keys.Left))
                                    {
                                        _spriteBatch.Draw(leftPiston, tilePos, Color.White);
                                    }
                                    else
                                    {
                                        _spriteBatch.Draw(leftPistonOpen, tilePos, Color.White);
                                    }
                                    break;

                                // start tile
                                case Type.Start:
                                    startPos = tilePos;
                                    break;

                                // evilDuck start tile
                                case Type.EvilStart:
                                    evilPos = tilePos;
                                    break;

                                // collectible tile
                                case Type.Collectible:

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
                    duck.Spawn(startPos);
                    evilDuck.Spawn(evilPos);

                    if (evilPos.X != 0)
                    {
                        evilDuck.Draw(spriteEffects, _spriteBatch);
                    }


                    //draw piston heads
                    if (pistonsToExtend != null)
                    {
                        //draw extended piston heads in front of all pistons
                        foreach(Tile piston in pistonsToExtend)
                        {
                            if (piston.Type == Type.UpPiston)
                            {
                                //set position of the piston head infront of the piston
                                headPos = piston.Position;
                                headPos.Y = headPos.Y - 32;
                                _spriteBatch.Draw(pistonHeadUp, headPos, Color.White);
                                heads.Add(headPos);
                            }
                            if (piston.Type == Type.DownPiston)
                            {
                                headPos = piston.Position;
                                headPos.Y = headPos.Y + 32;
                                _spriteBatch.Draw(pistonHeadDown, headPos, Color.White);
                                heads.Add(headPos);
                            }
                            if (piston.Type == Type.LeftPiston)
                            {
                                headPos = piston.Position;
                                headPos.X = headPos.X - 32;
                                _spriteBatch.Draw(pistonHeadLeft, headPos, Color.White);
                                heads.Add(headPos);
                            }
                            if (piston.Type == Type.RightPiston)
                            {
                                headPos = piston.Position;
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

                    //draw the level clear screen depending on how many collectibles were collected
                    switch (collectedCount)
                    {
                        case 0:
                            _spriteBatch.Draw(clear0Star, 
                                new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                            break;

                        case 1:
                            _spriteBatch.Draw(clear1Star, 
                                new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                            break;

                        case 2:
                            _spriteBatch.Draw(clear2Star, 
                                new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                            break;

                        case 3:
                            _spriteBatch.Draw(clear3Star, 
                                new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
                            break;
                    }

                    //draw main menu button
                    foreach (Button button in buttons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    break;

                case GameState.LevelFail:
                    _spriteBatch.DrawString(defaultFont, "You lost :(\n",
                        new Vector2(10, 10), Color.Black);

                    //draw main menu button
                    foreach (Button button in buttons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    break;

                case GameState.LevelSelect:

                    //draw background
                    _spriteBatch.Draw(levelSelectScreen, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);

                    //draw buttons
                    foreach (Button button in buttons)
                    {
                        button.Draw(_spriteBatch);
                    }
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        //button methods
        protected void InstructionsButton()
        {
            buttons.Clear();
            currentState = GameState.Instructions;
        }
        protected void PlayButton()
        {
            buttons.Clear();
            currentState = GameState.Gameplay;
        }
        protected void LevelSelectButton()
        {
            buttons.Clear();
            currentState = GameState.LevelSelect;
        }
        protected void MainMenuButton()
        {
            buttons.Clear();
            currentState = GameState.MainMenu;
        }
        protected void Level1Button()
        {
            buttons.Clear();
            tileMap.GenerateTileMap("../../../lvl 1.csv");

            //switch to gameplay
            heads.Clear();
            duck.Spawned = false;
            currentState = GameState.Gameplay;
        }
        protected void Level2Button()
        {
            buttons.Clear();
            tileMap.GenerateTileMap("../../../lvl 2.csv");

            //switch to gameplay
            heads.Clear();
            duck.Spawned = false;
            currentState = GameState.Gameplay;
        }
        protected void Level3Button()
        {
            buttons.Clear();
            tileMap.GenerateTileMap("../../../lvl 3.csv");

            //switch to gameplay
            heads.Clear();
            duck.Spawned = false;
            currentState = GameState.Gameplay;
        }
        protected void Level4Button()
        {
            buttons.Clear();
            tileMap.GenerateTileMap("../../../lvl 4.csv");

            //switch to gameplay
            heads.Clear();
            duck.Spawned = false;
            currentState = GameState.Gameplay;
        }
        protected void Level5Button()
        {
            buttons.Clear();
            tileMap.GenerateTileMap("../../../lvl 5.csv");

            //switch to gameplay
            heads.Clear();
            duck.Spawned = false;
            currentState = GameState.Gameplay;
        }

        protected void Level6Button()
        {
            buttons.Clear();
            tileMap.GenerateTileMap("../../../lvl 6.csv");

            //switch to gameplay
            heads.Clear();
            duck.Spawned = false;
            currentState = GameState.Gameplay;
        }

        private void DrawWalls(Tile tile)
        {
            //create pattern of wall colors
            if (tile.WallColor == 0)
            {
                _spriteBatch.Draw(wallRed, tilePos, Color.White);
            }
            else if (tile.WallColor == 1)
            {
                _spriteBatch.Draw(wallBlue, tilePos, Color.White);
            }
            else if (tile.WallColor == 2)
            {
                _spriteBatch.Draw(wallBlue, tilePos, Color.White);
            }
            else if (tile.WallColor == 3)
            {
                _spriteBatch.Draw(wallRed, tilePos, Color.White);
            }
            else if (tile.WallColor == 4)
            {
                _spriteBatch.Draw(wallBlue, tilePos, Color.Yellow);
            }
            else
            {
                _spriteBatch.Draw(wallYellow, tilePos, Color.LightPink);
            }
        }
    }
}
