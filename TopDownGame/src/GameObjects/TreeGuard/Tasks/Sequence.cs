using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Sequence : ParentTask
    {
        bool memory;
        int index = 0;

        public Sequence(TreeGuard baseObject, bool memory = false) : base(baseObject)
        {
            this.memory = memory;
        }

        public override void Start()
        {
            index = 0;
        }

        public override TaskStatus DoAction()
        {
            // reset index when not using index
            if (!memory)
            {
                index = 0;
            }
            for (;index < Subtasks.Count;index++)
            {
                if (CanStartSubStask(Subtasks[index]))
                {
                    Subtasks[index].Start();
                }

                TaskStatus result = Subtasks[index].DoAction();

                if (result == TaskStatus.Running)
                {
                    baseObject.AddToAciveTasks(this);
                    return TaskStatus.Running;
                }

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
