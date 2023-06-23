using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    /// <summary>
    /// Will flip success and failure but running will be passed through
    /// </summary>
    abstract class Decorator : Task
    {
        protected Task subTask;

        public Decorator(TreeGuard baseObject, Task subTask) : base(baseObject)
        {
            this.subTask = subTask;
        }
    }
}
