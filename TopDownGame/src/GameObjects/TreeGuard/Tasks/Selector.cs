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
                    return result;
                }
                
                else if (result == TaskStatus.Failure)
                {
                    //CurTask.End();
                }
            } 
            return TaskStatus.Failure;
        }

        public override void End()
        {
        }
    }
}
