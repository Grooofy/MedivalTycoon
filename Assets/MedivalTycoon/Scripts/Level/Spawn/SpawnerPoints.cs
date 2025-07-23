using System.Collections.Generic;
using Characters;
using UnityEngine;

public class SpawnerPoints: MonoBehaviour
{
    private Transform _parent;
    private int _spawnCount;
    private float _spacing;
    private Vector3 _spaceSize;
    private int _remainingToSpawn;

    public void Initialize(int spawnCount , float spacing, Transform parent)
    {
        _spawnCount = spawnCount;
        _spacing = spacing;
        _parent = parent;
        _remainingToSpawn = _spawnCount;
    }

    public List<Point> SpawnObjectsInCube(Vector3 spaceSize)
    {
        _spaceSize = spaceSize;
        var createdPoints = new List<Point>();

        int objectsPerAxis = Mathf.CeilToInt(Mathf.Pow(_spawnCount, 1f / 3f));
        Vector3 startOffset = -spaceSize / 2f + Vector3.one * (_spacing / 2f);

        for (int x = 0; x < objectsPerAxis; x++)
        {
            for (int y = 0; y < objectsPerAxis; y++)
            {
                for (int z = 0; z < objectsPerAxis; z++)
                {
                    if (_remainingToSpawn <= 0)
                        return createdPoints;

                    Vector3 offset = new Vector3(x * _spacing, y * _spacing, z * _spacing);
                    Vector3 localPos = startOffset + offset;

                    if (Mathf.Abs(localPos.x) > spaceSize.x / 2f ||
                        Mathf.Abs(localPos.y) > spaceSize.y / 2f ||
                        Mathf.Abs(localPos.z) > spaceSize.z / 2f)
                        continue;

                    var point = ObjectFactory.CreateObjectWithComponent<Point>($"Point {x},{y},{z}");
                    point.transform.parent = _parent;
                    point.transform.localPosition = localPos;

                    createdPoints.Add(point);
                    _remainingToSpawn--;
                }
            }
        }

        if (_remainingToSpawn > 0)
        {
            Debug.LogWarning($"Не все объекты заспавнены ({_remainingToSpawn} остались). Увеличь размеры области или уменьшай spacing.");
        }

        return createdPoints;
    }
    
    private void OnDrawGizmos()
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
    }
}


