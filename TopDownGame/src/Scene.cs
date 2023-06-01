using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Box2D;

namespace TopDownGame
{
    class Scene
    {
        // The pyhsics world
        public b2World World { get; }

        // The list of games objects in the scene
        public List<GameObject> GameObjects { get; }

        public Scene()
        {
            GameObjects = new List<GameObject>();
            World = new b2World(new Vector2());
        }

        /// <summary>
        /// Updates each game object and run pysics simulation
        /// </summary>
        public void Update()
        {
            // Update each game object
            foreach (GameObject gameObject in GameObjects)
                gameObject.Update();
            // Run physics simulation for the frame
            World.Step(1f / 60f, 1, 1);
            // Update the posistion of each collidable object to match the collision objects
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject is CollisionGameObject)
                {
                    CollisionGameObject collisionGameObject = (CollisionGameObject)gameObject;
                    collisionGameObject.Pos = CollisionShape.Box2ToVector2(collisionGameObject.CollisionShape.Body.GetPosition());
                }
            }
            // Clear the forces in the physics engine
            World.ClearForces();
        }

        /// <summary>
        /// Runs the render method for each game object
        /// </summary>
        public void Render()
        {
            foreach (GameObject gameObject in GameObjects)
                gameObject.Render();
        }
    }
}
