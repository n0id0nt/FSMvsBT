using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    abstract class ParentTask : Task
    {
        protected List<Task> Subtasks;
        protected Task CurTask;

        public ParentTask(TreeGuard baseObject) : base(baseObject)
        {
            Subtasks = new List<Task>();
        }

        public void Add(Task task)
        {
            Subtasks.Add(task);
        }

        public override void Start()
        {
            CurTask = Subtasks[0];
            Started = true;
        }

        public override void End()
        {
            Started = false;
            foreach (Task task in Subtasks)
                if (task.Started)
                    task.End();
        }
    }
}
