using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tables
{
    public struct GridCell
    {
        public int x;
        public int z;
    }

    public static class GridCalculator
    {
        public static List<Vector3> GetGridPositions(out List<GridCell> cells, Vector3 origin, Vector2 areaSize, float spacing, Vector3 offset)
        {
            var result = new List<Vector3>();
            cells = new List<GridCell>();
            int countX = Mathf.FloorToInt(areaSize.x / spacing);
            int countZ = Mathf.FloorToInt(areaSize.y / spacing);

            for (int x = 0; x < countX; x++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    var pos = origin + new Vector3(x * spacing, 0f, z * spacing) + offset;
                    result.Add(pos);
                    cells.Add(new GridCell { x = x, z = z });
                }
            }
            return result;
        }
    }
}