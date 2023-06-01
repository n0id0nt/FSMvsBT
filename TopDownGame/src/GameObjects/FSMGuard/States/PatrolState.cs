using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.Graphics;

namespace TopDownGame
{
    class PatrolState : IState
    {
        // reference to the base object
        private FSMGuard baseObject;

        // distance before the path is incremented
        private const float pathDist = 25f;
        private const float pathToTargetDist = 20f;

        public PatrolState(FSMGuard fSMGuard)
        {
            baseObject = fSMGuard;
        }

        public void Enter()
        {
            // create path to target
            baseObject.pathToTarget = new Path(baseObject.Pos, baseObject.path.Target, (TileMap)Game.Scene.GameObjects.Find(t => t is TileMap));
            
            // change the image
            baseObject.textureRect.Left = 0;
        }

        public void Update()
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
            force += baseObject.CollisionAvoidance(fsmGuards, FSMGuard.sightDist) * 1.1f;
            force += baseObject.Seperation(fsmGuards.ConvertAll(g=> (GameObject)g)) * 25;

            // set the acceleration to the force
            baseObject.Acc = force.Truncate(baseObject.MaxForce);

            if (baseObject.IsPlayerInSight())
                baseObject.stateManager.ChangeState("Chase");

            else if (baseObject.IsPlayerInRange())
                baseObject.stateManager.ChangeState("Look");
        }

        public void Draw()
        {
            if (Game.Debug)
            {
                // draw the path to the target
                VertexArray line = new VertexArray(PrimitiveType.LineStrip, (uint)baseObject.pathToTarget.Points.Length);

                for (uint i = 0; i < baseObject.pathToTarget.Points.Length; i++)
                {
                    Vertex vertex = new Vertex();
                    vertex.Position = baseObject.pathToTarget.Points[i];
                    vertex.Color = Color.Black;
                    line[i] = vertex;
                }

                Window.Draw(line);
            }
        }

        public void Exit()
        {
            // nothing
        }
    }
}
