﻿// Character1
// Class for the first character, inherits from Character
// Coders: Kiernan Brown

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class Character1:Character
    {
        // Constructor
        public Character1(int x, int y):base(x,y)
        {
            Position = new Rectangle(x, y, 50, 50);
            CRect = new Rectangle(x - 15, y - 15, Position.Width - 30, Position.Height - 30);
            Speed = 3;
        }
    }
}
