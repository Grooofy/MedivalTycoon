using System;
using UnityEngine;

namespace Visitors
{
    public class TavernVisitor : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public int BeerAmount { get; private set; }
        public bool IsTrigger = true;
        public Action LeavingTavern;
        private StateEvent _previousStateEvent;
        private StateEvent _currentStateEvent;
        private WaitTimerUI _fiilImage;
        private Vector3 _finishPosition;
        private Vector3 _exitPoint;
        private StateMachine _stateMachine;
        private int _minBeerAmount = 3;
        private int _maxBeerAmount;
        private float _speed;
        private float _maxWaitTime;
        private bool _isMoving;

        public void Initialize(float speed, int maxBeerAmount, float maxWaitTime, Vector3 exitPoint)
        {
            Animator = GetComponentInChildren<Animator>();
            _fiilImage = GetComponentInChildren<WaitTimerUI>();
            _maxBeerAmount = maxBeerAmount;
            _maxWaitTime = maxWaitTime;
            _exitPoint = exitPoint;
            _speed = speed;
            _stateMachine = new StateMachine();
            _stateMachine.SetInitialState(new IdleState(this));
            AddTransitions();
        }

        public void SetRandomAmountBeer()
        {
            BeerAmount = UnityEngine.Random.Range(_minBeerAmount, _maxBeerAmount);
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
            _stateMachine.AddTransition<MoveState>(StateEvent.Wait, () => new WaitWaiterState(this, _fiilImage, _maxWaitTime, _exitPoint));
            _stateMachine.AddTransition<WaitWaiterState>(StateEvent.Move, () => new MoveState(this));
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

            _previousStateEvent = _currentStateEvent;
            _currentStateEvent = stateEvent;
            _stateMachine.ChangeState(_currentStateEvent);

            if (_currentStateEvent == StateEvent.Move && _previousStateEvent == StateEvent.Wait)
                LeavingTavern?.Invoke();
        }
    }
}
