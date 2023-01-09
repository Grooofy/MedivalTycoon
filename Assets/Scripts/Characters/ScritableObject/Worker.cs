using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Worker", menuName = "Workers", order = 41)]
public class Worker : ScriptableObject
{
    [SerializeField] private float _speed;
    [SerializeField] private bool _select;
    [SerializeField] private Sprite _icon;
    
    public bool Select => _select;
    public float Speed => _speed;
    public Sprite Icon => _icon;

    public UnityAction Selected;

    public void ChangeValueSelect()
    {
        _select = !_select;
        Selected.Invoke();
    }
}