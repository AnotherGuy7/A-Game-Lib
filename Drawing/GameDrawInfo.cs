using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static AnotherLib.Drawing.SpriteBatchData;

namespace AnotherLib.Drawing
{
    /// <summary>
    /// A class containing information on the way the game is being drawn.
    /// </summary>
    public class GameDrawInfo
    {
        public SpriteBatchParameterData spriteBatchData;
        public Matrix matrix;
    }

    public static class SpritebatchGameDrawInfo
    {
        /// <summary>
        /// Starts a spriteBatch Batch using the provided parameters.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="info">The draw info to pass in. This data contains spritebatch info and matrix information.</param>
        public static void Begin(this SpriteBatch spriteBatch, GameDrawInfo info)
        {
            spriteBatch.Begin(info.spriteBatchData.sortMode, info.spriteBatchData.blendState, info.spriteBatchData.samplerState, info.spriteBatchData.stencilState, info.spriteBatchData.rasterizerState, info.spriteBatchData.effect, info.matrix);
        }

        /// <summary>
        /// Starts a spriteBatch Batch using the provided parameters.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="info">The draw info to pass in. This data contains spritebatch info and matrix information.</param>
        /// <param name="effect">The custom effect to pass in to replace the spriteBatch data's effect.</param>
        public static void Begin(this SpriteBatch spriteBatch, GameDrawInfo info, Effect effect)
        {
            spriteBatch.Begin(info.spriteBatchData.sortMode, info.spriteBatchData.blendState, info.spriteBatchData.samplerState, info.spriteBatchData.stencilState, info.spriteBatchData.rasterizerState, effect, info.matrix);
        }
    }
}
