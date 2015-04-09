using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Input
    {
        KeyboardState keyboardState;
        KeyboardState previousKeyboardState;

        public Input()
        {
            previousKeyboardState = Keyboard.GetState();
        }

        public void BeginFrame()
        {
            keyboardState = Keyboard.GetState();
        }

        public void EndFrame()
        {
            previousKeyboardState = keyboardState;
        }

        public bool WasKeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool WasKeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key);
        }
    }
}
