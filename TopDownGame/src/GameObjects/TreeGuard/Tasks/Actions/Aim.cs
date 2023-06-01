using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Aim : Task
    {
        public Aim(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
            Started = true;
            baseObject.Vel = new Vector2();
            baseObject.textureRect.Left = (int)baseObject.Size.X * 3;
        }

        public override TaskStatus DoAction()
        {
            // find the player
            Player player = null;
            foreach (GameObject gameObject in Game.Scene.GameObjects)
                if (gameObject is Player)
                    player = (Player)gameObject;

            // face the player
            baseObject.facing = (player.Pos - baseObject.Pos).Normalized();

            baseObject.AddToActiveTasks(this);
            return TaskStatus.Running;
        }

        public override void End()
        {
            Started = false;
        }
    }
}
