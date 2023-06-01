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

        public Queue<Task> activeTasks;

        public TreeGuard(Vector2 pos, Vector2 size, Path path, string image) : base(pos, size)
        {
            activeTasks = new Queue<Task>();
            {
                // create root
                Sequence rootTask = new Sequence(this);

                Selector patrolSelector = new Selector(this);
                rootTask.Add(patrolSelector);
                    // add player sensor
                    patrolSelector.Add(new IsPlayerSensed(this));
                    // add patrol task
                    patrolSelector.Add(new Patrol(this));

                Selector lookSelector = new Selector(this);
                rootTask.Add(lookSelector);
                    // add player in sight check
                    lookSelector.Add(new IsPlayerInSight(this));
                    // add look task
                    lookSelector.Add(new Look(this));

                Selector chaseSelector = new Selector(this);
                rootTask.Add(chaseSelector);
                    // add player range check
                    chaseSelector.Add(new IsPlayerWithinRange(this, 100f));
                    // add chase task
                    chaseSelector.Add(new Chase(this));

                Sequence shootSequence = new Sequence(this);
                rootTask.Add(shootSequence);
                    // add reload selector
                    Selector reloadSelector = new Selector(this);
                    shootSequence.Add(reloadSelector);
                        // add bullets check
                        reloadSelector.Add(new HasBullets(this));
                        // add reload sequence
                        Sequence reloadSequence = new Sequence(this);
                        reloadSelector.Add(reloadSequence);
                            reloadSequence.Add(new ChangeSpriteIndex(4, this));
                            reloadSequence.Add(new SetVelocity(this, new Vector2()));
                            reloadSequence.Add(new Wait(20, this));
                            reloadSequence.Add(new Reload(this));
                    // add aim task
                    shootSequence.Add(new TimerDecorator(this, new Aim(this), 35));
                    // add shoot task
                    shootSequence.Add(new ChangeSpriteIndex(3, this));
                    shootSequence.Add(new SetVelocity(this, new Vector2()));
                    shootSequence.Add(new Wait(20, this));
                    shootSequence.Add(new Shoot(this));

                root = new RootTask(rootTask);
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
            Acc = new Vector2();

            if (facing is null)
                Console.WriteLine("broke1");

            root.Update();

            Vel += Acc;
            Vel = Vel.Truncate(MaxSpeed);

            // face in the direction of the velocity
            if (Vel.Length != 0)
                facing = Vel.Normalized();
            if (facing is null)
                Console.WriteLine("broke2");

            MoveAndSlide(Vel);

            // finish the active tasks
            while (activeTasks.Count > 1)
            {
                Task task = activeTasks.Dequeue();
                task.End();
            }
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

        public void AddToActiveTasks(Task task)
        {
            if (!activeTasks.Contains(task))
                activeTasks.Enqueue(task);
        }
    }
}
