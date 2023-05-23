using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static AnotherLib.Input.InputManager;

namespace AnotherLib.Input
{
    public class GameInput
    {
        public static bool ControllerConnected => currentGamepadState.IsConnected;

        public static bool IsUpPressed()
        {
            return IsKeyPressed(upKey) || IsControllerUpHeld(movingAnalog);
        }

        public static bool IsUpJustPressed()
        {
            return IsKeyJustPressed(upKey) || IsControllerUpHeld(movingAnalog) && IsAnalogJustMoved(movingAnalog);
        }

        public static bool IsDownPressed()
        {
            return IsKeyPressed(downKey) || IsControllerDownHeld(movingAnalog);
        }

        public static bool IsDownJustPressed()
        {
            return IsKeyJustPressed(downKey) || IsControllerDownHeld(movingAnalog) && IsAnalogJustMoved(movingAnalog);
        }

        public static bool IsLeftPressed()
        {
            return IsKeyPressed(leftKey) || IsControllerLeftHeld(movingAnalog);
        }

        public static bool IsLeftJustPressed()
        {
            return IsKeyJustPressed(leftKey) || IsControllerLeftHeld(movingAnalog) && IsAnalogJustMoved(movingAnalog);
        }

        public static bool IsRightPressed()
        {
            return IsKeyPressed(rightKey) || IsControllerRightHeld(movingAnalog);
        }

        public static bool IsRightJustPressed()
        {
            return IsKeyJustPressed(rightKey) || IsControllerRightHeld(movingAnalog) && IsAnalogJustMoved(movingAnalog);
        }

        public static bool IsAttackHeld()
        {
            return IsMouseLeftHeld() || IsButtonPressed(attackButton);
        }

        public static bool IsAttackJustPressed()
        {
            return IsMouseLeftJustReleased() || IsButtonJustPressed(attackButton);
        }

        public static bool IsPickUpJustPressed()
        {
            return IsKeyJustPressed(pickUpKey) || IsButtonJustPressed(pickUpButton);
        }

        public static bool IsZoomInPressed()
        {
            return IsKeyPressed(Keys.OemPlus) || IsKeyPressed(Keys.Add);
        }

        public static bool IsZoomOutPressed()
        {
            return IsKeyPressed(Keys.OemMinus) || IsKeyPressed(Keys.Subtract);
        }

        public static bool IsUIConfirmPressed()
        {
            return IsMouseLeftJustPressed() || IsButtonJustPressed(Buttons.A);
        }

        public static bool IsUIBackPressed()
        {
            return IsKeyJustPressed(Keys.Escape) || IsButtonJustPressed(backbutton);
        }

        public static bool IsUIUpPressed()
        {
            return IsButtonJustPressed(Buttons.DPadUp);
        }

        public static bool IsUIDownPressed()
        {
            return IsButtonJustPressed(Buttons.DPadDown);
        }

        public static bool IsUILeftPressed()
        {
            return IsButtonJustPressed(Buttons.DPadLeft);
        }

        public static bool IsUIRightPressed()
        {
            return IsButtonJustPressed(Buttons.DPadRight);
        }

        public static Vector2 GetLeftAnalogVector()
        {
            Vector2 leftAnalogVector = currentGamepadState.ThumbSticks.Left;
            leftAnalogVector.Y *= -1;
            return leftAnalogVector;
        }

        public static Vector2 GetRightAnalogVector()
        {
            Vector2 rightAnalogVector = currentGamepadState.ThumbSticks.Right;
            rightAnalogVector.Y *= -1;
            return rightAnalogVector;
        }
    }
}
