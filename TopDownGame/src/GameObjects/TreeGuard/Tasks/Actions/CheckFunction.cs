using System;
using System.Collections.Generic;
using System.Text;


namespace TopDownGame
{
    class CheckFunction : Task
    {
        Func<bool> func;
        public CheckFunction(TreeGuard baseObject, Func<bool> func) : base(baseObject)
        {
            this.func = func;
        }

        public override void Start()
        {
        }

        public override TaskStatus DoAction()
        {
            if (func.Invoke())
                return TaskStatus.Success;

            return TaskStatus.Failure;
        }

        public override void End()
        {
        }
    }
}