using UnityEditor;

namespace Visitors
{
    public interface IVisitorsState
    {
        public void Enter();
        public void UpdateState();
        public void Exit();
    }
}