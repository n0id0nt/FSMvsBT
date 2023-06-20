using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class SetChasingPlayer : Task
    {
        private bool value;
        public SetChasingPlayer(TreeGuard baseObject, bool value) : base(baseObject)
        {
            this.value = value;
        }

        public override void Start()
        {

        }

        public override TaskStatus DoAction()
        {
            baseObject.chasingPlayer = value;
            return TaskStatus.Success;
        }

        public override void End()
        {

        }
    }
}
