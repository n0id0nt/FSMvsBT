using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Box2D;

namespace TopDownGame
{
    class CollisionRect : CollisionShape
    {
        public Vector2 Size { get; }
        public Vector2 Offset { get; }

        public CollisionRect(Vector2 size, Vector2 offset, ICollidable gameObject) : base(gameObject)
        {
            Size = size;
            Offset = offset;
        }

        public CollisionRect(Vector2 size, Vector2 offset, ICollidable gameObject, b2BodyType bodyType) : base(gameObject)
        {
            Size = size;
            Offset = offset;

            b2BodyDef bodyDef = new b2BodyDef();
            bodyDef.type = bodyType;
            bodyDef.position = Vector2ToBox2(gameObject.Pos - offset + (size/2));

            Body = Game.Scene.World.CreateBody(bodyDef);

            b2PolygonShape shape = new b2PolygonShape();
            shape.SetAsBox(size.X/(2 * BOX2D_SCALE), size.Y/(2 * BOX2D_SCALE));

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
                return (Pos.X - Offset.X < collisionRect.Pos.X - collisionRect.Offset.X + collisionRect.Size.X &&
                    Pos.X - Offset.X + Size.X > collisionRect.Pos.X - collisionRect.Offset.X &&
                    Pos.Y - Offset.Y < collisionRect.Pos.Y - collisionRect.Offset.Y + collisionRect.Size.Y &&
                    Pos.Y - Offset.Y + Size.Y > collisionRect.Pos.Y - collisionRect.Offset.Y);
            }
            return other.IsColliding(this);
        }

        public override bool IsColliding(Vector2 point)
        {
            return (Pos.X - Offset.X < point.X &&
                Pos.X - Offset.X + Size.X > point.X &&
                Pos.Y - Offset.Y < point.Y &&
                Pos.Y - Offset.Y + Size.Y > point.Y);
        }

        public override void Draw()
        {
            RectangleShape rect = new RectangleShape(Size);
            rect.Position = Pos - Offset;
            rect.FillColor = new Color(245, 15, 5, 100);
            Window.Draw(rect);
        }
    }
}
