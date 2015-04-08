using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class Input
    {
        Dictionary<Keys, bool> heldKeys;
        Dictionary<Keys, bool> pressedKeys;
        Dictionary<Keys, bool> releasedKeys;

        public Input()
        {
            heldKeys = new Dictionary<Keys, bool>();
            pressedKeys = new Dictionary<Keys, bool>();
            releasedKeys = new Dictionary<Keys, bool>();
        }

        public void BeginNewFrame()
        {
            pressedKeys.Clear();
            releasedKeys.Clear();
        }

        public void KeyDownEvent(KeyboardState keyboardState)
        {
            Keys[] pressed = keyboardState.GetPressedKeys();
            foreach (Keys key in pressed)
            {
                if (!pressedKeys.ContainsKey(key))
                {
                    pressedKeys.Add(key, true);
                }
                if (!heldKeys.ContainsKey(key))
                {
                    heldKeys.Add(key, true);
                }
            }
        }

        public void KeyUpEvent(KeyboardState keyboardState)
        {
            List<Keys> pressed = keyboardState.GetPressedKeys().ToList();
            foreach (KeyValuePair<Keys, bool> key in heldKeys)
            {
                if (!pressed.Contains(key.Key))
                {
                    heldKeys[key.Key] = false;
                    if (!releasedKeys.ContainsKey(key.Key))
                    {
                        releasedKeys.Add(key.Key, true);
                    }
                }
            }
        }

        public bool WasKeyPressed(Keys key)
        {
            if (pressedKeys.ContainsKey(key))
            {
                return pressedKeys[key];
            }
            return false;
        }

        public bool WasKeyReleased(Keys key)
        {
            if (releasedKeys.ContainsKey(key))
            {
                return releasedKeys[key];
            }
            return false;
        }

        public bool IsKeyHeld(Keys key)
        {
            if (heldKeys.ContainsKey(key))
            {
                return heldKeys[key];
            }
            return false;
        }
    }
}
