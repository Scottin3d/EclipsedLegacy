using System;
using System.Collections.Generic;
using in3d.Utilities.StateMachine.interfaces;

namespace in3d.Utilities.GameLogic.StateMachine
{
    public class StateMachine {
        StateNode current;

        Dictionary<Type, StateNode> nodes = new();
        HashSet<ITransition> anyTransitions = new();


        public void Update(){
            var transition = GetTransition();
            if (transition != null) ChangeState(transition.To);

            current.State?.Update();
        }

        public void FixedUpdate(){
            current.State?.FixedUpdate();
        }

        public void SetState(IState state){
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        private void ChangeState(IState state)
        {
            if (state == current.State) return;
            var previousState = current.State;
            var nextState = nodes[state.GetType()];

            previousState?.OnExit();
            nextState.State?.OnEnter();
            current = nodes[state.GetType()];
        }

        private ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition.Evaluate()) return transition;
            }

            foreach (var transition in current.transitions)
            {
                if (transition.Condition.Evaluate()) return transition;
            }

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(IState state){
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null){
                node = new StateNode(state);
                nodes[state.GetType()] = node;
            }

            return node;
        }
        

        class StateNode {
            public IState State { get;}
            public HashSet<Transition> transitions { get; }

            public StateNode(IState state) {
                this.State = state;
                transitions = new HashSet<Transition>();
            }

            public void AddTransition(IState to, IPredicate condition) {
                transitions.Add(new Transition(to, condition));
            }
        }
    }
}
