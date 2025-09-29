using UnityEngine;
using UnityEngine.UI;

namespace Visitors
{
    public class TavernVisitor : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public int BeerAmount { get; private set; }
        private StateEvent _currentStateEvent;
        private WaitTimerUI _fiilImage;
        private int _minBeerAmount = 3;
        private int _maxBeerAmount;
        private StateMachine _stateMachine;
        private float _speed;
        private Vector3 _finishPosition;
        private bool _isMoving;

        public void Initialize(float speed, int maxBeerAmount)
        {
            Animator = GetComponentInChildren<Animator>();
            _fiilImage = GetComponentInChildren<WaitTimerUI>();
            _maxBeerAmount = maxBeerAmount;
            _speed = speed;
            _stateMachine = new StateMachine();
            _stateMachine.SetInitialState(new IdleState(this));
            AddTransitions();
            //_fiilImage.Initialize();
        }

        public void SetRandomAmountBeer()
        {
            BeerAmount = Random.Range(_minBeerAmount, _maxBeerAmount);
        }

        public void GoTo(Vector3 position)
        {
            _finishPosition = position;
            _isMoving = true;
            ChangeState(StateEvent.Move);
        }

        public void MoveToPoint()
        {
            if (_isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, _finishPosition, _speed * Time.deltaTime);
                transform.LookAt(_finishPosition);

                if (Vector3.Distance(transform.position, _finishPosition) < 0.05f)
                {
                    transform.position = _finishPosition;
                    _isMoving = false;
                }
            }
        }

        private void AddTransitions()
        {
            _stateMachine.AddTransition<IdleState>(StateEvent.Move, () => new MoveState(this));
            _stateMachine.AddTransition<MoveState>(StateEvent.Waite, () => new WaitWaiterState(this, _fiilImage));
            _stateMachine.AddTransition<WaitWaiterState>(StateEvent.Drink, () => new DrinkState(this));
            _stateMachine.AddTransition<DrinkState>(StateEvent.Sleep, () => new SleepState(this));
        }

        public void UpdateState()
        {
            _stateMachine.UpdateState();
        }

        public void ChangeState(StateEvent stateEvent)
        {
            if (_currentStateEvent == stateEvent) return;

            _currentStateEvent = stateEvent;
            _stateMachine.ChangeState(_currentStateEvent);
        }
    }
}
