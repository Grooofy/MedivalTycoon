using System;
using UnityEngine;

public interface IProps
{
    void SetActive(bool value);

    Vector3 GetPosition();

    void Move(Vector3 endPoint);

    void SetNewParent(Transform newParent);
}
