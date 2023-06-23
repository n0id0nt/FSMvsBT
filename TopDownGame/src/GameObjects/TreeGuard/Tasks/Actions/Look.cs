using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class Look : Task
    {
        private Vector2 initialRot;

        private int ticks;
        private const int maxCount = 100;
        private const float rotationFactor = 9f;

        public Look(TreeGuard baseObject) : base(baseObject)
        {

        }

        public override void Start()
        {
            ticks = 0;
            initialRot = baseObject.facing;
            baseObject.Vel = new Vector2();
            baseObject.textureRect.Left = (int)baseObject.Size.X * 2;
        }

        public override TaskStatus DoAction()
        {
            // get the direction to rotate the base object
            if (ticks < maxCount / 4 || ticks > maxCount * 3 / 4)
                baseObject.facing.Rotate(rotationFactor / maxCount);
            else if (ticks < maxCount * 3 / 4)
                baseObject.facing.Rotate(-1 * rotationFactor / maxCount);

            if (ticks > maxCount)
            {
                baseObject.facing = initialRot;
                return TaskStatus.Success;
            }

            ticks++;
            return baseObject.TaskRunning(this);
        }

        public override void End()
        {
        }
    }
}
