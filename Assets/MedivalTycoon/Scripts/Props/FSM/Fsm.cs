using System;
using System.Collections.Generic;

public class Fsm
{
    private FsmState _currentState;
    
    private Dictionary<Type, FsmState> _states = new Dictionary<Type, FsmState>();

    public void AddState(FsmState state)
    {
        _states.Add(state.GetType(), state);
    }
    
    public void SetState<T>() where T : FsmState
    {
        var type = typeof(T);

        if (_currentState.GetType() == type)
            return;
        
        if (_states.TryGetValue(type, out var newState))
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }

    public void Update()
    {
        _currentState?.Update();
    }
    
}
