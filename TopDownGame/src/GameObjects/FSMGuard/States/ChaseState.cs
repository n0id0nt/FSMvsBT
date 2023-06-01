using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class ChaseState : IState
    {
        // reference to the base object
        private FSMGuard baseObject;

        // distance away from the player the guard needs to be before it starts shooting at the player
        private const float shootDist = 90f;

        public ChaseState(FSMGuard baseObject)
        {
            this.baseObject = baseObject;
        }

        public void Enter()
        {
            // change the image of the guard
            baseObject.textureRect.Left = (int)baseObject.Size.X;
        }

        public void Update()
        {
            // get reference to the player
            Player player = null;
            foreach (GameObject gameObject in Game.Scene.GameObjects)
                if (gameObject is Player)
                    player = (Player)gameObject;

            // use seek force to move towards the player
            baseObject.Acc = baseObject.Seek(player.Pos).Normalized() * baseObject.MaxForce;

            if (Vector2.Distance(player.Pos, baseObject.Pos) < shootDist)
                baseObject.stateManager.ChangeState("Aim");
            
            else if(!baseObject.IsPlayerInRange())
                baseObject.stateManager.ChangeState("Look");           
        }

        public void Draw()
        {

        }

        public void Exit()
        {

        }
    }
}
