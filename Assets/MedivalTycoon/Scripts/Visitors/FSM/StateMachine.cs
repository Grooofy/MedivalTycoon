using System;
using System.Collections.Generic;


namespace Visitors
{
    public class StateMachine
    {
        private IVisitorsState currentState;
       
        private Dictionary<(Type, StateEvent), Func<IVisitorsState>> transitions = new Dictionary<(Type, StateEvent), Func<IVisitorsState>>();

        public void SetInitialState(IVisitorsState state)
        {
            currentState = state;
            currentState.Enter();
        }

        public void AddTransition<TState>(StateEvent stateEvent, Func<IVisitorsState> targetStateFactory) where TState : IVisitorsState
        {
            transitions[(typeof(TState), stateEvent)] = targetStateFactory;
        }

        public void ChangeState(StateEvent stateEvent)
        {
            var key = (currentState.GetType(), evt: stateEvent);
            if (transitions.TryGetValue(key, out var targetStateFactory))
            {
                currentState.Exit();
                currentState = targetStateFactory();
                currentState.Enter();
            }
        }

        public void Update()
        {
            currentState?.UpdateState();
        }
    }
}