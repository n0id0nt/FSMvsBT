using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Sequence : ParentTask
    {
        public Sequence(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
        }

        public override TaskStatus DoAction()
        {
            foreach (Task task in Subtasks)
            {
                if (CanStartSubStask(task))
                {
                    task.Start();
                }

                TaskStatus result = task.DoAction();

                if (result == TaskStatus.Running)
                    return TaskStatus.Running;

                if (result == TaskStatus.Success)
                {
                    //CurTask.End();
                }

                else if (result == TaskStatus.Failure)
                {
                    //CurTask.End();
                    return result;
                }
            }
            return TaskStatus.Success;
        }

        public override void End()
        {
        }
    }
}
