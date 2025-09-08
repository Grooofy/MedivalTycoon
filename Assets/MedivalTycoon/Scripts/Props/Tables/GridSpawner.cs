using UnityEngine;
using System.Collections.Generic;
using Beers;
using UnityEngine.Serialization;

namespace Tables
{
    public class GridSpawner : MonoBehaviour
    {
        private Vector2 areaSize = new Vector2(2f, 3f);
        private Vector3 _prefabOffset = new Vector3(0.25f, 0.02f, 0.25f);
        private float spacing = 1f;
        private readonly List<ViewTable> _viewTables = new List<ViewTable>();
        private readonly List<Table> _tables = new List<Table>();
        private readonly List<BeerTaker> _beerTakers = new List<BeerTaker>();
        private TableTrigger _triggerZonePrefab;
        private Table _tablePrefab;
        private int _objectsToSpawn;
        private Vector3 _origin;


        public void Initialize(Table table, TableTrigger prefabTableZone, int objectsToSpawn)
        {
            _tablePrefab = table;
            _triggerZonePrefab = prefabTableZone;
            _objectsToSpawn = objectsToSpawn;
            _origin = transform.position;
        }

        public void SpawnGrid(ConstructionHandler handler, int[] startPrice)
        {
            if (_triggerZonePrefab == null) return;

            int countX = Mathf.FloorToInt(areaSize.x / spacing);
            int countZ = Mathf.FloorToInt(areaSize.y / spacing);
            int totalCells = countX * countZ;

            if (totalCells == 0) return;

            List<Vector3> gridPositions = new List<Vector3>();

            for (int x = 0; x < countX; x++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    Vector3 basePos = _origin + new Vector3(x * spacing, 0f, z * spacing);
                    Vector3 correctedPos = basePos + _prefabOffset;
                    gridPositions.Add(correctedPos);
                }
            }

            for (int i = gridPositions.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (gridPositions[i], gridPositions[j]) = (gridPositions[j], gridPositions[i]);
            }

            int spawnCount = Mathf.Min(_objectsToSpawn, gridPositions.Count);

            for (int i = 0; i < spawnCount; i++)
            {
                var createObject = Instantiate(_triggerZonePrefab, gridPositions[i], Quaternion.identity, transform);
                var viewTable = createObject.Initialize(handler);
                var tableBuilderAnimation = createObject.GetComponent<TableBuilderAnimation>();
                var table = createObject.CreateTable(_tablePrefab);
                tableBuilderAnimation.Initialize();
                table.Initialize(startPrice[i]);
                table.InitializeBeerTaker();
                viewTable.Initialize(table, tableBuilderAnimation);
                _tables.Add(table);
                _viewTables.Add(viewTable);
            }
        }

        public void CheckHits()
        {
            foreach (var beerTaker in _tables)
            {
                beerTaker.CheckHits();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                _origin + new Vector3(areaSize.x / 2, 0, areaSize.y / 2),
                new Vector3(areaSize.x, 0.1f, areaSize.y)
            );
        }
    }
}