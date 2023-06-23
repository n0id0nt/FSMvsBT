using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    abstract class ParentTask : Task
    {
        protected List<Task> Subtasks;

        public ParentTask(TreeGuard baseObject) : base(baseObject)
        {
            Subtasks = new List<Task>();
        }

        public void Add(Task task)
        {
            Subtasks.Add(task);
        }

        protected bool CanStartSubStask(Task task)
        {
            return (baseObject.ActiveTask != task);
        }
    }
}
