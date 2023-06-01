using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SFML.Graphics;
using Box2D;

namespace TopDownGame
{
    class Player : CollisionGameObject
    {
        // Distance for the arrive force
        private const float arriveDist = 20f;

        // The health of the player
        private uint health;
        // Players max health
        private const uint maxHealth = 2;

        public Player(Vector2 pos, Vector2 size, string image) : base(pos, size)
        {
            health = maxHealth;

            CollisionShape = new CollisionCircle(size.Length / 4, this, b2BodyType.b2_dynamicBody);
            MaxSpeed = 4f;
            MaxForce = 1f;

            Sprite = new Image(image);
        }

        public override void Update()
        {
            // Get the mouse position
            Vector2 target = SFML.Window.Mouse.GetPosition(Window.window);

            // cacluate movement forces
            Acc = Arrive(target, arriveDist);
            Vel += Acc;
            Vel = Vel.Truncate(MaxSpeed);
            MoveAndSlide(Vel);
        }

        public override void Render()
        {
            RectangleShape rect = new RectangleShape(Size);
            rect.Texture = new Texture(Sprite);
            rect.Rotation = Vel.Angle * 180f / (float)Math.PI;
            rect.Origin = Size / 2;
            rect.Position = Pos;
            Window.Draw(rect);

            if (Game.Debug)
                CollisionShape.Draw();
        }

        /// <summary>
        /// Decreases the amount of health of the player 
        /// </summary>
        /// <param name="damageAmount">The amount of damage taken</param>
        public void Damage(uint damageAmount = 1)
        {
            // add delay so there is mkore impact to being hit
            Thread.Sleep(100);
            
            // decrease the amount of health
            health -= damageAmount;

            // reset the level when no damage is left
            if (health <= 0)
            {
                Game.ResetLevel();
            }
        }
    }
}
