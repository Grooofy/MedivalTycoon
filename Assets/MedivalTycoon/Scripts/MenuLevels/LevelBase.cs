using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBase : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    public int LevelsCount => _levels.Count;

    public bool TutorIsComlete()
    {
        return IsComplete(0);
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
        return _levels[id].IsComplete || LevelRewards.GetBest(_levels[id].NumberLevel) > 0;
    }

    public bool CanStart(int id)
    {
        return id >= 0 && id < _levels.Count && !IsComplete(id)
            && (id == 0 || IsComplete(id - 1));
    }
}
