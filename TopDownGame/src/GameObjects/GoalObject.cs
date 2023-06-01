using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace TopDownGame
{
    class GoalObject : GameObject, ICollidable
    {
        public CollisionShape CollisionShape { get; }

        public GoalObject(Vector2 pos, Vector2 size) : base(pos, size)
        {
            CollisionShape = new CollisionRect(size, size / 2, this);
        }

        public override void Update()
        {
            // if colliding with player
            foreach (GameObject gameObject in Game.Scene.GameObjects)
            {
                if (gameObject is Player)
                {
                    Player player = (Player)gameObject;
                    if (CollisionShape.IsColliding(player.CollisionShape))
                    {
                        // change to next level
                        Game.NextLevel();
                    }
                }
            }
        }

        public override void Render()
        {
            RectangleShape rect = new RectangleShape(Size);
            rect.FillColor = new Color(212, 175, 55);
            rect.Origin = Size / 2;
            rect.Position = Pos;
            Window.Draw(rect);

            if (Game.Debug)
                CollisionShape.Draw();
        }

        public bool IsColliding()
        {
            return false;
        }
    }
}
