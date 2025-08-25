using UnityEngine;

namespace Visitors
{
    public class DrinkState : IVisitorsState
    {
        private TavernVisitor _tavernVisitor;

        public DrinkState(TavernVisitor tavernVisitor)
        {
            _tavernVisitor = tavernVisitor;
        }
        
        
        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorDrink);
        }

        public void UpdateState()
        {
            Debug.Log("Drink state");
        }

        public void Exit()
        {
           
        }
    }
}