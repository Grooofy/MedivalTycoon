using UnityEngine;

public static class AnimatorExtensions
{
    public static void Set(this Animator animator, AnimatorParameters param, bool value)
    {
        animator.SetBool(param.ToString(), value);
    }

    public static void Set(this Animator animator, AnimatorParameters param)
    {
        Debug.Log(param.ToString()+"!!!!!!!!!!!!");
        animator.SetTrigger(param.ToString());
    }

    public static void Play(this Animator animator, AnimatorParameters param)
    {
        animator.Play(param.ToString());
    }
}