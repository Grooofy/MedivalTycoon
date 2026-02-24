using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visitors
{
    public class WaitWaiterState : IVisitorsState
    {
        private TavernVisitor _tavernVisitor;
        private WaitTimerUI _waitTimerUI;
        private Queue<Vector3> _exitWay;
        private float _maxWaitTime;

        private float _elapsed;
        private float _progress;


        public WaitWaiterState(TavernVisitor tavernVisitor, WaitTimerUI image, float maxWaitTime, Stack<Vector3> exitWay)
        {
            _maxWaitTime = maxWaitTime;
            _waitTimerUI = image;
            _tavernVisitor = tavernVisitor;
            _exitWay = new Queue<Vector3>(exitWay); 
            _waitTimerUI.Initialize();
        }

        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorIdle);
            StartWaitingForOrder();
        }

        public void UpdateState()
        {
            WaitForOrderRoutine();
        }

        public void Exit()
        {
            OnOrderDelivered();
        }

        private void WaitForOrderRoutine()
        {
            _elapsed += Time.deltaTime;
             _progress = 1f - (_elapsed / _maxWaitTime);

            if (_waitTimerUI != null)
            {
                _waitTimerUI.SetFill(_progress);
            }

            if(_progress <= 0)
            {                
                _tavernVisitor.GoTo(_exitWay);                
            }
        }

        private void OnOrderDelivered()
        {
            if (_waitTimerUI != null)
            {
                _waitTimerUI.SetActive(false);
            }
        }

        private void StartWaitingForOrder()
        {
            if (_waitTimerUI != null)
            {
                _waitTimerUI.SetActive(true);
                _waitTimerUI.SetFill(1f);
            }
        }
    }
}