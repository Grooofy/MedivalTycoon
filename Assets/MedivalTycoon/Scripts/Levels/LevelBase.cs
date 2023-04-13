using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    public int LevelsCount => _levels.Count;

    public Level GetLevelData(int id)
    {
        return _levels[id];
    }

}
