using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Box2D;

namespace TopDownGame
{
    class FSMGuard : CollisionGameObject
    {
        // the path the guard searches
        public Path path;
        // the path to the next target
        public Path pathToTarget;

        // the state manager controls the guards states
        public StateManager stateManager;

        // the area that the gurad can see the player in
        public CollisionShape fieldOfView;
        // the angle that the guard see
        public const float viewAngle = 1f;

        // the angle the guard is facing
        public Vector2 facing;
        // the distance the player can see
        public const float sightDist = 200;
        
        // the amount of ammo the guard has
        public int ammo;
        // the guards maximum amount of ammo
        public const int maxAmmo = 3;

        // the area that on the sprite to display
        public IntRect textureRect;

        // the range to the guards gun
        public const int gunRange = 170;

        public FSMGuard(Vector2 pos, Vector2 size, Path path, string image) : base(pos, size)
        {
            ammo = maxAmmo;
            this.path = path;

            fieldOfView = new CollisionCircle(sightDist, this);

            // initialize the guards states
            stateManager = new StateManager(new Dictionary<string, IState>
            {
                ["Patrol"] = new PatrolState(this),
                ["Look"] = new LookState(this),
                ["Chase"] = new ChaseState(this),
                ["Reload"] = new ReloadState(this),
                ["Shoot"] = new ShootState(this),
                ["Aim"] = new AimState(this)
            });
            // set the initial state to patrol
            stateManager.ChangeState("Patrol");
            CollisionShape = new CollisionCircle(size.Length / 4, this, b2BodyType.b2_dynamicBody);

            MaxSpeed = 3f;
            MaxForce = 0.5f;

            facing = new Vector2(1, 0);

            Sprite = new Image(image);
            textureRect = new IntRect(new Vector2(), size);
        }

        ~FSMGuard()
        {
            stateManager.CurState.Exit();
        }

        /// <summary>
        /// Chacks if the player is within sencing range of the guard
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerInRange()
        {
            foreach (GameObject gameObject in Game.Scene.GameObjects)
            {
                if (gameObject is Player)
                {
                    if (Vector2.Distance(gameObject.Pos, Pos) < sightDist)
                    {
                        CollisionLine collisionLine = new CollisionLine(new Vector2(), gameObject.Pos - Pos, this);

                        List<GameObject> collidingObjects = collisionLine.GetIntersectingObjects();

                        return collidingObjects.Count <= 1;
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the player is within the guards sight
        /// </summary>
        /// <returns></returns>
        public bool IsPlayerInSight()
        {
            foreach (GameObject gameObject in Game.Scene.GameObjects)
            {
                if (gameObject is Player)
                {
                    if (Vector2.Distance(gameObject.Pos, Pos) < sightDist && (gameObject.Pos - Pos).Angle < facing.Angle + viewAngle && (gameObject.Pos - Pos).Angle > facing.Angle - viewAngle)
                    {
                        CollisionLine collisionLine = new CollisionLine(new Vector2(), gameObject.Pos - Pos, this);

                        List<GameObject> collidingObjects = collisionLine.GetIntersectingObjects();

                        return collidingObjects.Count <= 1;
                    }
                    return false;
                }
            }
            return false;
        }

        public override void Update()
        {
            Acc = new Vector2();

            stateManager.Update();

            Vel += Acc;
            Vel = Vel.Truncate(MaxSpeed);

            // face in the direction of the velocity
            if (Vel.Length != 0)
                facing = Vel.Normalized();

            MoveAndSlide(Vel);
        }

        public override void Render()
        {
            RectangleShape rect = new RectangleShape(Size);
            rect.Texture = new Texture(Sprite);
            rect.TextureRect = textureRect;
            rect.Rotation = facing.Angle * 180f / (float)Math.PI;
            rect.Origin = Size / 2;
            rect.Position = Pos;
            Window.Draw(rect);

            if (Game.Debug)
            {
                path.Render();
                CollisionShape.Draw();
                fieldOfView.Draw();

                VertexArray line = new VertexArray(PrimitiveType.LineStrip, 5);

                Vertex vertex0 = new Vertex();
                vertex0.Position = Pos + facing.Rotated(-viewAngle) * sightDist;
                vertex0.Color = Color.Black;
                line[0] = vertex0;

                Vertex vertex1 = new Vertex();
                vertex1.Position = Pos;
                vertex1.Color = Color.Black;
                line[1] = vertex1;

                Vertex vertex2 = new Vertex();
                vertex2.Position = Pos + facing.Rotated(viewAngle) * sightDist;
                vertex2.Color = Color.Black;
                line[2] = vertex2;

                Vertex vertex3 = new Vertex();
                vertex3.Position = Pos;
                vertex3.Color = Color.Black;
                line[3] = vertex3;

                Vertex vertex4 = new Vertex();
                vertex4.Position = Pos + facing * sightDist;
                vertex4.Color = Color.Black;
                line[4] = vertex4;

                Window.Draw(line);
            }
            stateManager.CurState.Draw();
        }

        /// <summary>
        /// Shoots the guards gun
        /// </summary>
        public void Shoot()
        {
            // create a line the length of the guns range
            CollisionLine line = new CollisionLine(new Vector2(), facing.Normalized() * gunRange, this);

            // get the gameobject that the line collides with
            List<GameObject> collidingObjects = line.GetIntersectingObjects();
            foreach (GameObject gameObject in collidingObjects)
            {
                // check the player is the closestthing in the bullets path
                if (gameObject is Player)
                {
                    CollisionLine collisionLine = new CollisionLine(new Vector2(), facing.Normalized() * (gameObject.Pos - Pos).Length, this);
                    if (collisionLine.GetIntersectingObjects().Count <= 1)
                    {
                        Player player = (Player)gameObject;
                        player.Damage();
                    }
                }
            }
        }
    }
}
