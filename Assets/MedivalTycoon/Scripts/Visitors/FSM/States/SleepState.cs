using Propses;
using System.Collections;
using UnityEngine;

namespace Visitors
{
    public class SleepState : IVisitorsState
    {
        private SleepVisitorMover _sleepVisitorMover;
        private SleepVisitorGiver _sleepVisitorTaker;
        private IProps _sleepVisitor;
        private TavernVisitor _tavernVisitor;
        private ParticleSystem _particalSystem;


        public SleepState(TavernVisitor tavernVisitor, ParticleSystem particleSystem, SleepVisitorMover sleepVisitorMover, VisitorProps sleepVisitor, SleepVisitorGiver sleepVisitorTaker)
        {
            _sleepVisitorMover = sleepVisitorMover;
            _sleepVisitorTaker = sleepVisitorTaker;
            _sleepVisitor = sleepVisitor;
            _tavernVisitor = tavernVisitor;
            _particalSystem = particleSystem;
        }

        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorSleep);
            _sleepVisitorMover.RegistSleepVisitor(_sleepVisitor);
            _particalSystem.Play();
        }

        public void UpdateState()
        {
            _sleepVisitorTaker.CheckHits();
        }

        public void Exit()
        {
            _particalSystem.Stop();
        }        
    }
}