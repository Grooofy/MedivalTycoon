using MedivalTycoon.Scripts.Events;
using UnityEngine;

namespace Propses
{
    public class TransformMover
    {
        public void MoveTo(Transform transform, Point endPoint, float moveSpeed)
        {
            if (endPoint == null) return;

            transform.position =
                Vector3.MoveTowards(transform.position, endPoint.transform.position, moveSpeed * Time.deltaTime);
            transform.SetParent(endPoint.transform);

            if (IsMinDistance(transform.position, endPoint.transform.position))
            {
                EventBus.Raise(new PointFillingEvent());
                endPoint.Fill();
            }
        }

        private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
        {
            float minDistance = 0.001f;
            return Vector3.Distance(startPosition, endPosition) < minDistance;
        }
    }
}