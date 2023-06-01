using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class IsPlayerWithinRange : Task
    {
        private float range;

        public IsPlayerWithinRange(TreeGuard baseObject, float range) : base(baseObject)
        {
            this.range = range;
        }

        public override void Start()
        {

        }

        public override TaskStatus DoAction()
        {
            Player player = null;
            foreach (GameObject gameObject in Game.Scene.GameObjects)
                if (gameObject is Player)
                    player = (Player)gameObject;

            if (Vector2.Distance(player.Pos, baseObject.Pos) <= range)
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }

        public override void End()
        {

        }
    }
}
