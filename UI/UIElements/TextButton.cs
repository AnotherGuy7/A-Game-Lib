using AnotherLib.Input;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnotherLib.UI.UIElements
{
    public class TextButton : UIObject
    {
        public Texture2D texture;
        public Color drawColor;
        public Vector2 buttonPosition;
        public bool buttonPressed = false;
        public bool buttonHover = false;
        public bool buttonFocus = false;        //For controllers
        public int buttonWidth = 0;
        public int buttonHeight = 0;
        public Text buttonText;

        private float scale;
        private float defaultScale;
        private float hoverScale;
        private Rectangle hitbox;
        private Color idleColor;
        private Color hoverColor;
        private bool drawPanel;
        private Vector2 buttonOrigin;
        private Vector2 buttonCenter;
        private bool centered = false;
        private bool playedSound = false;
        private readonly Vector2 ConstantOffset = new Vector2(-1.5f, -2f);

        public TextButton(SpriteFont font, string text, Vector2 position, float defaultScale, float hoverScale, Color idleColor, Color hoverColor, bool drawPanel = false, bool centeredOrigin = false)
        {
            buttonText = new Text(font, text, position, idleColor, centerOrigin: centeredOrigin);
            //buttonText.position.Y -= buttonText.size.Y;
            int originalWidth = (int)buttonText.size.X * 2;
            int originalHeight = (int)buttonText.size.Y * 2;
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(originalWidth * defaultScale) + 4, (int)(originalHeight * defaultScale) + 4);
            UISize = new Vector2(originalWidth, originalHeight);

            this.defaultScale = defaultScale;
            this.hoverScale = hoverScale;
            buttonPosition = position;
            this.idleColor = idleColor;
            this.hoverColor = hoverColor;
            buttonWidth = (int)(originalWidth * defaultScale);
            buttonHeight = (int)(originalHeight * defaultScale);
            texture = TextureGenerator.CreatePanelTexture(originalWidth + 4, originalHeight + 4, 1, Color.Black, Color.White);
            this.drawPanel = drawPanel;
            if (centeredOrigin)
            {
                buttonCenter = buttonPosition + (new Vector2(buttonWidth, buttonHeight) / 2f);
                buttonOrigin = (new Vector2(buttonWidth, buttonHeight) / 2f);
                buttonText.position += buttonOrigin;
            }
        }

        public TextButton(string text, Vector2 position, Vector2 customTextOffset, float defaultScale, float hoverScale, Color idleColor, Color hoverColor, bool drawPanel = false, bool centeredOrigin = false)
        {
            buttonText = new Text(text, position, idleColor, customTextOffset, centerOrigin: centeredOrigin);
            //buttonText.position.Y -= buttonText.size.Y;
            int originalWidth = (int)buttonText.size.X * 2;
            int originalHeight = (int)buttonText.size.Y * 2;
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(originalWidth * defaultScale) + 4, (int)(originalHeight * defaultScale) + 4);
            UISize = new Vector2(originalWidth, originalHeight);

            this.defaultScale = defaultScale;
            this.hoverScale = hoverScale;
            buttonPosition = position;
            this.idleColor = idleColor;
            this.hoverColor = hoverColor;
            buttonWidth = (int)(originalWidth * defaultScale);
            buttonHeight = (int)(originalHeight * defaultScale);
            texture = TextureGenerator.CreatePanelTexture(originalWidth + 4, originalHeight + 4, 1, Color.Black, Color.White);
            this.drawPanel = drawPanel;
        }

        public void ReInitializeTextPositions()
        {
            buttonText.position = buttonPosition;
        }

        public override void Update()
        {
            scale = defaultScale;
            drawColor = idleColor;
            buttonHover = false;
            buttonPressed = false;
            if (hitbox.Contains(GameData.MouseScreenPosition.ToPoint()) || buttonFocus)
            {
                scale = hoverScale;
                buttonHover = true;
                drawColor = hoverColor;
                GameData.MouseOverUI = true;
                if (GameInput.IsUIConfirmPressed())
                    buttonPressed = true;
            }

            buttonText.drawColor = drawColor;
            buttonText.scale = scale;
            buttonFocus = false;

            hitbox.X = (int)buttonPosition.X - 2;
            hitbox.Y = (int)buttonPosition.Y - 2;
            buttonText.position = buttonPosition + buttonText.positionOffset;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawPanel)
                spriteBatch.Draw(texture, buttonPosition + buttonOrigin + ConstantOffset, null, drawColor, 0f, buttonOrigin, scale, SpriteEffects.None, 0f);

            buttonText.Draw(spriteBatch);
        }
    }
}
