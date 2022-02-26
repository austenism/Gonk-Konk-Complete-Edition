using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gonk_Konk_Complete_Edition.Sprites.MenuSprites
{
    public class GenericSprite
    {
        private Texture2D texture;
        private string textureName = "";
        /// <summary>
        /// position of the sprite
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// size in pixels of the sprite
        /// </summary>
        private Vector2 Size;
        /// <summary>
        /// scale of the sprite
        /// </summary>
        public float Scale = 1;
        /// <summary>
        /// whether flipped or not
        /// </summary>
        public bool Flipped = false;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="textureName">name of the image for the sprite</param>
        /// <param name="position">position on the screen</param>
        /// <param name="sizeX">size in pixels of sprite image</param>
        /// <param name="sizeY">size in pixels of sprite image/></param>
        public GenericSprite(string textureName, Vector2 position, int sizeX, int sizeY)
        {
            this.textureName = textureName;
            Position = position;
            Size.X = sizeX;
            Size.Y = sizeY;
        }
        public GenericSprite(string textureName, Vector2 position, int sizeX, int sizeY, float scale, bool flipped)
        {
            this.textureName = textureName;
            Position = position;
            Size.X = sizeX;
            Size.Y = sizeY;
            Scale = scale;
            Flipped = flipped;
        }
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MenuContent/" + textureName);
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Rectangle rect = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
            SpriteEffects flip = SpriteEffects.None;
            if (Flipped) flip = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, Position, rect, Color.White, 0, new Vector2(Size.X / 2, Size.Y / 2), Scale, flip, 1);
        }
        //Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth
    }
}
