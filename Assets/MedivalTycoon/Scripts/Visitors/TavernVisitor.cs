using UnityEngine;

namespace Visitors
{
    public class TavernVisitor : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public int BeerAmount { get; private set; }
        private StateEvent _currentStateEvent;
        private int _minBeerAmount = 0;
        private int _maxBeerAmount;
        private StateMachine _stateMachine;
        private float _speed;
        private Vector3 _finishPosition;
        
        public void Initialize(float speed, int maxBeerAmount)
        {
            Animator = GetComponentInChildren<Animator>();
            _maxBeerAmount = maxBeerAmount;
            _speed = speed;
            _stateMachine = new StateMachine();
            _stateMachine.SetInitialState(new IdleState(this));
            AddTransitions();
        }
       
        public void SetRandomAmountBeer()
        {
            BeerAmount = Random.Range(_minBeerAmount, _maxBeerAmount);
        }

        public void SetFinishPosition(Vector3 position)
        {
            _finishPosition = position;
        }
       
        public void MoveToPoint()
        {
            if (_finishPosition == Vector3.zero) return;
            
            transform.position = Vector3.MoveTowards(transform.position, _finishPosition, _speed * Time.deltaTime);
        }

        private void AddTransitions()
        {
            _stateMachine.AddTransition<IdleState>(StateEvent.Move, ()=> new MoveState(this));
            _stateMachine.AddTransition<MoveState>(StateEvent.Idle, ()=> new IdleState(this));
            _stateMachine.AddTransition<MoveState>(StateEvent.Drink, ()=> new DrinkState(this));
            _stateMachine.AddTransition<DrinkState>(StateEvent.Sleep, ()=> new SleepState(this));
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
            _stateMachine.UpdateState();
        }
        
        public void ChangeState(StateEvent stateEvent)
        {
            if (_currentStateEvent == stateEvent) return;
            
            _currentStateEvent = stateEvent;
        }
    }
}
