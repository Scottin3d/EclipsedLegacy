
using System;
using in3d.Utilities.StateMachine.interfaces;

namespace in3d.Utilities.GameLogic.StateMachine
{
    /// <summary>
    /// Represents a predicate that is based on a provided function.
    /// </summary>
    public class FuncPredicate : IPredicate
    {
        readonly Func<bool> func;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncPredicate"/> class with the specified function.
        /// </summary>
        /// <param name="func">The function to be used for evaluation.</param>
        public FuncPredicate(Func<bool> func)
        {
            this.func = func;
        }

        /// <summary>
        /// Evaluates the predicate by invoking the underlying function.
        /// </summary>
        /// <returns>The result of the function invocation.</returns>
        public bool Evaluate() => func.Invoke();
    }
}