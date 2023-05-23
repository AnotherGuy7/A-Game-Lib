using Microsoft.Xna.Framework.Graphics;

namespace AnotherLib.Drawing
{
    public struct ShaderEffect
    {
        public byte ID;
        public Effect Effect;

        public ShaderEffect(byte id, Effect effect)
        {
            ID = id;
            Effect = effect;
        }
    }
}
