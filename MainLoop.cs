using Gonk_Konk_Complete_Edition.GameLoops;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;
using System.Text;

namespace Gonk_Konk_Complete_Edition
{
    public class MainLoop : Game
    {
        private enum GameState
        {
            Menu,
            Gaming
        }


        public GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont spriteFont;

        private GameState gameState = GameState.Menu;
        private MenuLoop _menuLoop;
        private GamingLoop _gameLoop;

        bool wasClicked = false;
        bool isClicked = false;

        public MainLoop()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _menuLoop = new MenuLoop();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _menuLoop.Initialize();


            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            spriteFont = Content.Load<SpriteFont>("MenuContent/ComicSans");
            _menuLoop.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // the update stuff for the menu
            if (gameState == GameState.Menu)
            {
                //yes i know this is an awful way to make a button but it was quicker for this instance to do this
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) isClicked = true;
                else isClicked = false;

                if (!wasClicked && isClicked)
                {
                    if (Mouse.GetState().X > 60 && Mouse.GetState().Y > 195 &&
                        Mouse.GetState().X < 150 && Mouse.GetState().Y < 255)
                    {
                        gameState = GameState.Gaming;
                        _gameLoop = new GamingLoop();
                        _gameLoop.Initialize(Content);

                        spriteFont = Content.Load<SpriteFont>("GameContent/ComicSansMS30");
                        _gameLoop.LoadContent(Content);

                        _graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
                        _graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
                        _graphics.ApplyChanges();
                    }
                }
                wasClicked = isClicked;    

                //go into the menu update class
                _menuLoop.Update(gameTime);

                
            }
            else //gamestate is gaming
            {
                _gameLoop.Update(gameTime, Content);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.Black);
            
            if(gameState == GameState.Menu)
                _menuLoop.Draw(gameTime, _spriteBatch, spriteFont);
            else
            {
                _gameLoop.Draw(gameTime, _spriteBatch, spriteFont);
            }


            base.Draw(gameTime);
        }
    }
}
