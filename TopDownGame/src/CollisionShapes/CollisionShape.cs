using System;
using System.Collections.Generic;
using System.Text;
using Box2D;


namespace TopDownGame
{
    abstract class CollisionShape
    {
        public const float BOX2D_SCALE = 100;

        public ICollidable BaseObject { get; }
        
        public b2Body Body { get; protected set; }

        public Vector2 Pos { get { return BaseObject.Pos; } }

        public CollisionShape(ICollidable baseObject)
        {
            BaseObject = baseObject;
        }

        public abstract bool IsColliding(CollisionShape other);

        public abstract bool IsColliding(Vector2 point);

        public static Vector2 Vector2ToBox2(Vector2 vect)
        {
            return vect / BOX2D_SCALE;
        }

        public static Vector2 Box2ToVector2(Vector2 vect)
        {
            return vect * BOX2D_SCALE;
        }

        public abstract void Draw();

        protected bool LineLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float uA = ((b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X)) / ((b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y));
            float uB = ((a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X)) / ((b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y));

            return (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1);
        }

        protected bool LinePoint(Vector2 l1, Vector2 l2, Vector2 p)
        {
            float d1 = (l1 - p).Length;
            float d2 = (l2 - p).Length;

            // get the length of the line
            float lineLen = (l1 - l2).Length;

            // since floats are so minutely accurate, add
            // a little buffer zone that will give collision
            float buffer = 0.1f;    // higher # = less accurate

            // if the two distances are equal to the line's
            // length, the point is on the line!
            // note we use the buffer here to give a range,
            // rather than one #
            return (d1 + d2 >= lineLen - buffer && d1 + d2 <= lineLen + buffer);
        }

        protected bool PointCircle(Vector2 point, Vector2 circlePos, float radius)
        {
            return Vector2.Distance(point, circlePos) <= radius;
        }
    }
}
