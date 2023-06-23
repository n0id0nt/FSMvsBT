using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class CompleteAction : Task
    {
        Action action;
        public CompleteAction(TreeGuard baseObject, Action action) : base(baseObject)
        {
            this.action = action;
        }

        public override void Start()
        {
        }

        public override TaskStatus DoAction()
        {
            action.Invoke();

            return TaskStatus.Success;
        }

        public override void End()
        {
        }
    }
}