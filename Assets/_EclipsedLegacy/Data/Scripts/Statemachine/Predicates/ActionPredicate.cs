
using System;
using in3d.Utilities.StateMachine.interfaces;

namespace in3d.Utilities.GameLogic.StateMachine
{
    /// <summary>
    /// Represents a predicate that encapsulates an action and evaluates to true once the action has been invoked.
    /// </summary>
    /// <example>
    /// <code>
    /// public Action OnJump;
    /// stateMachine.AddTransition(standing, jumping, new ActionPredicate(OnJump));
    /// </code>
    /// </example>
    public class ActionPredicate : IPredicate {
        bool flag;
        
        public ActionPredicate(Action eventReaction) => eventReaction += () => { flag = true; };
        
        public bool Evaluate() {
            bool result = flag;
            flag = false;
            return result;
        }
    }
}