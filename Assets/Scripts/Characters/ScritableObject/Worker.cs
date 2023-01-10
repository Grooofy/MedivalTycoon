using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Worker", menuName = "Workers", order = 41)]
public class Worker : ScriptableObject
{
    [SerializeField] private float _speed;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _id;
    
    public bool IsSelect;
    
    public float Speed => _speed;
    public Sprite Icon => _icon;
    public int Id => _id;
    
    public void ChangeValueSelect()
    {
        IsSelect = !IsSelect;
    }
}