using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Selector : ParentTask
    {
        public Selector(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override TaskStatus DoAction()
        {   
            
            for (int i = 0; i < Subtasks.Count; i++)
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
                    return TaskStatus.Success;
                }
                
                else if (result == TaskStatus.Failure)
                {
                    CurTask.End();
                }
            } 
            return TaskStatus.Failure;
        }
    }
}
