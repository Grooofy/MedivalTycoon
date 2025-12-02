using UnityEngine;

namespace Visitors
{
    public class SleepState : IVisitorsState
    {
        
        private TavernVisitor _tavernVisitor;
        public ParticleSystem _particalSystem; 

        public SleepState(TavernVisitor tavernVisitor, ParticleSystem particleSystem)
        {
            _tavernVisitor = tavernVisitor;
            _particalSystem = particleSystem;
        }
        
        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorSleep);
            _particalSystem.Play();
        }

        public void UpdateState()
        {
            Debug.Log("Sleep state");
        }

        public void Exit()
        {
            _particalSystem.Stop();
        }
    }
}