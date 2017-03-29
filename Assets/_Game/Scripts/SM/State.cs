using System;

namespace Tools.Patterns.StateMachine
{
    interface IConcreteState
    {
        void Handle(Context context);
    }

    public abstract class State : IConcreteState
    {
        public virtual void Handle(Context context)
        {
           // TODO check this context.state = this;
        }
    }
}
