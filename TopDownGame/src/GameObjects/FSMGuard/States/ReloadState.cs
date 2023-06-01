using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class ReloadState : IState
    {
        public FSMGuard baseObject;
        private int ticks;
        private const int maxTicks = 20;

        public ReloadState(FSMGuard baseObject)
        {
            this.baseObject = baseObject;
        }

        public void Enter()
        {
            baseObject.textureRect.Left = (int)baseObject.Size.X * 4;
            ticks = 0;
        }

        public void Update()
        {
            if (ticks > maxTicks)
            {
                // reset the ammo
                baseObject.ammo = FSMGuard.maxAmmo;

                if (baseObject.IsPlayerInRange())
                    baseObject.stateManager.ChangeState("Aim");

                else
                    baseObject.stateManager.ChangeState("Look");
            }

            ticks++;
        }

        public void Draw()
        {

        }

        public void Exit()
        {

        }
    }
}
