using UnityEngine;

public interface IProps
{
    void SetActive(bool value);

    void Move(Vector3 endPoint);

    void SetNewParent(Transform newParent);
}
