using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class CollisionLine : CollisionShape
    {
        private Vector2 offset1;
        private Vector2 offset2;

        public CollisionLine(Vector2 offset1, Vector2 offset2, ICollidable baseObject) : base(baseObject)
        {
            this.offset1 = offset1;
            this.offset2 = offset2;
        }

        public override bool IsColliding(CollisionShape other)
        {
            if (other is CollisionRect)
            {
                CollisionRect collisionRect = (CollisionRect)other;

                bool left = LineLine(offset1 + Pos, offset2 + Pos, collisionRect.Pos - collisionRect.Offset, new Vector2(collisionRect.Pos.X - collisionRect.Offset.X, collisionRect.Pos.Y - collisionRect.Offset.Y + collisionRect.Size.Y));
                bool right = LineLine(offset1 + Pos, offset2 + Pos, new Vector2(collisionRect.Pos.X - collisionRect.Offset.X + collisionRect.Size.X, collisionRect.Pos.Y - collisionRect.Offset.Y), collisionRect.Pos - collisionRect.Offset + collisionRect.Size);
                bool top = LineLine(offset1 + Pos, offset2 + Pos, collisionRect.Pos - collisionRect.Offset, new Vector2(collisionRect.Pos.X - collisionRect.Offset.X + collisionRect.Size.X, collisionRect.Pos.Y - collisionRect.Offset.Y));
                bool bottom = LineLine(offset1 + Pos, offset2 + Pos, new Vector2(collisionRect.Pos.X - collisionRect.Offset.X, collisionRect.Pos.Y - collisionRect.Offset.Y + collisionRect.Size.Y), new Vector2(collisionRect.Pos.X - collisionRect.Offset.X + collisionRect.Size.X, collisionRect.Pos.Y - collisionRect.Offset.Y));

                return left || right || top || bottom;
            }
            else if (other is CollisionCircle)
            {
                CollisionCircle collisionCircle = (CollisionCircle)other;

                bool inside1 = PointCircle(offset1 + Pos, collisionCircle.Pos, collisionCircle.Radius);
                bool inside2 = PointCircle(offset2 + Pos, collisionCircle.Pos, collisionCircle.Radius);
                if (inside1 || inside2) return true;

                float x1 = offset1.X + Pos.X;
                float x2 = offset2.X + Pos.X;
                float y1 = offset1.Y + Pos.Y;
                float y2 = offset2.Y + Pos.Y;

                float distX = x1 - x2;
                float distY = y1 - y2;
                float len = (float)Math.Sqrt((distX * distX) + (distY * distY));

                float dot = (((collisionCircle.Pos.X - x1) * (x2 - x1)) + ((collisionCircle.Pos.Y - y1) * (y2 - y1))) / (len * len);

                float closestX = x1 + (dot * (x2 - x1));
                float closestY = y1 + (dot * (y2 - y1));

                bool onSegment = LinePoint(offset1 + Pos, offset2 + Pos, new Vector2(closestX, closestY));
                if (!onSegment) return false;

                // get distance to closest point
                float distance = (new Vector2(closestX, closestY) - collisionCircle.Pos).Length;

                return (distance <= collisionCircle.Radius);
            }
            else if (other is CollisionTileMap)
            {
                return other.IsColliding(this);
            }
            return false;
        }

        public override bool IsColliding(Vector2 point)
        {
            return LinePoint(offset1 + Pos, offset2 + Pos, point);
        }

        public List<GameObject> GetIntersectingObjects()
        {
            List<GameObject> intersectingObjects = new List<GameObject>();

            foreach (GameObject gameObject in Game.Scene.GameObjects)
            {
                if (gameObject is ICollidable && gameObject != BaseObject)
                {
                    ICollidable collidable = (ICollidable)gameObject;
                    if (IsColliding(collidable.CollisionShape))
                    {
                        intersectingObjects.Add(gameObject);
                    }   
                }
            }

            return intersectingObjects;
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }
    }
}
