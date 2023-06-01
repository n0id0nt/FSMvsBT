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

        public override TaskStatus DoAction()
        {
            for (int i = Subtasks.IndexOf(CurTask); i < Subtasks.Count; i++)
            {
                CurTask = Subtasks[i];

                if (!CurTask.Started)
                {
                    CurTask.Start();
                }
                TaskStatus result = CurTask.DoAction();

                if (result == TaskStatus.Running)
                    return TaskStatus.Running;

                if (result == TaskStatus.Success)
                {
                    CurTask.End();
                }                

                else if (result == TaskStatus.Failure)
                {
                    return TaskStatus.Failure;
                    CurTask.End();
                }
            }
            return TaskStatus.Success;
        }
    }
}
