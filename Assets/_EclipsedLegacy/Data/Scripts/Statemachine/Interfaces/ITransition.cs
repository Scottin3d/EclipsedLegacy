namespace in3d.Utilities.StateMachine.interfaces
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}