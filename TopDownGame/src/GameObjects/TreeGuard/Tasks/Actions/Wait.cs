using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Wait : Task
    {
        private int ticks;
        private int maxTicks;

        public Wait(int time, TreeGuard baseObject) : base(baseObject)
        {
            maxTicks = time;
        }

        public override void Start()
        {
            ticks = 0;
        }

        public override TaskStatus DoAction()
        {
            if (ticks >= maxTicks)
                return TaskStatus.Success;
            ticks++;
            return baseObject.TaskRunning(this);
        }

        public override void End()
        {
        }
    }
}
