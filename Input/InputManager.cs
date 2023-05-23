using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnotherLib.Input
{
    public class InputManager
    {
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;
        public static MouseState currentMouseState;
        public static MouseState previousMouseState;
        public static GamePadState currentGamepadState;
        public static GamePadState previousGamepadState;

        public static bool MouseLeftPressed;
        public static bool MouseRightPressed;

        public static Keys upKey = Keys.W;
        public static Keys downKey = Keys.S;
        public static Keys leftKey = Keys.A;
        public static Keys rightKey = Keys.D;
        public static Keys pickUpKey = Keys.Q;

        /*
        Square = A
        Triangle = 
        Cross = 
        Circle = 
        */
        public static Buttons confirmButton = Buttons.A;        //Square on PS
        public static Buttons backbutton = Buttons.Y;

        public static Buttons movingAnalog = Buttons.LeftStick;
        public static Buttons lookingAnalong = Buttons.RightStick;
        public static Buttons attackButton = Buttons.RightTrigger;
        public static Buttons pickUpButton = Buttons.B;

        //Keyboard inputs
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyJustPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyJustReleased(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
        }

        public static bool IsMouseLeftJustPressed()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsMouseLeftJustReleased()
        {
            return currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsMouseLeftHeld()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsMouseRightJustPressed()
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }

        public static bool IsMouseRightJustReleased()
        {
            return currentMouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsMouseRightHeld()
        {
            return currentMouseState.RightButton == ButtonState.Pressed;
        }

        //Controller inputs
        public static bool IsControllerUpHeld(Buttons button)
        {
            if (button == Buttons.LeftStick)
                return currentGamepadState.ThumbSticks.Left.Y < -0.1f;
            else
                return currentGamepadState.ThumbSticks.Right.Y < -0.1f;
        }

        public static bool IsControllerLeftHeld(Buttons button)
        {
            if (button == Buttons.LeftStick)
                return currentGamepadState.ThumbSticks.Left.X < -0.1f;
            else
                return currentGamepadState.ThumbSticks.Right.X < -0.1f;
        }

        public static bool IsControllerDownHeld(Buttons button)
        {
            if (button == Buttons.LeftStick)
                return currentGamepadState.ThumbSticks.Left.Y > 0.1f;
            else
                return currentGamepadState.ThumbSticks.Right.Y > 0.1f;
        }

        public static bool IsControllerRightHeld(Buttons button)
        {
            if (button == Buttons.LeftStick)
                return currentGamepadState.ThumbSticks.Left.X > 0.1f;
            else
                return currentGamepadState.ThumbSticks.Right.X > 0.1f;
        }

        public static bool IsAnalogJustMoved(Buttons analog)
        {
            if (analog == Buttons.LeftStick)
                return currentGamepadState.ThumbSticks.Left.X > 0.1f && previousGamepadState.ThumbSticks.Left.X <= 0.1f;
            else
                return currentGamepadState.ThumbSticks.Right.X > 0.1f;
        }

        public static bool IsButtonPressed(Buttons button)
        {
            return currentGamepadState.IsButtonDown(button);
        }

        public static bool IsButtonJustPressed(Buttons button)
        {
            return currentGamepadState.IsButtonDown(button) && previousGamepadState.IsButtonUp(button);
        }

        public static bool IsButtonJustReleased(Buttons button)
        {
            return currentGamepadState.IsButtonUp(button) && previousGamepadState.IsButtonDown(button);
        }

        public static void UpdateControlsState()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            previousGamepadState = currentGamepadState;
            currentGamepadState = GamePad.GetState(PlayerIndex.One);


            GameData.MouseScreenPosition = currentMouseState.Position.ToVector2() / GameData.mouseScreenDivision;
            if (!GameInput.ControllerConnected)
            {
                GameData.MouseWorldPosition = Vector2.Transform(currentMouseState.Position.ToVector2(), Matrix.Invert(GameData.CurrentGameDrawInfo.matrix));
            }
            else
            {
                Vector2 rightAnalogVector = GameInput.GetRightAnalogVector();
                Vector2 matrixCenter = new Vector2(GameScreen.halfScreenWidth, GameScreen.halfScreenHeight) * 1.00001f;     //Otherwise you'll get a NaN, because the origin of the matrix is the same.
                GameData.MouseWorldPosition = Vector2.Transform(matrixCenter + rightAnalogVector * GameScreen.halfScreenHeight / 11f * 10f, Matrix.Invert(GameData.CurrentGameDrawInfo.matrix));
            }
            
            MouseLeftPressed = currentMouseState.LeftButton == ButtonState.Pressed;
            MouseRightPressed = currentMouseState.RightButton == ButtonState.Pressed;
        }
    }
}
