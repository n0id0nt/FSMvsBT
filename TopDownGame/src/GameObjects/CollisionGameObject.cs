using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TopDownGame 
{
    abstract class CollisionGameObject : GameObject, ICollidable
    {
        public float MaxSpeed { get; set; }

        public float MaxForce { get; set; }

        public Vector2 Acc { get; set; }

        public Vector2 Vel { get; set; }

        public CollisionShape CollisionShape { get; protected set; }

        public CollisionGameObject(Vector2 pos, Vector2 size) : base(pos, size)
        {
            Acc = new Vector2();
            Vel = new Vector2();
        }

        public bool IsColliding()
        {
            foreach (GameObject gameObject in Game.Scene.GameObjects)
            {
                if (gameObject is ICollidable && gameObject != this)
                {
                    ICollidable collidable = (ICollidable)gameObject;
                    if (collidable.CollisionShape.IsColliding(CollisionShape))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Moves the collision to handel collision
        /// </summary>
        /// <param name="vel"></param>
        public void MoveAndSlide(Vector2 vel)
        {
            CollisionShape.Body.SetLinearVelocity(CollisionShape.Vector2ToBox2(vel)*60);
        }

        /// <summary>
        /// Seek steering force
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Vector2 Seek(Vector2 target)
        {
            Vector2 desired = target - Pos;
            desired = desired.Normalized() * MaxSpeed;

            Vector2 steer = desired - Vel;
            steer = steer.Truncate(MaxForce);
            return steer;
        }

        /// <summary>
        /// seperation steering force
        /// </summary>
        /// <param name="group">group of objects to avoid</param>
        /// <param name="dist">distance from object to add to force</param>
        /// <returns></returns>
        public Vector2 SeperationWithDistance(List<GameObject> group, float dist)
        {
            Vector2 steeringForce = new Vector2();
            foreach (GameObject bot in group)
            {
                Vector2 toBot = Pos - bot.Pos;
                if (toBot.Length < dist)
                    steeringForce += toBot.Normalized() / toBot.Length;
            }

            return steeringForce.Truncate(MaxForce);
        }

        /// <summary>
        /// seperation steering force
        /// </summary>
        /// <param name="group">group of objects to avoid</param>
        /// <returns></returns>
        public Vector2 Seperation(List<GameObject> group)
        {
            Vector2 steeringForce = new Vector2();
            foreach (GameObject bot in group)
            {
                Vector2 toBot = Pos - bot.Pos;
                steeringForce += toBot.Normalized() / toBot.Length;
            }

            return steeringForce.Truncate(MaxForce);
        }

        /// <summary>
        /// Collision avoidance froce
        /// </summary>
        /// <param name="group">list of objects to avoid</param>
        /// <param name="viewDist">sight to see objects</param>
        /// <returns></returns>
        public Vector2 CollisionAvoidance(List<CollisionGameObject> group, float viewDist)
        {
            // sort the list of objects by the distance to self
            group = group.OrderBy(g => Vector2.Distance(Pos, g.Pos)).ToList();

            // test if going to collide with object
            foreach (CollisionGameObject gameObject in group)
            {
                float dist = (float)Vector2.Distance(Pos, gameObject.Pos);
                Vector2 ahead = Pos + Vel.Normalized() * dist;

                TileMap tileMap = (TileMap)Game.Scene.GameObjects.First(g => g is TileMap);
                CollisionLine line = new CollisionLine(Pos, ahead, tileMap);
                if (dist < viewDist && gameObject.CollisionShape.IsColliding(ahead) && !line.IsColliding(tileMap.CollisionShape))
                {
                    Vector2 avoidanceForce = ahead - gameObject.Pos - Align(gameObject);
                    return avoidanceForce.Normalized() * MaxForce;
                }
            }

            return new Vector2();
        }

        /// <summary>
        /// arrive steering force
        /// </summary>
        /// <param name="target"></param>
        /// <param name="arriveDist"></param>
        /// <returns></returns>
        public Vector2 Arrive(Vector2 target, float arriveDist)
        {
            Vector2 desired = target - Pos;
            float d = desired.Length;
            desired = desired.Normalized();

            if (d < arriveDist)
            {
                float m = d * MaxSpeed / arriveDist;
                desired *= m;
            }
            else
                desired *= MaxSpeed;

            Vector2 steer = desired - Vel;
            steer = steer.Truncate(MaxForce);
            return steer;
        }

        /// <summary>
        /// alignment steering force
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public Vector2 Align(List<CollisionGameObject> group)
        {
            Vector2 sum = new Vector2();
            foreach (CollisionGameObject other in group)
            {
                sum += other.Vel;
            }
            sum /= group.Count;

            sum = sum.Normalized() * MaxSpeed;

            Vector2 steer = sum + Vel;
            return steer.Truncate(MaxForce);
        }

        /// <summary>
        /// alignment steering force for singular object
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector2 Align(CollisionGameObject other)
        {
            Vector2 sum = other.Vel.Normalized() * MaxSpeed;

            Vector2 steer = sum + Vel;
            return steer.Truncate(MaxForce);
        }
    }
}
