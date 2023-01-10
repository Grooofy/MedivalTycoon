using UnityEngine;

public class ModelButton : MonoBehaviour
{
    [SerializeField] private Worker _worker;

    public void SelectWorker()
    {
        _worker.ChangeValueSelect();
    }

    public Sprite GetIcon()
    {
        return _worker.Icon;
    }

    public bool GetStatus()
    {
        return _worker.IsSelect;
    }

    public int GetId()
    {
        return _worker.Id;
    }

}