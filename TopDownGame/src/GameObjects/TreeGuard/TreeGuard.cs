using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using Box2D;

namespace TopDownGame
{
    class TreeGuard : CollisionGameObject
    {
        // the path the guard searches
        public Path path;
        // the path to the next target
        public Path pathToTarget;

        private RootTask root;

        public IntRect textureRect;

        // the area that the gurad can see the player in
        public CollisionShape fieldOfView;
        // the angle that the guard see
        public const float viewAngle = 1f;
        // the angle the guard is facing
        public Vector2 facing;

        public const float sightDist = 200;

        // the amount of ammo the guard has
        public int ammo;
        // the guards maximum amount of ammo
        public const int maxAmmo = 3;

        // the range to the guards gun
        public const int gunRange = 170;

        public bool chasingPlayer = false;

        public bool looking = false;

        private Task activeTask;
        public Task ActiveTask => activeTask;

        public TreeGuard(Vector2 pos, Vector2 size, Path path, string image) : base(pos, size)
        {
            //activeTasks = new Queue<Task>();
            {
                // create root
                Sequence rootTask = new Sequence(this);
                root = new RootTask(rootTask);

                All rootSequence = new All(this);
                rootTask.Add(rootSequence);
                {
                    Sequence IsPlayerSensedSequence = new Sequence(this);
                    rootSequence.Add(IsPlayerSensedSequence);
                    {
                        IsPlayerSensedSequence.Add(new IsPlayerSensed(this));
                        Selector IsPlayerInSightSelector = new Selector(this);
                        IsPlayerSensedSequence.Add(IsPlayerInSightSelector);
                        {
                            Sequence IsPlayerInSightSequence = new Sequence(this);
                            IsPlayerInSightSelector.Add(IsPlayerInSightSequence);
                            {
                                IsPlayerInSightSequence.Add(new IsPlayerInSight(this));
                                IsPlayerInSightSequence.Add(new Chase(this));
                            }

                            IsPlayerInSightSelector.Add(new CompleteAction(this, () => looking = true));
                        }
                    }
                    rootSequence.Add(new Patrol(this));
                }

            }

            this.path = path;

            Sprite = new Image(image);
            textureRect = new IntRect(new Vector2(), size);

            CollisionShape = new CollisionCircle(size.Length / 4, this, b2BodyType.b2_dynamicBody);
            facing = new Vector2(1, 0);

            fieldOfView = new CollisionCircle(sightDist, this);

            MaxSpeed = 3f;
            MaxForce = 0.5f;
            ammo = maxAmmo;
        }

        public override void Update()
        {
            // reset the velocity
            Acc = new Vector2();

            if (facing is null)
                Console.WriteLine("broke1");

            root.Update();

            // apply physics to the object
            Vel += Acc;
            Vel = Vel.Truncate(MaxSpeed);

            // face in the direction of the velocity
            if (Vel.Length != 0)
                facing = Vel.Normalized();
            if (facing is null)
                Console.WriteLine("broke2");

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
        }

        public TaskStatus TaskRunning(Task task)
        {
            activeTask = task;
            return TaskStatus.Running;
        }
    }
}
