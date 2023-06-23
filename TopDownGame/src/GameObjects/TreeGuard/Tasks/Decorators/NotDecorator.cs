using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    /// <summary>
    /// Will flip success and failure but running will be passed through
    /// </summary>
    class NotDecorator : Decorator
    {
        public NotDecorator(TreeGuard baseObject, Task subTask) : base(baseObject, subTask)
        {
        }

        public override void Start()
        {
            subTask.Start();
        }

        public override TaskStatus DoAction()
        {
            TaskStatus result = subTask.DoAction();
            if (result == TaskStatus.Success)
                return TaskStatus.Failure;
            if (result == TaskStatus.Failure)
                return TaskStatus.Success;
            return result;
        }

        public override void End()
        {
            subTask.End();
        }
    }
}