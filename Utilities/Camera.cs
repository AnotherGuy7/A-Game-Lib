using AnotherLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AnotherLib.Utilities
{
    public class Camera
    {
        public const float DefaultZoom = 3f;
        public const float MaxZoomOut = 5f;
        public const float MaxZoomIn = 2.5f;

        public Vector2 position;
        public Vector2 bounds;
        public float zoomStrength;
        public float previousZoomStrength;
        public Vector2 cameraOrigin;
        public float cameraRotation;
        public Matrix cameraMatrix;
        public bool cameraLocked = false;
        public bool zoomJustChanged = false;
        public ControlMode cameraControlMode;
        public float mouseFollowIntensity = 0.5f;

        public Vector2 cameraShakeOffset;
        public int cameraShakeStrength = 0;
        public int cameraShakeTimer = 0;

        private int cameraThrowTime;
        private int cameraThrowTimer;
        private Vector2 cameraThrowOffset;
        private Vector2 cameraThrowCameraPositionOffset;

        public enum ControlMode
        {
            None,
            Mouse,
            Keys
        }

        public Camera(Vector2 position, Vector2 bounds, ControlMode controlMode)
        {
            this.position = position;
            this.bounds = bounds;
            cameraControlMode = controlMode;
        }


        /// <summary>
        /// Changes the settings of the Camera to allow it show properly show UI screens.
        /// </summary>
        public void SetToUICamera()
        {
            zoomStrength = DefaultZoom;
            previousZoomStrength = DefaultZoom;
            cameraOrigin = Vector2.Zero;
            position = Vector2.Zero;
        }

        /// <summary>
        /// Changes the settings of the Camera to allow it to properly show the Player and World.
        /// </summary>
        public void SetToPlayerCamera()
        {
            zoomStrength = DefaultZoom;
            previousZoomStrength = DefaultZoom;
            cameraOrigin = new Vector2(GameScreen.resolutionWidth, GameScreen.resolutionHeight) / 2f;
            position = cameraOrigin / 3f;       //3f because of the Screen matrix scale
        }

        public void Update()
        {
            if (cameraLocked)
                return;

            ManageZoom();
            ManageCameraShake();
            ManageCameraThrow();
            ReframeCamera();
            UpdateCameraView();
        }

        public void ThrowCamera(Vector2 cameraOffset, int time)
        {
            cameraShakeTimer = 0;
            cameraThrowTime = time;
            cameraThrowOffset = cameraOffset;
        }

        public void ShakeCamera(int shakeStrength, int duration)
        {
            cameraShakeTimer = duration;
            cameraShakeStrength = shakeStrength;
        }

        private void ManageCameraThrow()
        {
            if (cameraThrowTime <= 0)
                return;

            cameraThrowTimer++;
            float lerpValue = (float)Math.Sin(MathHelper.ToRadians(cameraThrowTimer / (float)cameraThrowTime * 180));
            cameraThrowCameraPositionOffset = Vector2.Lerp(Vector2.Zero, cameraThrowOffset, lerpValue);

            if (cameraThrowTimer >= cameraThrowTime)
            {
                cameraThrowTime = 0;
                cameraThrowTimer = 0;
                cameraThrowOffset = Vector2.Zero;
                cameraThrowCameraPositionOffset = Vector2.Zero;
            }
        }

        private void ManageCameraShake()
        {
            if (cameraShakeTimer > 0)
            {
                cameraShakeTimer--;
                float offsetX = GameData.random.Next(-cameraShakeStrength, cameraShakeStrength + 1);
                float offsetY = GameData.random.Next(-cameraShakeStrength, cameraShakeStrength + 1);
                position += new Vector2(offsetX, offsetY);
            }
        }

        public void UpdateCamera(Vector2 cameraPosition)
        {
            position = cameraPosition;

            position.X = MathHelper.Clamp(position.X, GameScreen.halfScreenWidth / zoomStrength + 1, bounds.X - GameScreen.halfScreenWidth / zoomStrength - 1);
            position.Y = MathHelper.Clamp(position.Y, GameScreen.halfScreenHeight / zoomStrength + 1, bounds.Y - GameScreen.halfScreenHeight / zoomStrength - 1);
            position += cameraThrowCameraPositionOffset;

            if (cameraControlMode == ControlMode.Keys)
            {
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.Up))
                    position.Y -= 15f;
                else if (keyboard.IsKeyDown(Keys.Down))
                    position.Y += 15f;

                if (keyboard.IsKeyDown(Keys.Left))
                    position.X -= 15f;
                else if (keyboard.IsKeyDown(Keys.Right))
                    position.X += 15f;
            }
            else if (cameraControlMode == ControlMode.Mouse)
            {
                Vector2 mouseOffset = GameData.MouseWorldPosition - position;
                mouseOffset.Normalize();
                mouseOffset *= Vector2.Distance(GameData.MouseWorldPosition, position) * 24f / GameScreen.resolutionWidth;
                position += mouseOffset * mouseFollowIntensity;
            }
        }

        public void ReframeCamera()
        {
            //Fixed that you could look outside the dungeon when you move the camera to the dungeon wall (when there is the end of an dungeon and you move the camera using your mouse to that side)
            //and fixes the posiblity of this happening during camera shake
            position.X = MathHelper.Clamp(position.X, GameScreen.halfScreenWidth / zoomStrength + 1, bounds.X - GameScreen.halfScreenWidth / zoomStrength - 1);
            position.Y = MathHelper.Clamp(position.Y, GameScreen.halfScreenHeight / zoomStrength + 1, bounds.Y - GameScreen.halfScreenHeight / zoomStrength - 1);
        }

        private void ManageZoom()
        {
            if (GameInput.IsZoomOutPressed() && zoomStrength > MaxZoomIn)
                zoomStrength -= 0.01f;
            if (GameInput.IsZoomInPressed() && zoomStrength < MaxZoomOut)
                zoomStrength += 0.01f;
            zoomStrength = MathHelper.Clamp(zoomStrength, MaxZoomIn, MaxZoomOut);

            if (previousZoomStrength != zoomStrength)
            {
                previousZoomStrength = zoomStrength;
            }
        }

        public void UpdateCameraView()
        {
            cameraMatrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateRotationZ(cameraRotation) *
                Matrix.CreateScale(zoomStrength) * Matrix.CreateTranslation(new Vector3(cameraOrigin.X, cameraOrigin.Y, 0));
        }
    }
}
