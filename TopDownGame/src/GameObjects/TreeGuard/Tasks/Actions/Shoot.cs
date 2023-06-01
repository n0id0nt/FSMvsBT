using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Shoot : Task
    {
        public Shoot(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {

        }

        public override TaskStatus DoAction()
        {
            baseObject.ammo--;
            // create a line the length of the guns range
            CollisionLine line = new CollisionLine(new Vector2(), baseObject.facing.Normalized() * TreeGuard.gunRange, baseObject);

            // get the gameobject that the line collides with
            List<GameObject> collidingObjects = line.GetIntersectingObjects();
            foreach (GameObject gameObject in collidingObjects)
            {
                // check the player is the closestthing in the bullets path
                if (gameObject is Player)
                {
                    CollisionLine collisionLine = new CollisionLine(new Vector2(), baseObject.facing.Normalized() * (gameObject.Pos - baseObject.Pos).Length, baseObject);
                    if (collisionLine.GetIntersectingObjects().Count <= 1)
                    {
                        Player player = (Player)gameObject;
                        player.Damage();
                    }
                }
            }

            return TaskStatus.Success;
        }

        public override void End()
        {

        }
    }
}
