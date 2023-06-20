using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class IsChasingPlayer : Task
    {
        public IsChasingPlayer(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {

        }

        public override TaskStatus DoAction()
        {
            if (baseObject.chasingPlayer)
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }

        public override void End()
        {

        }
    }
}
