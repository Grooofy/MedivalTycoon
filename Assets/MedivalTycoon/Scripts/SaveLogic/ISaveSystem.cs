using System;
using UnityEngine.PlayerLoop;

public interface ISaveSystem
{
    void Save(SaveData data);

    SaveData Load();
}

