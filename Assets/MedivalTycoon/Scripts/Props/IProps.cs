using System.Collections;
using UnityEngine;

namespace Propses
{
    public interface IProps
    {       
        public void Initilization(Transform spawnPoint, float moveSpeed, Animator animator);
        public IEnumerator TryMoveTo(Point endPoint);
        public void Reset();
    }
}