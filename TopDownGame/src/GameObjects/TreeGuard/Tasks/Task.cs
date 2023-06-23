using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    enum TaskStatus
    {
        Running,
        Success,
        Failure
    }

    abstract class Task
    {
        protected TreeGuard baseObject;

        public Task(TreeGuard baseObject)
        {
            this.baseObject = baseObject;
        }

        public abstract void Start();

        public abstract TaskStatus DoAction();

        public abstract void End();
    }
}
