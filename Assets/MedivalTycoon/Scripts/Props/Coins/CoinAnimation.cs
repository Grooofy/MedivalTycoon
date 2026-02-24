using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimation 
{
    private Animator _animator;

    public CoinAnimation(Animator animator)
    {
        _animator = animator;
    }    

    public void Reset()
    {
        AnimatorExtensions.Set(_animator, AnimatorParameters.ResetCoin);
    }
}
