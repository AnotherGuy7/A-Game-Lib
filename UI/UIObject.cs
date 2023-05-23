using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.UI
{
    public abstract class UIObject
    {
        public virtual int UIWidth { get; }
        public virtual int UIHeight { get; }

        public readonly int TopLeft = 0;
        public readonly int TopRight = 1;
        public readonly int Center = 2;
        public readonly int BottomLeft = 3;
        public readonly int BottomRight = 4;
        public Vector2[] anchors = new Vector2[5];      //These are anchors for UI objects, not the screen.

        public Vector2 UISize;
        public int buttonIndex = -1;
        public UIObject[] uiButtons;
        public bool controllerConfirmPressed = false;

        public virtual void Initialize()
        { }

        public virtual void ReInitializePositions()
        { }

        /// <summary>
        /// Sets the anchors for a UI object.
        /// </summary>
        /// <param name="position">The position of the UI object</param>
        /// <param name="centered">Whether or not the object is centered</param>
        public void PositionAnchors(Vector2 position, bool centered = false)
        {
            anchors = new Vector2[5];
            UISize = new Vector2(UIWidth, UIHeight);

            if (centered)
            {
                anchors[TopLeft] = position - (UISize / 2f);
                anchors[TopRight] = position + (new Vector2(UISize.X, -UISize.Y) / 2f);
                anchors[Center] = position;
                anchors[BottomLeft] = position - (new Vector2(UISize.X, -UISize.Y) / 2f);
                anchors[BottomRight] = position + (UISize / 2f);
            }
            else
            {
                anchors[TopLeft] = position;
                anchors[TopRight] = position + (new Vector2(UISize.X, 0f));
                anchors[Center] = position + (UISize / 2f);
                anchors[BottomLeft] = position + new Vector2(0f, UISize.Y);
                anchors[BottomRight] = position + UISize;
            }
        }

        /// <summary>
        /// Updates the UI for controller input.
        /// </summary>
        public virtual void UpdateControllerUI()
        { }

        public virtual void Update()
        { }

        public virtual void Draw(SpriteBatch spriteBatch)
        { }
    }
}
