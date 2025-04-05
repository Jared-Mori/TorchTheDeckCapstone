using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    public IEnumerator PlayAnimationAndWait(string animationName)
    {
        // Trigger the animation
        animator.Play(animationName);

        // Wait until the animation has finished
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        Debug.Log($"Animation '{animationName}' finished!");
    }

    public void TriggerAnimation(string animationName)
    {
        if (animator != null)
        {
            StartCoroutine(PlayAnimationAndWait(animationName));
        }
        else
        {
            Debug.LogWarning("Animator component is not assigned!");
        }
    }
}