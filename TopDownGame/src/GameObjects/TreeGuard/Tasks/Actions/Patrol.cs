using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Patrol : Task
    {
        // distance before the path is incremented
        private const float pathDist = 25f;
        private const float pathToTargetDist = 20f;

        public Patrol(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
            Started = true;
            baseObject.textureRect.Left = (int)baseObject.Size.X * 0;

            // create path to target
            baseObject.pathToTarget = new Path(baseObject.Pos, baseObject.path.Target, (TileMap)Game.Scene.GameObjects.Find(t => t is TileMap));
        }

        public override TaskStatus DoAction()
        {
            // Update the paths
            if (Vector2.Distance(baseObject.Pos, baseObject.path.Target) < pathDist)
            {
                baseObject.path.IncrementPath();
                baseObject.pathToTarget = new Path(baseObject.Pos, baseObject.path.Target, (TileMap)Game.Scene.GameObjects.Find(t => t is TileMap));
            }
            else if (Vector2.Distance(baseObject.Pos, baseObject.pathToTarget.Target) < pathToTargetDist)
            {
                baseObject.pathToTarget.IncrementPath();
            }
            else if (!baseObject.pathToTarget.IsPathClear(baseObject.Pos, (TileMap)Game.Scene.GameObjects.Find(t => t is TileMap)))
            {
                baseObject.pathToTarget = new Path(baseObject.Pos, baseObject.path.Target, (TileMap)Game.Scene.GameObjects.Find(t => t is TileMap));
            }

            // find the other guards 
            List<CollisionGameObject> fsmGuards = new List<CollisionGameObject>();
            foreach (GameObject g in Game.Scene.GameObjects)
                if (g is CollisionGameObject && g != baseObject)
                    fsmGuards.Add((CollisionGameObject)g);

            Vector2 force = baseObject.Seek(baseObject.pathToTarget.Target);
            force += baseObject.CollisionAvoidance(fsmGuards, TreeGuard.sightDist) * 1.1f;
            force += baseObject.Seperation(fsmGuards.ConvertAll(g => (GameObject)g)) * 25;

            // set the acceleration to the force
            baseObject.Acc = force.Truncate(baseObject.MaxForce);

            baseObject.AddToActiveTasks(this);
            return TaskStatus.Running;
        }

        public override void End()
        {
            Started = false;
        }
    }
}
