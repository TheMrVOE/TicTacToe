using System;

namespace Tools.Patterns.StateMachine
{
    public class Context
    {
        internal State state { get; set; }
        public event Action<State> onStateChanging;
        public event Action<State> onStateChanged;

        public Context(State defaultState)
        {
            state = defaultState;
        }

        public void SwitchState()
        {
            if(onStateChanging!=null)
            {
                onStateChanging.Invoke(this.state);
            }

            this.state = state;
            state.Handle(this);

            if(onStateChanged!=null)
            {
                onStateChanged.Invoke(this.state);
            }
        }
        public void SetState(State state)
        {
            if (onStateChanging != null)
            {
                onStateChanging.Invoke(this.state);
            }

            this.state = state;

            if (onStateChanged != null)
            {
                onStateChanged.Invoke(this.state);
            }
        }
        public void SetStateSilent(State state)
        {
            this.state = state;
        }
    }
}