using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class LookState : IState
    {
        private FSMGuard baseObject;
        private Vector2 initialRot;

        private int ticks;
        private const int maxCount = 100;
        private const float rotationFactor = 9f;

        public LookState(FSMGuard fSMGuard)
        {
            baseObject = fSMGuard;
        }

        public void Enter()
        {
            ticks = 0;
            initialRot = baseObject.facing;
            baseObject.Acc = new Vector2();
            baseObject.Vel = new Vector2();
            baseObject.textureRect.Left = (int)baseObject.Size.X * 2;
        }

        public void Update()
        {
            // get the direction to rotate the base object
            if (ticks < maxCount / 4 || ticks > maxCount * 3 / 4)
                baseObject.facing.Rotate(rotationFactor/maxCount);
            else if (ticks < maxCount * 3 / 4)
                baseObject.facing.Rotate(-1 * rotationFactor / maxCount);
                

            if (baseObject.IsPlayerInSight())
                baseObject.stateManager.ChangeState("Chase");

            else if (ticks > maxCount)
                baseObject.stateManager.ChangeState("Patrol");

            ticks++;
        }

        public void Draw()
        {

        }

        public void Exit()
        {
            // set the bace object facing to the initial rotation 
            baseObject.facing = initialRot;
        }
    }
}
