using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class SetVelocity : Task
    {
        private Vector2 vel;
        public SetVelocity(TreeGuard baseObject, Vector2 velocity) : base(baseObject)
        {
            vel = velocity;
        }

        public override void Start()
        {
            
        }

        public override TaskStatus DoAction()
        {
            baseObject.Vel = vel;
            return TaskStatus.Success;
        }

        public override void End()
        {

        }
    }
}
