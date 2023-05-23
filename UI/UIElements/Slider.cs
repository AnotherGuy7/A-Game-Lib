using AnotherLib.Input;
using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.UI.UIElements
{
    public class Slider : UIObject
    {
        public static Texture2D sliderButtonTexture;

        private const float TextScale = 0.3f;

        public float sliderValue { get; set; }
        private bool clickedOn = false;

        public Text sliderName;
        public Color drawColor;
        public Vector2 sliderPosition;      //Position of the actual slider button
        public Vector2 position;      //Position of the whole bar
        public Rectangle rect;
        private Vector2 previousPosition;

        public Vector2 offset = new Vector2(0, 0);

        public Slider(SpriteFont font, Vector2 position, int width, int height, Color color, string label = "", float defaultValue = 0f)
        {
            this.position = position;
            rect = new Rectangle((int)position.X, (int)position.Y, width, height);
            drawColor = color;
            sliderPosition.Y = position.Y + (height / 2f);
            sliderName = new Text(font, label, position, color, TextScale);
            //sliderName.position += new Vector2(-sliderName.size.X - 1f, 3f);
            previousPosition = position;

            UISize = new Vector2(width, height);
            SetValue(defaultValue);
        }

        public override void Update()
        {
            rect.X = (int)position.X;
            rect.Y = (int)position.Y;

            if (rect.Contains(GameData.MouseScreenPosition.ToPoint()))
            {
                GameData.MouseOverUI = true;
                if (InputManager.IsMouseLeftHeld())
                    clickedOn = true;
            }
            if (clickedOn)
            {
                if (InputManager.IsMouseLeftJustReleased())
                {
                    clickedOn = false;
                    return;
                }

                sliderPosition.X = MathHelper.Clamp(GameData.MouseScreenPosition.X, position.X, position.X + rect.Width);
                sliderValue = (sliderPosition.X - position.X) / (float)rect.Width;
            }

            if (previousPosition != position)
            {
                sliderPosition.Y = position.Y + (rect.Height / 2f);
                sliderName.position = position;
                sliderName.position += new Vector2(-sliderName.size.X - 1f, 2.5f);
                previousPosition = position;
                SetValue(sliderValue);
            }
        }

        public void SetValue(float value)
        {
            sliderPosition.X = position.X + (rect.Width * value);
            sliderValue = value;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(sliderButtonTexture.Width / 2f, sliderButtonTexture.Height / 2f) - offset;
            spriteBatch.Draw(sliderButtonTexture, sliderPosition, null, drawColor, 0f, origin, 1f, SpriteEffects.None, 0f);
            Texture2D texture = TextureGenerator.CreatePanelTexture(2, 2, 0, Color.White, Color.White, false);

            //Top line
            Vector2 topLinePosition = position + new Vector2(1f, 0f) + offset;
            Vector2 topLineScale = new Vector2(rect.Width - 2f, 1f);
            spriteBatch.Draw(texture, topLinePosition, null, drawColor, 0f, Vector2.Zero, topLineScale / 2f, SpriteEffects.None, 0f);

            //Left line
            Vector2 leftLinePosition = position + new Vector2(0f, 1f) + offset;
            Vector2 leftLineScale = new Vector2(1f, rect.Height - 2f);
            spriteBatch.Draw(texture, leftLinePosition, null, drawColor, 0f, Vector2.Zero, leftLineScale / 2f, SpriteEffects.None, 0f);

            //Right line
            Vector2 rightLinePosition = position + new Vector2(rect.Width - 1f, 1f) + offset;
            Vector2 rightLineScale = new Vector2(1f, rect.Height - 2f);
            spriteBatch.Draw(texture, rightLinePosition, null, drawColor, 0f, Vector2.Zero, rightLineScale / 2f, SpriteEffects.None, 0f);

            //Left line
            Vector2 bottomLinePosition = position + new Vector2(1f, rect.Height - 1f) + offset;
            Vector2 bottomLineScale = new Vector2(rect.Width - 2f, 1f);
            spriteBatch.Draw(texture, bottomLinePosition, null, drawColor, 0f, Vector2.Zero, bottomLineScale / 2f, SpriteEffects.None, 0f);

            sliderName.Draw(spriteBatch);
        }
    }
}
