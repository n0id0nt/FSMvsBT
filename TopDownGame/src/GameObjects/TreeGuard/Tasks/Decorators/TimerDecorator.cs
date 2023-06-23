using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    /// <summary>
    /// Will return the value of the underlying task until the timer has finished then it will return Success
    /// </summary>
    class TimerDecorator : Decorator
    {
        private int ticks;
        private int maxTicks;

        public TimerDecorator(TreeGuard baseObject, Task subTask, int time) : base(baseObject, subTask)
        {
            maxTicks = time;
        }

        public override void Start()
        {
            ticks = 0;
            subTask.Start();
        }

        public override TaskStatus DoAction()
        {
            TaskStatus result = subTask.DoAction();
            if (ticks >= maxTicks)
                result = TaskStatus.Success;
            ticks++;
            AddToActive(result);
            return result;
        }

        public override void End()
        {
            subTask.End();
        }
    }
}
