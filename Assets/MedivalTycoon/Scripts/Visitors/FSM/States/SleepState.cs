using UnityEngine;

namespace Visitors
{
    public class SleepState : IVisitorsState
    {
        
        private TavernVisitor _tavernVisitor;

        public SleepState(TavernVisitor tavernVisitor)
        {
            _tavernVisitor = tavernVisitor;
        }
        
        public void Enter()
        {
            AnimatorExtensions.Play(_tavernVisitor.Animator, AnimatorParameters.VisitorSleep);
        }

        public void UpdateState()
        {
            Debug.Log("Sleep state");
        }

        public void Exit()
        {
        }
    }
}