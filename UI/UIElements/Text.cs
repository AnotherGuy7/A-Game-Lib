using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.UI.UIElements
{
    public class Text : UIObject
    {
        public string text;
        public Vector2 position;
        public Color drawColor;
        public SpriteFont spriteFont;
        public float scale;
        public Vector2 origin;
        public readonly Vector2 positionOffset;
        public readonly Vector2 size;

        //Perhaps text alignment support may be useful in the future.
        public Text(SpriteFont font, string text, Vector2 textPosition, Color textColor, float textScale = 0.5f, bool centerOrigin = false)
        {
            this.text = text;
            position = textPosition;
            drawColor = textColor;
            spriteFont = font;
            scale = textScale;

            Vector2 fontSize = font.MeasureString(text);
            if (centerOrigin)
                origin = fontSize / 2f;
            else
                origin = new Vector2(0f, 8.5f);

            size = fontSize * scale;
            positionOffset = new Vector2(0f, (size.Y / 2f) + (2 * scale));
            position.Y += positionOffset.Y;
        }

        public Text(string text, Vector2 textPosition, Color textColor, Vector2 customOffset, float textScale = 0.5f, bool centerOrigin = false)
        {
            this.text = text;
            position = textPosition;
            drawColor = textColor;
            scale = textScale;

            Vector2 fontSize = spriteFont.MeasureString(text);
            if (centerOrigin)
                origin = fontSize / 2f;
            else
                origin = new Vector2(0f, 8.5f);

            size = fontSize * scale;
            positionOffset = customOffset;
            position += positionOffset;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, position, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
