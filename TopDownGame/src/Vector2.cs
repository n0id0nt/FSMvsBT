using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using Box2D;

namespace TopDownGame
{
    class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2()
        {
            X = 0f;
            Y = 0f;
        }

        public Vector2(Vector2 vect)
        {
            X = vect.X;
            Y = vect.Y;
        }

        public float Length { get { return (float)Math.Sqrt(X * X + Y * Y); } }

        public float Angle { get { return (float)Math.Atan2(Y, X); } }

        public static double Distance(Vector2 lhs, Vector2 rhs)
        {
            double x = lhs.X - rhs.X;
            double y = lhs.Y - rhs.Y;
            return Math.Sqrt(x * x + y * y);
        }

        public void Rotate(double angle)
        {
            float x = (float)(Math.Cos(angle) * X - Math.Sin(angle) * Y);
            float y = (float)(Math.Sin(angle) * X + Math.Cos(angle) * Y);
            X = x;
            Y = y;
        }

        public Vector2 Rotated(double angle)
        {
            Vector2 temp = new Vector2();
            temp.X = (float)(Math.Cos(angle) * X - Math.Sin(angle) * Y);
            temp.Y = (float)(Math.Sin(angle) * X + Math.Cos(angle) * Y);
            return temp;
        }

        public Vector2 Normalized()
        {
            float length = Length;
            Vector2 temp = new Vector2();
            if (length != 0)
            {
                temp.X = X / length;
                temp.Y = Y / length;
            }
            return temp;
        }

        public Vector2 Truncate(float value)
        {
            float length = Length;
            if (value < length)
                return Normalized() * value;
            else
                return this;
        }

        public static float Dot(Vector2 lhs, Vector2 rhs)
        {
            float a = lhs.X * rhs.X;
            float b = lhs.Y * rhs.Y;
            return a + b;
        }

        public static float AngleBetween(Vector2 lhs, Vector2 rhs)
        {
            return (float)Math.Atan2(rhs.Y - lhs.Y, rhs.X - lhs.X);
        }

        public static Vector2 operator+(Vector2 lhs, Vector2 rhs)
        {
            Vector2 temp = new Vector2();
            temp.X = lhs.X + rhs.X;
            temp.Y = lhs.Y + rhs.Y;
            return temp;
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            Vector2 temp = new Vector2();
            temp.X = lhs.X - rhs.X;
            temp.Y = lhs.Y - rhs.Y;
            return temp;
        }

        public static Vector2 operator -(Vector2 lhs, float rhs)
        {
            Vector2 temp = new Vector2();
            temp.X = lhs.X - rhs;
            temp.Y = lhs.Y - rhs;
            return temp;
        }

        public static Vector2 operator*(Vector2 lhs, float rhs)
        {
            Vector2 temp = new Vector2();
            temp.X = lhs.X * rhs;
            temp.Y = lhs.Y * rhs;
            return temp;
        }

        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            Vector2 temp = new Vector2();
            temp.X = lhs.X / rhs;
            temp.Y = lhs.Y / rhs;
            return temp;
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator Vector2f(Vector2 vec)
        {
            return new Vector2f(vec.X, vec.Y);
        }

        public static implicit operator Vector2(Vector2i vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static implicit operator b2Vec2(Vector2 vec)
        {
            return new b2Vec2(vec.X, vec.Y);
        }

        public static implicit operator Vector2(b2Vec2 vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static implicit operator Vector2i(Vector2 vec)
        {
            return new Vector2i((int)vec.X, (int)vec.Y);
        }
    }
}
