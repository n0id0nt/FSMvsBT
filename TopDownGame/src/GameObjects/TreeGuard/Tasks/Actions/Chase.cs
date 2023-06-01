using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Chase : Task
    {
        Player target;

        public Chase(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
            Started = true;

            baseObject.textureRect.Left = (int)baseObject.Size.X * 1;

            foreach (GameObject gameObject in Game.Scene.GameObjects)
                if (gameObject is Player)
                    target = (Player)gameObject;
        }

        public override TaskStatus DoAction()
        {
            baseObject.Acc = baseObject.Seek(target.Pos).Normalized() * baseObject.MaxForce;
            baseObject.AddToActiveTasks(this);
            return TaskStatus.Running;
        }

        public override void End()
        {
            Started = false;
        }
    }
}
