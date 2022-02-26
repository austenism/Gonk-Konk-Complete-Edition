using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gonk_Konk_Complete_Edition.Collisions
{
    public struct BoundingRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public BoundingRectangle(float x, float y, float height, float width)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public BoundingRectangle(Vector2 position, float height, float width)
        {
            X = position.X;
            Y = position.Y;
            Height = height;
            Width = width;
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            {
                return !(this.Right < other.Left || this.Left > other.Right
                      || this.Top > other.Bottom || this.Bottom < other.Top);
            }
        }

    }
}
