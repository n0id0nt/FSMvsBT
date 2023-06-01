using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Box2D;

namespace TopDownGame
{
    class CollisionCircle : CollisionShape
    {
        public float Radius { get; set; }

        public CollisionCircle(float radius, ICollidable gameObject) : base(gameObject)
        {
            Radius = radius;
        }

        public CollisionCircle(float radius, ICollidable gameObject, b2BodyType bodyType) : base(gameObject)
        {
            Radius = radius;

            b2BodyDef bodyDef = new b2BodyDef();
            bodyDef.type = bodyType;
            bodyDef.position = Vector2ToBox2(gameObject.Pos);

            Body = Game.Scene.World.CreateBody(bodyDef);

            b2CircleShape shape = new b2CircleShape();
            shape.m_radius = radius / BOX2D_SCALE;

            b2FixtureDef fixtureDef = new b2FixtureDef();
            fixtureDef.shape = shape;
            fixtureDef.density = 1f;
            fixtureDef.friction = 0.3f;

            Body.CreateFixture(fixtureDef);
        }

        public override bool IsColliding(CollisionShape other)
        {
            if (other is CollisionRect)
            {
                CollisionRect collisionRect = (CollisionRect)other;
                float DeltaX = Pos.X - Math.Max(collisionRect.Pos.X - collisionRect.Offset.X, Math.Min(Pos.X, collisionRect.Pos.X - collisionRect.Offset.X + collisionRect.Size.X));
                float DeltaY = Pos.Y - Math.Max(collisionRect.Pos.Y - collisionRect.Offset.Y, Math.Min(Pos.Y, collisionRect.Pos.Y - collisionRect.Offset.Y + collisionRect.Size.Y));
                return (DeltaX * DeltaX + DeltaY * DeltaY) < (Radius * Radius);
            }
            else if (other is CollisionCircle)
            {
                CollisionCircle collisionCircle = (CollisionCircle)other;
                return Vector2.Distance(collisionCircle.Pos, Pos) <= Radius + collisionCircle.Radius;   
            }
            return false;
        }

        public override bool IsColliding(Vector2 point)
        {
            return Vector2.Distance(point, Pos) <= Radius;
        }

        public override void Draw()
        {
            CircleShape circle = new CircleShape(Radius);
            circle.Position = Pos - Radius;
            circle.FillColor = new Color(245, 15, 5, 100);
            Window.Draw(circle);
        }
    }
}
