
using System;
using in3d.Utilities.StateMachine.interfaces;

namespace in3d.Utilities.GameLogic.StateMachine
{
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> func;

        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }
        public bool Evaluate() => func.Invoke();
    }
}