using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Pool;

namespace Propses
{
    public class BarrelPool : MonoBehaviour
    {
        [SerializeField] private Barrel _barrelPrefab;
        [SerializeField] private float _moveSpeed;

        private ObjectPool<Barrel> _pool;
        
        public Barrel SpawnBarrel()
        {
            return _pool.Get();
        }

        public void DespawnBarrel(Barrel barrel)
        {
            _pool.Release(barrel);
        }
        
        public BarrelPool Initialize( int defaultSize, int maxSize)
        {
            _pool = new ObjectPool<Barrel>(
                CreateBarrel,
                OnGetBarrel,
                OnReleaseBarrel,
                OnDestroyBarrel,
                collectionCheck: false,
                defaultCapacity: defaultSize,
                maxSize: maxSize
            );
            return this;
        }

        private Barrel CreateBarrel()
        {
            var barrel = Instantiate(_barrelPrefab, transform);
            barrel.gameObject.SetActive(false);
            return barrel;
        }

        private void OnGetBarrel(Barrel barrel)
        {
            barrel.gameObject.SetActive(true);
            barrel.Initilization(transform, _moveSpeed, barrel.gameObject.GetComponent<Animator>());
        }

        private void OnReleaseBarrel(Barrel barrel)
        {
            barrel.gameObject.SetActive(false);
        }

        private void OnDestroyBarrel(Barrel barrel)
        {
            Destroy(barrel.gameObject);
        }
    }

    
}