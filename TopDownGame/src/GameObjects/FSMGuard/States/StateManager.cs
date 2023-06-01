using System;
using System.Collections.Generic;
using System.Text;

namespace TopDownGame
{
    class StateManager
    {
        private Dictionary<string, IState> states;

        public IState CurState { get; private set; }

        public StateManager(Dictionary<string, IState> states)
        {
            this.states = states;
        }

        /// <summary>
        /// Change the state
        /// </summary>
        /// <param name="state">state to change to</param>
        public void ChangeState(string state)
        {
            if (CurState != null)
                CurState.Exit();

            CurState = states[state];

            CurState.Enter();
        }

        public void Update()
        {
            CurState.Update();
        }

        public void Draw()
        {
            CurState.Draw();
        }
    }
}
