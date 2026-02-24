using Beers;
using Lever;
using Propses;
using Beers;
using MedivalTycoon;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barrels
{
    public class BarrelManager : MonoBehaviour
    {
        [Header("Barrel CubeSpawnPointSize")] 
        [SerializeField] private Vector3 _spaceSize;
        [SerializeField] private int _spawnCount;
        [SerializeField] private float _spacing;
        [SerializeField] private BarrelBuffer _barrelBuffer;
        [SerializeField] private LeverInstaller _leverInstaller;
        [SerializeField] private BarrelGiver _barrelGiver;
        [SerializeField] private BarrelTaker _barrelTaker;
        [SerializeField] private BarrelBeerBuffer _barrelBeerBuffer;
        [SerializeField] private PropsSpawner _propsSpawner;
        [SerializeField] private Point _finishPositionToBarrel;
        [SerializeField] private float _delayBarrelReset;
        [SerializeField] private LayerMask _layerMask;

        private IPropsPool _barrelPool;


        public void Initialize()
        {
            _barrelPool = _propsSpawner.GetBarrelPool();
            _barrelBuffer.Initialize("Barrel", _barrelPool);
            _leverInstaller.InitializeBarrelLever(_barrelBuffer);
            _barrelGiver.Initialize(_barrelBuffer, _layerMask);
            _barrelBeerBuffer.Initialize(_barrelPool, _finishPositionToBarrel, _delayBarrelReset);
            _barrelTaker.Initialize(_barrelBeerBuffer, _layerMask);
        }

        public void CreatePoints()
        {
            _barrelBuffer.CreatePoints(_spawnCount, _spacing, _spaceSize);
            _barrelBeerBuffer.CreatePoints(_spawnCount, _spacing, _spaceSize);
        }

        public void CheckHits()
        {
            _leverInstaller.CheakHits();
            _barrelGiver.CheckHits();
            _barrelTaker.CheckHits();
        }


        /*private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, _spaceSize);

            int objectsPerAxis = Mathf.CeilToInt(Mathf.Pow(_spawnCount, 1f / 3f));
            Vector3 startOffset = -_spaceSize / 2f + Vector3.one * (_spacing / 2f);

            for (int x = 0; x < objectsPerAxis; x++)
            {
                for (int y = 0; y < objectsPerAxis; y++)
                {
                    for (int z = 0; z < objectsPerAxis; z++)
                    {
                        Vector3 offset = new Vector3(x * _spacing, y * _spacing, z * _spacing);
                        Vector3 localPos = startOffset + offset;
                        Vector3 worldPos = transform.position + localPos;

                        if (Mathf.Abs(localPos.x) > _spaceSize.x / 2f ||
                            Mathf.Abs(localPos.y) > _spaceSize.y / 2f ||
                            Mathf.Abs(localPos.z) > _spaceSize.z / 2f)
                            continue;

                        Gizmos.DrawSphere(worldPos, 0.15f);
                    }
                }
            }
        }*/
    }
}