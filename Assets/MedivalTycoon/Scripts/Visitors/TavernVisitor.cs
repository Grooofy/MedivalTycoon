using Events;
using Propses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using UnityEngine;

namespace Visitors
{
    public class TavernVisitor : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        public int BeerAmount { get; private set; }
        public Action LeavingTavern;
        public bool IsMoving;
        private bool _isIpropsInit;

        private StateEvent _previousStateEvent;
        private StateEvent _currentStateEvent;
        private WaitTimerUI _fiilImage;
        private Vector3 _finishPosition;
        private Vector3 _exitPoint;
        private Stack<Vector3> _exitWay = new();
        private Queue<Vector3> _wayPoint;
        private StateMachine _stateMachine;
        private Seat _seat;
        private Beer _beerModel;
        private VisitorProps _visitorProps;
        private SleepVisitorMover _sleepVisitorMover;
        private SleepVisitorGiver _sleepVisitorTaker;
        private ParticleSystem _particleSystem;
        private LayerMask layerMask;
        private int _minBeerAmount = 3;
        private int _maxBeerAmount;
        private float _speed;
        private float _maxWaitTime;

        public void Initialize(float speed, int maxBeerAmount, float maxWaitTime, Vector3 exitPoint)
        {
            Animator = GetComponentInChildren<Animator>();
            _fiilImage = GetComponentInChildren<WaitTimerUI>();
            _beerModel = GetComponentInChildren<Beer>();
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _maxBeerAmount = maxBeerAmount;
            _maxWaitTime = maxWaitTime;
            _exitPoint = exitPoint;
            _speed = speed;
            _stateMachine = new StateMachine();
            _stateMachine.SetInitialState(new IdleState(this));
            _beerModel.gameObject.SetActive(false);
            AddTransitions();
        }       

        public void InitializeIpropsVisitor(float speed, LayerMask layerMask)
        {
            _visitorProps = GetComponentInChildren<VisitorProps>();
            _sleepVisitorMover = GetComponent<SleepVisitorMover>();
            _sleepVisitorTaker = GetComponent<SleepVisitorGiver>();

            _visitorProps.Initilization(this.transform, speed, Animator);
            _sleepVisitorTaker.Initialize(_sleepVisitorMover, layerMask);
            _visitorProps.SetVisitor(this);
            _isIpropsInit = true;
        }

        public void SetRandomAmountBeer()
        {
            BeerAmount = UnityEngine.Random.Range(_minBeerAmount, _maxBeerAmount);
        }

        public void GoTo(Queue<Vector3> way)
        {
            _wayPoint = way;
            IsMoving = true;
            ChangeState(StateEvent.Move);
        }

        public void SavePoint(Seat seat)
        {
            _seat = seat;
        }

        public void ClearPoint()
        {
            _seat.OnVisitorLeftTavern();
            EventBus.Raise(new VisitorLeaveTavern());
        }

        public StateEvent GetState()
        {
            return _currentStateEvent;
        }

        public void MoveToPoint()
        {
            if (IsMoving)
            {    
                _wayPoint.TryPeek(out _finishPosition);
                
                transform.position = Vector3.MoveTowards(transform.position, _finishPosition, _speed * Time.deltaTime);
                transform.LookAt(_finishPosition);

                if (Vector3.Distance(transform.position, _finishPosition) < 0.05f)
                {
                    transform.position = _finishPosition;
                    RememberExitWay(_wayPoint.Dequeue());
                    
                    if(_wayPoint.Count == 0)
                    {
                        IsMoving = false;
                    }                    
                }                
            }
        }       

        private void RememberExitWay(Vector3 point)
        {
            if(_exitWay.Count == 0)
                _exitWay.Push(_exitPoint);

            _exitWay.Push(point);
        }

        private void AddTransitions()
        {
            _stateMachine.AddTransition<IdleState>(StateEvent.Move, () => new MoveState(this));
            _stateMachine.AddTransition<MoveState>(StateEvent.Wait, () => new WaitWaiterState(this, _fiilImage, _maxWaitTime, _exitWay));
            _stateMachine.AddTransition<WaitWaiterState>(StateEvent.Move, () => new MoveState(this));
            _stateMachine.AddTransition<WaitWaiterState>(StateEvent.Drink, () => new DrinkState(this, _beerModel));
            
            if(_isIpropsInit)
                _stateMachine.AddTransition<DrinkState>(StateEvent.Sleep, () => new SleepState(this, _particleSystem, _sleepVisitorMover, _visitorProps, _sleepVisitorTaker));
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
