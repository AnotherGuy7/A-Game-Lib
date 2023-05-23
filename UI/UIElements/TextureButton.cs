using AnotherLib.Input;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.UI.UIElements
{
    public class TextureButton : UIObject
    {
        public static Texture2D checkMarkTexture;
        public static Texture2D arrowTexture;

        public Texture2D texture;
        public Texture2D panelTexture;
        public Color drawColor;
        public Vector2 buttonPosition;
        public bool buttonPressed = false;
        public bool buttonPressedRightClick = false;
        public bool buttonHover = false;
        public int buttonWidth = 0;
        public int buttonHeight = 0;
        public string buttonText;
        public bool focused = false;
        public bool buttonFocus = false;
        public float scale;
        public bool drawTexture = true;

        private float defaultScale;
        private float hoverScale;
        private Rectangle hitbox;
        private Color idleColor;
        private Color hoverColor;
        private bool drawPanel;
        private Vector2 buttonSize;
        private bool playedSound = false;

        public enum ButtonType
        {
            Checkbox,
            Arrow
        }

        public TextureButton(Vector2 position, int width, int height, float defaultScale, float hoverScale, Color idleColor, Color hoverColor, ButtonType buttonType, bool drawPanel = false)
        {
            buttonWidth = width;
            buttonHeight = height;
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(width * defaultScale), (int)(height * defaultScale));
            this.defaultScale = defaultScale;
            this.hoverScale = hoverScale;
            buttonPosition = position;
            this.idleColor = idleColor;
            this.hoverColor = hoverColor;
            buttonSize = new Vector2(buttonWidth, buttonHeight);
            panelTexture = TextureGenerator.CreatePanelTexture(buttonWidth + 2, buttonHeight + 2, 1, Color.Black, Color.White);
            if (buttonType == ButtonType.Checkbox)
                texture = checkMarkTexture;
            else if (buttonType == ButtonType.Arrow)
                texture = arrowTexture;

            this.drawPanel = drawPanel;
        }

        public TextureButton(Texture2D texture, Vector2 position, int width, int height, float defaultScale, float hoverScale, Color idleColor, Color hoverColor, bool drawPanel = false)
        {
            buttonWidth = width;
            buttonHeight = height;
            hitbox = new Rectangle((int)position.X, (int)position.Y, (int)(width * defaultScale), (int)(height * defaultScale));
            this.defaultScale = defaultScale;
            this.hoverScale = hoverScale;
            buttonPosition = position;
            this.idleColor = idleColor;
            this.hoverColor = hoverColor;
            buttonSize = new Vector2(buttonWidth, buttonHeight);
            panelTexture = TextureGenerator.CreatePanelTexture(buttonWidth + 2, buttonHeight + 2, 1, Color.Black, Color.White);
            this.texture = texture;

            this.drawPanel = drawPanel;
        }

        public override void Update()
        {
            scale = defaultScale;
            drawColor = idleColor;
            buttonHover = false;
            buttonPressed = false;
            buttonPressedRightClick = false;
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
                if (InputManager.IsMouseRightJustPressed())
                    buttonPressedRightClick = true;
            }
            buttonFocus = false;

            hitbox.X = (int)buttonPosition.X;
            hitbox.Y = (int)buttonPosition.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawPanel)
                spriteBatch.Draw(panelTexture, buttonPosition, null, drawColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (drawTexture)
                spriteBatch.Draw(texture, buttonPosition + (Vector2.One * scale), null, drawColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
