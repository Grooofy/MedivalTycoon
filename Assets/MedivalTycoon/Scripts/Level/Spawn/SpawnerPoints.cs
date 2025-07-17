using Characters;
using UnityEngine;

public class SpawnerPoints
{
    private Transform _parent;
    private int _spawnCount = 100;
    private float _spacing = 2f;

    private int _remainingToSpawn;

    public void Initialize(int spawnCount , float spacing, Transform parent)
    {
        _spawnCount = spawnCount;
        _spacing = spacing;
        _parent = parent;
        _remainingToSpawn = _spawnCount;
    }

    public void SpawnObjectsInCube(Vector3 spaceSize)
    {
        int objectsPerAxis = Mathf.CeilToInt(Mathf.Pow(_spawnCount, 1f / 3f));
        Vector3 startOffset = -spaceSize / 2f + Vector3.one * (_spacing / 2f); 

        for (int x = 0; x < objectsPerAxis; x++)
        {
            for (int y = 0; y < objectsPerAxis; y++)
            {
                for (int z = 0; z < objectsPerAxis; z++)
                {
                    if (_remainingToSpawn <= 0)
                        return;

                    Vector3 offset = new Vector3(x * _spacing, y * _spacing, z * _spacing);
                    Vector3 localPos = startOffset + offset;
                    
                    if (Mathf.Abs(localPos.x) > spaceSize.x / 2f ||
                        Mathf.Abs(localPos.y) > spaceSize.y / 2f ||
                        Mathf.Abs(localPos.z) > spaceSize.z / 2f)
                        continue;

                    var point = ObjectFactory.CreateObjectWithComponent<Point>($"Point {x},{y},{z}");
                    point.transform.parent = _parent;
                    point.transform.localPosition = localPos;

                    _remainingToSpawn--;
                }
            }
        }

        if (_remainingToSpawn > 0)
        {
            Debug.LogWarning($"Не все объекты заспавнены ({_remainingToSpawn} остались). Увеличь размеры области или уменьшай spacing.");
        }
    }
}


