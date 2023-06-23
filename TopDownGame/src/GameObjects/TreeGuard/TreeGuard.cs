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
        public const int gunRange = 90;
        public const int bufferRange = 10;

        public bool chasingPlayer = false;

        public bool looking = false;
        public bool shooting = false;

        private List<Task> activeTask = new List<Task>();
        public List<Task> ActiveTask => activeTask;

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
                        // check if player is senced
                        IsPlayerSensedSequence.Add(new IsPlayerSensed(this));
                        // If player is senced we can start looking for the player
                        IsPlayerSensedSequence.Add(new CompleteAction(this, () => looking = true));

                        // run sequence to see if player can be seen
                        Sequence IsPlayerInSightSequence = new Sequence(this);
                        IsPlayerSensedSequence.Add(IsPlayerInSightSequence);
                        {
                            // check if player can bee seen
                            IsPlayerInSightSequence.Add(new IsPlayerInSight(this));
                            // check if self can shoot player before chasing the player
                            Selector ShootPlayerSelector = new Selector(this);
                            IsPlayerInSightSequence.Add(ShootPlayerSelector);
                            {
                                // if player can shoot run shooting sequence
                                Sequence shootPlayerSequence = new Sequence(this);
                                ShootPlayerSelector.Add(shootPlayerSequence);
                                {
                                    // check if player is close enough to shoot
                                    shootPlayerSequence.Add(new IsPlayerWithinRange(this, gunRange));
                                    shootPlayerSequence.Add(new CompleteAction(this, () => shooting = true));
                                }
                                // if player is in the buffer range still shoot 
                                Sequence stillShootPlayerSequence = new Sequence(this);
                                ShootPlayerSelector.Add(stillShootPlayerSequence);
                                {
                                    // check if player is close enough to shoot
                                    stillShootPlayerSequence.Add(new CheckFunction(this, () => shooting));
                                    stillShootPlayerSequence.Add(new IsPlayerWithinRange(this, gunRange + bufferRange));
                                }

                                // chase pleyer to get within shooting range
                                ShootPlayerSelector.Add(new NotDecorator(this,new CompleteAction(this, () => shooting = false)));
                                ShootPlayerSelector.Add(new Chase(this));
                            }
                        }
                    }

                    Selector lookSelector = new Selector(this);
                    rootSequence.Add(lookSelector);
                    {
                        Sequence isShootingSequence = new Sequence(this);
                        lookSelector.Add(isShootingSequence);
                        {
                            isShootingSequence.Add(new CheckFunction(this, () => shooting));
                            // run shooting sequence
                            Sequence shootSequence = new Sequence(this, true);
                            isShootingSequence.Add(shootSequence);
                            {
                                shootSequence.Add(new ChangeSpriteIndex(this, 3));
                                shootSequence.Add(new TimerDecorator(this, new Aim(this), 30));
                                shootSequence.Add(new Wait(this, 10));
                                shootSequence.Add(new ChangeSpriteIndex(this, 4));
                                shootSequence.Add(new Shoot(this));
                                shootSequence.Add(new Wait(this, 20));
                                shootSequence.Add(new CompleteAction(this, () => shooting = false));
                            }
                        }

                        Sequence lookSequence = new Sequence(this);
                        lookSelector.Add(lookSequence);
                        {
                            lookSequence.Add(new CheckFunction(this, () => looking));
                            lookSequence.Add(new Look(this));
                        }
                        lookSelector.Add(new Patrol(this));

                    }
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
            activeTask.Clear();
            activeTask.Add(task);
            return TaskStatus.Running;
        }

        public void AddToAciveTasks(Task task)
        {
            activeTask.Add(task);
        }
    }
}
