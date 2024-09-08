using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private string currentState = "";

    public void ChangeState(string state)
    {
        currentState = state;
        animator.CrossFade(currentState, 0f);
    }
}