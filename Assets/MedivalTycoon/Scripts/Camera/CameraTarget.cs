using System;
using Characters;
using UnityEngine.Events;

public class CameraTarget : IDisposable
{
    private SwitcherSelectedCharacter _switcher;
    public UnityAction<int> TargetReady;

    public void Initilize(SwitcherSelectedCharacter switcher)
    {
        _switcher = switcher;
        _switcher.Activate += TargetReady;
    }

    public void Dispose()
    {
        _switcher.Activate -= TargetReady;
    }
}