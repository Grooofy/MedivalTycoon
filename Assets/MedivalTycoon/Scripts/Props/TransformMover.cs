using UnityEngine;

namespace Propses
{
    public class TransformMover
    {
        private Point _currentTarget = null;

        public void MoveTo(Transform transform, Point endPoint, float moveSpeed)
        {
            if (endPoint.IsFill) return;
            
            if (_currentTarget != endPoint)
            {
                transform.SetParent(endPoint.transform);
                _currentTarget = endPoint;
            }

            transform.position =
                Vector3.MoveTowards(transform.position, endPoint.transform.position, moveSpeed * Time.deltaTime);

            if (IsMinDistance(transform.position, endPoint.transform.position))
            {
                endPoint.Fill();
                _currentTarget = null; 
            }
        }

        private bool IsMinDistance(Vector3 startPosition, Vector3 endPosition)
        {
            float minDistance = 0.001f;
            return Vector3.Distance(startPosition, endPosition) < minDistance;
        }
    }

    
}