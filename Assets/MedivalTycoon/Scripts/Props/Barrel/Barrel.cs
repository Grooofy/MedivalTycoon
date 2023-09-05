using UnityEngine;

public class Barrel : Props
{
    private Transform _point;

    private void Update()
    {
        TryMoveTo(_point);
    }

    public void SetPointToMove(Transform point)
    {
        _point = point;
    }

    internal override void TryMoveTo(Transform endPoint)
    {
        if (endPoint == null)
        {
            return;
        }
        MoveTo(endPoint);
    }
}
