using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class AimState : IState
    {
        // reference to the base object
        public FSMGuard baseObject;
        // the amount of update cycles the state 
        private int ticks;
        // the max amount of ticks before changing state
        private const int maxTicks = 35;
        // the distance the player can move away before the guard will move closer to the player
        private const float chaseDist = 100f;

        public AimState(FSMGuard baseObject)
        {
            this.baseObject = baseObject;
        }

        public void Enter()
        {
            // reset the force vectors to the player stops moving
            baseObject.Acc = new Vector2();
            baseObject.Vel = new Vector2();

            // change the image displayed
            baseObject.textureRect.Left = (int)baseObject.Size.X * 3;

            // reset the ticks
            ticks = 0;
        }

        public void Update()
        {
            // find the player
            Player player = null;
            foreach (GameObject gameObject in Game.Scene.GameObjects)
                if (gameObject is Player)
                    player = (Player)gameObject;

            // face the player
            baseObject.facing = (player.Pos - baseObject.Pos).Normalized();

            if (Vector2.Distance(player.Pos, baseObject.Pos) > chaseDist)
                baseObject.stateManager.ChangeState("Chase");
            
            else if (!baseObject.IsPlayerInRange())
                baseObject.stateManager.ChangeState("Look");
           
            else if (ticks > maxTicks)
                baseObject.stateManager.ChangeState("Shoot");
            
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
