using AnotherLib.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnotherLib.Drawing
{
    public class ShaderBatch
    {
        public static List<DrawData> queuedDraws;
        public static SpriteBatch spriteBatch;
        public static Dictionary<byte, Effect> activeShaderEffects;

        public struct DrawData
        {
            public Texture2D texture;
            public Vector2 position;
            public Rectangle rect;
            public float rotation;
            public Vector2 origin;
            public float scale;
            public SpriteEffects spriteEffect;
            public byte shaderID;
        }

        public ShaderBatch(Dictionary<byte, Effect> effects)
        {
            activeShaderEffects = effects;
        }

        public static void InitializeShaderBatchLists()
        {
            queuedDraws = new List<DrawData>();
        }

        public static void DrawQueuedShaderDraws()
        {
            if (queuedDraws.Count <= 0)
                return;

            byte[] shaderKeys = activeShaderEffects.Keys.ToArray();
            for (byte i = 0; i < activeShaderEffects.Count; i++)
            {
                DrawData[] shaderedItems = SortEffectBatch(shaderKeys[i]);
                DrawShaderItems(shaderedItems, activeShaderEffects[shaderKeys[i]]);
            }

            queuedDraws.Clear();
        }

        /// <summary>
        /// Creates a list of draw datas using this effect.
        /// </summary>
        /// <param name="effect">The effect to be used as a sorting requirement.</param>
        /// <returns></returns>
        private static DrawData[] SortEffectBatch(byte shaderID)
        {
            int amountOfMatches = 0;
            bool[] effectMatches = new bool[queuedDraws.Count];
            for (int i = 0; i < queuedDraws.Count; i++)
            {
                if (queuedDraws[i].shaderID == shaderID)
                {
                    amountOfMatches++;
                    effectMatches[i] = true;
                }
            }

            int sortIndex = -1;
            DrawData[] sortedDraws = new DrawData[amountOfMatches];
            DrawData[] queuedDrawsClone = queuedDraws.ToArray();
            for (int i = 0; i < queuedDraws.Count; i++)
            {
                if (!effectMatches[i])
                    continue;

                sortIndex++;
                sortedDraws[sortIndex] = queuedDrawsClone[i];
                queuedDraws.RemoveAt(i);
                i--;
            }
            Array.Clear(queuedDrawsClone, 0, queuedDrawsClone.Length);
            Array.Clear(effectMatches, 0, effectMatches.Length);

            return sortedDraws;
        }


        public static void QueueShaderDraw(Texture2D texture, Vector2 position, Rectangle? rectangle, byte shaderID)
        {
            Rectangle rect;
            if (rectangle == null)
                rect = new Rectangle(0, 0, texture.Width, texture.Height);
            else
                rect = (Rectangle)rectangle;

            DrawData drawData = new DrawData();
            drawData.texture = texture;
            drawData.position = position;
            drawData.rect = rect;
            drawData.rotation = 0f;
            drawData.origin = Vector2.Zero;
            drawData.scale = 0f;
            drawData.spriteEffect = SpriteEffects.None;
            drawData.shaderID = shaderID;
            queuedDraws.Add(drawData);
        }

        public static void QueueShaderDraw(Texture2D texture, Vector2 position, Rectangle? rectangle, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffect, byte shaderID)
        {
            Rectangle rect;
            if (rectangle == null)
                rect = new Rectangle(0, 0, texture.Width, texture.Height);
            else
                rect = (Rectangle)rectangle;

            DrawData drawData = new DrawData();
            drawData.texture = texture;
            drawData.position = position;
            drawData.rect = rect;
            drawData.rotation = rotation;
            drawData.origin = origin;
            drawData.scale = scale;
            drawData.spriteEffect = spriteEffect;
            drawData.shaderID = shaderID;
            queuedDraws.Add(drawData);
        }

        public static void DrawScreenShader(RenderTarget2D renderTarget, Effect effect)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, null);
            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public static void DrawScreenShader(RenderTarget2D renderTarget, Effect effect, string parameterName, float parameterValue)
        {
            effect.Parameters[parameterName].SetValue(parameterValue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, null);
            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        public static void DrawScreenShader(RenderTarget2D renderTarget, Effect effect, string[] parameterNames, float[] parameterValues)
        {
            for (int i = 0; i < parameterNames.Length; i++)
            {
                effect.Parameters[parameterNames[i]].SetValue(parameterValues[i]);
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, null);
            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        private static void DrawShaderItems(DrawData[] drawData, Effect effect)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, GameData.CurrentGameDrawInfo.matrix);

            for (int i = 0; i < drawData.Length; i++)
            {
                DrawData data = drawData[i];
                spriteBatch.Draw(data.texture, data.position, data.rect, Color.White, data.rotation, data.origin, data.scale, data.spriteEffect, 0f);
            }

            spriteBatch.End();
        }

        public static void DrawShaderItemImmediately(Texture2D texture, Vector2 position, Rectangle? rectangle, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffect, byte shaderID)
        {
            spriteBatch.End();

            spriteBatch.Begin(GameData.CurrentGameDrawInfo, GetShaderFromEffectID(shaderID));

            Rectangle rect;
            if (rectangle == null)
                rect = new Rectangle(0, 0, texture.Width, texture.Height);
            else
                rect = (Rectangle)rectangle;

            spriteBatch.Draw(texture, position, rect, Color.White, rotation, origin, scale, spriteEffect, 0f);

            spriteBatch.End();

            spriteBatch.Begin(GameData.CurrentGameDrawInfo);
        }

        public static void DrawShaderItemImmediately(GameDrawInfo gameDrawInfo, Texture2D texture, Vector2 position, Rectangle? rectangle, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffect, byte shaderID)
        {
            spriteBatch.End();

            spriteBatch.Begin(gameDrawInfo, GetShaderFromEffectID(shaderID));

            Rectangle rect;
            if (rectangle == null)
                rect = new Rectangle(0, 0, texture.Width, texture.Height);
            else
                rect = (Rectangle)rectangle;

            spriteBatch.Draw(texture, position, rect, Color.White, rotation, origin, scale, spriteEffect, 0f);

            spriteBatch.End();

            spriteBatch.Begin(gameDrawInfo);
        }

        public static void DrawShaderItemImmediately(Texture2D texture, Vector2 position, Rectangle? rectangle, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffect, Effect effect)
        {
            spriteBatch.End();

            spriteBatch.Begin(GameData.CurrentGameDrawInfo, effect);

            Rectangle rect;
            if (rectangle == null)
                rect = new Rectangle(0, 0, texture.Width, texture.Height);
            else
                rect = (Rectangle)rectangle;

            spriteBatch.Draw(texture, position, rect, Color.White, rotation, origin, scale, spriteEffect, 0f);

            spriteBatch.End();

            spriteBatch.Begin(GameData.CurrentGameDrawInfo);
        }

        public static void DrawShaderItemImmediately(GameDrawInfo gameDrawInfo, Texture2D texture, Rectangle sourceRectangle, Rectangle? rectangle, float rotation, Vector2 origin, SpriteEffects spriteEffect, Effect effect)
        {
            spriteBatch.End();

            spriteBatch.Begin(gameDrawInfo, effect);

            Rectangle rect;
            if (rectangle == null)
                rect = new Rectangle(0, 0, texture.Width, texture.Height);
            else
                rect = (Rectangle)rectangle;

            spriteBatch.Draw(texture, sourceRectangle, rect, Color.White, rotation, origin, spriteEffect, 0f);

            spriteBatch.End();

            spriteBatch.Begin(gameDrawInfo);
        }

        public static void SetRenderTargetReadyForShaders(GraphicsDevice graphicsDevice, RenderTarget2D renderTarget)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.CornflowerBlue);
        }

        public static void ClearShaderTarget(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Black);
        }

        public static Effect GetShaderFromEffectID(byte shaderID)
        {
            return activeShaderEffects[shaderID];
        }
    }
}
