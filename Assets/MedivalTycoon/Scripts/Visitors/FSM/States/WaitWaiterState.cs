using UnityEngine;
using UnityEngine.UI;

namespace Visitors
{
    public class WaitWaiterState : IVisitorsState
    {
        private TavernVisitor _tavernVisitor;
        private WaitTimerUI _waitTimerUI;
        private float _maxWaitTime = 30f;

        private float elapsed = 0f;


        public WaitWaiterState(TavernVisitor tavernVisitor, WaitTimerUI image)
        {
            _waitTimerUI = image;
            _tavernVisitor = tavernVisitor;
        }

        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorIdle);
            StartWaitingForOrder();
            Debug.Log("START WAITE");
        }

        public void UpdateState()
        {
            WaitForOrderRoutine();
            Debug.Log("WAITE");
        }

        public void Exit()
        {
            OnOrderDelivered();
        }

        private void WaitForOrderRoutine()
        {

            elapsed += Time.deltaTime;
            float progress = 1f - (elapsed / _maxWaitTime);

            if (_waitTimerUI != null)
            {
                _waitTimerUI.SetFill(progress);
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