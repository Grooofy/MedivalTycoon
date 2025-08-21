using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Visitors
{
    public class VisitorController 
    {
        private StateMachine _stateMachine;
        private StateEvent _currentStateEvent;

        public VisitorController(TavernVisitor tavernVisitor, float speed, int maxBeerCount)
        {
            _stateMachine = new StateMachine();
            tavernVisitor.Initialize(_stateMachine, speed, maxBeerCount);
        }
        
        public void ChangeState(StateEvent stateEvent)
        {
            _stateMachine.ChangeState(stateEvent);
        }

        public void UpdateState()
        {
            switch (_currentStateEvent)
            {
                case StateEvent.Idle:
                    _stateMachine.ChangeState(StateEvent.Idle);
                    break;
                case StateEvent.Move :
                    _stateMachine.ChangeState(StateEvent.Move);
                    break;
                case StateEvent.Drink :
                    _stateMachine.ChangeState(StateEvent.Drink);
                    break;
                case StateEvent.Sleep :
                    _stateMachine.ChangeState(StateEvent.Sleep);
                    break;
            }
            _stateMachine.Update();
        }
    }
}