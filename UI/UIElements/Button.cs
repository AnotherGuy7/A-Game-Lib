using AnotherLib.Input;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.UI.UIElements
{
    public class Button : UIObject
    {
        public Texture2D texture;
        public Color drawColor;
        public Vector2 buttonPosition;
        public bool buttonPressed = false;
        public bool buttonHover = false;
        public int buttonWidth = 0;
        public int buttonHeight = 0;
        public string buttonText;
        public bool focused = false;
        public bool buttonFocus = false;
        public float scale;

        private float defaultScale;
        private float hoverScale;
        private Rectangle hitbox;
        private Color idleColor;
        private Color hoverColor;
        private bool drawPanel;

        public Button(Vector2 position, int width, int height, float defaultScale, float hoverScale, Color idleColor, Color hoverColor, bool drawPanel = false)
        {
            buttonWidth = width;
            buttonHeight = height;
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(width * defaultScale), (int)(height * defaultScale));
            this.defaultScale = defaultScale;
            this.hoverScale = hoverScale;
            buttonPosition = position;
            this.idleColor = idleColor;
            this.hoverColor = hoverColor;
            texture = TextureGenerator.CreatePanelTexture(buttonWidth, buttonHeight, 1, Color.Black, Color.White);
            this.drawPanel = drawPanel;
        }

        public override void Update()
        {
            scale = defaultScale;
            drawColor = idleColor;
            buttonHover = false;
            buttonPressed = false;
            if (focused)
            {
                scale = hoverScale;
                buttonHover = true;
                drawColor = hoverColor;
            }
            if (hitbox.Contains(GameData.MouseScreenPosition.ToPoint()) || buttonFocus)
            {
                scale = hoverScale;
                buttonHover = true;
                drawColor = hoverColor;
                GameData.MouseOverUI = true;
                if (GameInput.IsUIConfirmPressed())
                    buttonPressed = true;
            }
            buttonFocus = false;


            hitbox.X = (int)buttonPosition.X;
            hitbox.Y = (int)buttonPosition.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawPanel)
                spriteBatch.Draw(texture, buttonPosition, null, drawColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
