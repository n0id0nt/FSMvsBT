using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class IsPlayerSensed : Task
    {
        public IsPlayerSensed(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
            
        }

        public override TaskStatus DoAction()
        {
            foreach (GameObject gameObject in Game.Scene.GameObjects)
            {
                if (gameObject is Player)
                {
                    if (Vector2.Distance(gameObject.Pos, baseObject.Pos) < TreeGuard.sightDist)
                    {
                        CollisionLine collisionLine = new CollisionLine(new Vector2(), gameObject.Pos - baseObject.Pos, baseObject);

                        List<GameObject> collidingObjects = collisionLine.GetIntersectingObjects();

                        if (collidingObjects.Count <= 1)
                            return TaskStatus.Success;
                    }
                }
            }
            return TaskStatus.Failure;
        }

        public override void End()
        {
            
        }
    }
}
