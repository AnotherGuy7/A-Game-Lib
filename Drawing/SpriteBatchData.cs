using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.Drawing
{
    public static class SpriteBatchData
    {
        public struct SpriteBatchParameterData
        {
            public SpriteSortMode sortMode;
            public BlendState blendState;
            public SamplerState samplerState;
            public DepthStencilState stencilState;
            public RasterizerState rasterizerState;
            public Effect effect;
        }

        /// <summary>
        /// Starts a spriteBatch Batch using the provided parameters. No matrix is passed in.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="data">The parameters to pass in</param>
        public static void Begin(this SpriteBatch spriteBatch, SpriteBatchParameterData data)
        {
            spriteBatch.Begin(data.sortMode, data.blendState, data.samplerState, data.stencilState, data.rasterizerState, data.effect, null);
        }

        /// <summary>
        /// Starts a spriteBatch Batch using the provided parameters.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="data">The parameters to pass in</param>
        /// <param name="matrix">The view matrix the spriteBatch will use.</param>
        public static void Begin(this SpriteBatch spriteBatch, SpriteBatchParameterData data, Matrix matrix)
        {
            spriteBatch.Begin(data.sortMode, data.blendState, data.samplerState, data.stencilState, data.rasterizerState, data.effect, matrix);
        }

        /// <summary>
        /// Starts a spriteBatch Batch using the provided parameters.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="data">The parameters to pass in</param>
        /// <param name="effect">The custom effect to pass in to replace the spriteBatch data's effect.</param>
        /// <param name="matrix">The view matrix the spriteBatch will use.</param>
        public static void Begin(this SpriteBatch spriteBatch, SpriteBatchParameterData data, Effect effect, Matrix matrix)
        {
            spriteBatch.Begin(data.sortMode, data.blendState, data.samplerState, data.stencilState, data.rasterizerState, effect, matrix);
        }
    }
}
