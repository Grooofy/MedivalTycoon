using UnityEngine;

namespace Visitors
{
    public class DrinkState : IVisitorsState
    {
        private TavernVisitor _tavernVisitor;
        private Beer _beerModel;

        public DrinkState(TavernVisitor tavernVisitor, Beer beerModel)
        {
            if (beerModel == null)
                return;
            
            _beerModel = beerModel;
            _beerModel.gameObject.SetActive(false);
            _tavernVisitor = tavernVisitor;
        }
        
        
        public void Enter()
        {
            AnimatorExtensions.Set(_tavernVisitor.Animator, AnimatorParameters.VisitorDrink);
            _beerModel.gameObject.SetActive(true);
        }

        public void UpdateState()
        {
            Debug.Log("Drink state");
        }

        public void Exit()
        {
            _beerModel.gameObject.SetActive(false);
        }
    }
}