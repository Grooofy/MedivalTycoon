using MedivalTycoon;
using UnityEngine;
using UnityEngine.Pool;

namespace Propses
{
    public class PropsPool<T> : MonoBehaviour, IPropsPool where T : Component, IProps
    {
        [SerializeField] private T _prefab;     
        [SerializeField] private float _moveSpeed;

        private ObjectPool<IProps> _pool;

        public IProps Spawn()
        {
            return _pool.Get();
        }

        public void Despawn(IProps prop)
        {
            _pool.Release(prop);
        }

        public PropsPool<T> Initialize(int defaultSize, int maxSize)
        {
            _pool = new ObjectPool<IProps>(
                CreateProp,
                OnGetProp,
                OnReleaseProp,
                OnDestroyProp,
                collectionCheck: false,
                defaultCapacity: defaultSize,
                maxSize: maxSize
            );
            return this;
        }

        private IProps CreateProp()
        {
            var instance = Instantiate(_prefab, transform);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnGetProp(IProps prop)
        {
            var component = prop as T;
            component.gameObject.SetActive(true);
            component.Initilization(transform, _moveSpeed, component.GetComponent<Animator>());
        }

        private void OnReleaseProp(IProps prop)
        {
            var component = prop as T;
            component.gameObject.SetActive(false);
            component.Reset();
        }

        private void OnDestroyProp(IProps prop)
        {
            var component = prop as T;
            Destroy(component.gameObject);
        }
    }
}