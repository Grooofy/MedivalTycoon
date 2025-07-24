using System.Collections.Generic;
using Characters;
using UnityEngine;

public class SpawnerPoints
{
    private Transform _parent;
    private int _spawnCount;
    private float _spacing;
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
        var createdPoints = new List<Point>();

        // Более точный расчет количества объектов на ось
        int objectsPerAxis = Mathf.CeilToInt(Mathf.Pow(_spawnCount, 1f / 3f));
        Vector3 halfSize = spaceSize / 2f;
        Vector3 startPos = -halfSize + new Vector3(_spacing / 2f, _spacing / 2f, _spacing / 2f);

        for (int x = 0; x < objectsPerAxis; x++)
        {
            for (int y = 0; y < objectsPerAxis; y++)
            {
                for (int z = 0; z < objectsPerAxis; z++)
                {
                    if (_remainingToSpawn <= 0)
                        return createdPoints;

                    Vector3 offset = new Vector3(x * _spacing, y * _spacing, z * _spacing);
                    Vector3 spawnPos = startPos + offset;

                    // Явная проверка границ
                    if (spawnPos.x > halfSize.x || spawnPos.x < -halfSize.x ||
                        spawnPos.y > halfSize.y || spawnPos.y < -halfSize.y ||
                        spawnPos.z > halfSize.z || spawnPos.z < -halfSize.z)
                        continue;

                    var point = ObjectFactory.CreateObjectWithComponent<Point>($"Point {x},{y},{z}");
                    point.transform.SetParent(_parent, false);
                    point.transform.localPosition = spawnPos;

                    createdPoints.Add(point);
                    _remainingToSpawn--;
                }
            }
        }

        if (_remainingToSpawn > 0)
        {
            Debug.LogWarning($"Не удалось разместить {_remainingToSpawn} объектов. " +
                             $"Увеличьте пространство (текущий размер: {spaceSize}) " +
                             $"или уменьшите расстояние между объектами (текущее: {_spacing})");
        }

        return createdPoints;
    }
    
}


