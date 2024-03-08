using in3d.Utilities.StateMachine.interfaces;

namespace in3d.Utilities.GameLogic.StateMachine
{
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }

        public Transition(IState to, IPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}