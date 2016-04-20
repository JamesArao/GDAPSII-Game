using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GroupGame
{
    class LeaderboardHandler
    {
        Keys[] previousPressedKeys;
        bool shift = false;
        bool done = false;
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool Done
        {
            get { return done; }
        }

        public void Update()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            if (previousPressedKeys.Contains(Keys.LeftShift) || previousPressedKeys.Contains(Keys.RightShift))
            {
                shift = true;
            }
            else shift = false;

            foreach (Keys key in pressedKeys)
            {
                if (previousPressedKeys.Contains(key) == false && done == false)
                {
                    KeyPressed(key);
                }
            }
            previousPressedKeys = pressedKeys;
        }

        private void KeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    if (name.Length > 0) name = name.Remove(name.Length - 1);
                    break;
                case Keys.Space:
                    name += " ";
                    break;
                case Keys.Enter: done = true;
                    break;
                default:
                    if (shift == false && IsKeyNumber(key)) name += key.ToString()[1];
                    else if (shift == false && (IsKeyLetter(key))) name += key.ToString().ToLower();
                    else if (IsKeyLetter(key)) name += key;
                    break;
            }
        }

        public bool IsKeyLetter(Keys key)
        {
            char c;
            char.TryParse(key.ToString(), out c);
            if (char.IsLetter(c)) return true;
            return false;
        }

        public bool IsKeyNumber(Keys key)
        {
            return ((key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9));
        }

        public LeaderboardHandler()
        {
            previousPressedKeys = new Keys[0];
        }
    }
}
