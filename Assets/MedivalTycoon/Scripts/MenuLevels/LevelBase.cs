using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    public int LevelsCount => _levels.Count;

    public bool TutorIsComlete()
    {
        return _levels[0].IsComplete; 
    }

    public Level GetTutorLevelData()
    {
        return _levels[0];
    }

    public Level GetLevelData(int id)
    {
        return _levels[id];
    }

    public bool IsComplete(int id)
    {
        return _levels[id].IsComplete;
    }
}
