using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class ShootState : IState
    {
        private FSMGuard baseObject;
        private int ticks;
        private const int maxTicks = 20;

        public ShootState(FSMGuard baseObject)
        {
            this.baseObject = baseObject;
        }

        public void Enter()
        {
            baseObject.textureRect.Left = (int)baseObject.Size.X * 3;
            ticks = 0;
        }

        public void Update()
        {
            if (ticks > maxTicks)
            {
                // shoot the guards gun
                baseObject.Shoot();
                // decrease the ammo the player has
                baseObject.ammo--;

                if (baseObject.ammo <= 0)
                    baseObject.stateManager.ChangeState("Reload");
                else
                    baseObject.stateManager.ChangeState("Aim");
            }
            else if (ticks > maxTicks*3/4)
            {
                // change the image
                baseObject.textureRect.Left = (int)baseObject.Size.X * 4;
            }
            else if (!baseObject.IsPlayerInRange())
            {
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
