using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class RootTask
    {
        private Task task;

        public RootTask(Task task)
        {
            this.task = task;
        }

        public void Update()
        {
            //task.Start();

            TaskStatus result = task.DoAction();

            //if (result != TaskStatus.Running) task.End();
        }
    }
}
