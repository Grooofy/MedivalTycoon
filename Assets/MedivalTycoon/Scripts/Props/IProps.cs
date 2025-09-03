using System.Collections;
using UnityEngine;

namespace Propses
{
    public interface IProps
    {
        //public GameObject Prefab { get; }
        public void Initilization(Transform parent, float moveSpeed, Animator animator);
        public IEnumerator TryMoveTo(Point endPoint);
        public void Reset();
    }
}