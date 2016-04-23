// LeaderboardHandler
// Class to handle the name entry for our leaderboard
// Coders: Kiernan Brown

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
        // Attributes
        Keys[] previousPressedKeys;
        bool shift = false;
        bool done = false;
        private string name = "";

        // Name property
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // Done property
        public bool Done
        {
            get { return done; }
        }

        // Method to update the LeaderboardHandler
        public void Update()
        {
            // Get an array of the keys being pressed
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            // If shift is pressed, we set the shift bool to true and use caps
            if (previousPressedKeys.Contains(Keys.LeftShift) || previousPressedKeys.Contains(Keys.RightShift))
            {
                shift = true;
            }
            else shift = false;

            // Call the KeyPressed method for each key that is being pressed
            foreach (Keys key in pressedKeys)
            {
                if (previousPressedKeys.Contains(key) == false && done == false)
                {
                    KeyPressed(key);
                }
            }

            // Set the previousPressedKeys array
            previousPressedKeys = pressedKeys;
        }

        // KeyPressed method, adds to the name
        private void KeyPressed(Keys key)
        {
            switch (key)
            {
                // If the back key is pressed, delete the last character in the name
                case Keys.Back:
                    if (name.Length > 0) name = name.Remove(name.Length - 1);
                    break;
                // If the space key is pressed, add a space
                case Keys.Space:
                    name += " ";
                    break;
                // If the enter key is pressed, end the name entry
                case Keys.Enter: done = true;
                    break;
                // Otherwise, another key is pressed, add letters and numbers to the name
                default:
                    if (shift == false && IsKeyNumber(key)) name += key.ToString()[1]; // If the key is a number, add the character of the key that is the number to the name
                    else if (shift == false && (IsKeyLetter(key))) name += key.ToString().ToLower(); // If the key is a letter and shift isn't pressed, add the lowercase letter to the name
                    else if (IsKeyLetter(key)) name += key; // If the key is a letter and shift is pressed, add the lowercase letter to the name
                    break;
            }
        }

        // IsKeyLetter method, checks if a key is a letter key
        public bool IsKeyLetter(Keys key)
        {
            char c;
            char.TryParse(key.ToString(), out c);
            if (char.IsLetter(c)) return true;
            return false;
        }

        // IsKeyNumber method, checks if a key is a number key
        public bool IsKeyNumber(Keys key)
        {
            return ((key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9));
        }

        // Constructor
        public LeaderboardHandler()
        {
            previousPressedKeys = new Keys[0];
        }
    }
}
