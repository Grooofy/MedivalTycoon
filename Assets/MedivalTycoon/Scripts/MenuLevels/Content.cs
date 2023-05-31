using UnityEngine;

public class Content : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    public void SetStartPosition()
    {
        _rectTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
