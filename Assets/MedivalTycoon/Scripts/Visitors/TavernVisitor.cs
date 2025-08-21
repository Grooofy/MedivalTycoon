using UnityEngine;



namespace Visitors
{
    public class TavernVisitor : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public int BeerAmount { get; private set; }
        private int _minBeerAmount = 0;
        private int _maxBeerAmount;
        private StateMachine _stateMachine;
        private float _speed;
        private Vector3 _finishPosition;
        
        public void Initialize(StateMachine stateMachine, float speed, int maxBeerAmount)
        {
            Animator = GetComponentInChildren<Animator>();
            _maxBeerAmount = maxBeerAmount;
            _stateMachine = stateMachine;
            _speed = speed;
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
    }
}
