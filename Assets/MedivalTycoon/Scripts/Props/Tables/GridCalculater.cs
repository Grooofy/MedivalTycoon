using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tables
{
    public static class GridCalculator
    {
        public static List<Vector3> GetGridPositions(Vector3 origin, Vector2 areaSize, float spacing, Vector3 offset)
        {
            var result = new List<Vector3>();
            int countX = Mathf.FloorToInt(areaSize.x / spacing);
            int countZ = Mathf.FloorToInt(areaSize.y / spacing);

            for (int x = 0; x < countX; x++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    var pos = origin + new Vector3(x * spacing, 0f, z * spacing) + offset;
                    result.Add(pos);
                }
            }
            return result;
        }
    }
}