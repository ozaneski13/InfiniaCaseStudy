using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private string currentState = "";

    public void ChangeState(string state)
    {
        if (!gameObject.activeInHierarchy)
            return;

        animator.StopPlayback();
        currentState = state;
        animator.CrossFade(currentState, 0.1f);
    }
}