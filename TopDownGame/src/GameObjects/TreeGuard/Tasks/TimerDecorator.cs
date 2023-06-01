using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    /// <summary>
    /// Will return the value of the underlying task until the timer has finished then it will return Success
    /// </summary>
    class TimerDecorator : Task
    {
        private int ticks;
        private int maxTicks;
        private Task subTask;

        public TimerDecorator(TreeGuard baseObject, Task subTask, int time) : base(baseObject)
        {
            maxTicks = time;
            this.subTask = subTask;
        }

        public override void Start()
        {
            ticks = 0;
            Started = true;
            subTask.Start();
        }

        public override TaskStatus DoAction()
        {
            TaskStatus result = subTask.DoAction();
            if (ticks >= maxTicks)
                result = TaskStatus.Success;
            ticks++;
            return result;
        }

        public override void End()
        {
            Started = false;
            subTask.End();
        }
    }
}
