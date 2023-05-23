using Microsoft.Xna.Framework;
using System;

namespace AnotherLib.Utilities
{
    public static class Vector2Utils
    {
        public static float GetRotation(this Vector2 vector) => (float)Math.Atan2(vector.Y, vector.X);

        public static Vector2 CreateAngleVector(int degrees) => new Vector2((float)Math.Cos(MathHelper.ToRadians(degrees)), (float)Math.Sin(MathHelper.ToRadians(degrees)));
        public static Vector2 CreateAngleVector(float radians) => new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
    }
}
