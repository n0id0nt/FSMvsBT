using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    interface IState
    {
        void Enter();

        void Update();

        void Draw();

        void Exit();
    }
}
