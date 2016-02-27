using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GroupGame
{
    class GameObject
    {
        private Texture2D image;
        private Rectangle position;
        private Color color = Color.White;

        // Position property
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        // Image property
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        // Draw method
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(image, position, Color.White);
        }
    }
}
