using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float maxDistanceForSample = 10f;

    [SerializeField] protected AnimationController animationController;

    private IEnumerator followRoutine;

    public Action OnFollowStoped;

    protected void Follow(Transform target, float range)
    {
        if (followRoutine != null)
            StopCoroutine(followRoutine);

        if (!gameObject.activeInHierarchy)
            return;

        followRoutine = FollowRoutine(target, range);
        StartCoroutine(followRoutine);
    }

    private IEnumerator FollowRoutine(Transform target, float range)
    {
        NavMeshHit hit;
        Vector3 destination = target.position;
        Vector3 result;

        animationController.ChangeState("Walk");

        while (Vector3.Distance(transform.position, destination) >= range)
        {
            if (NavMesh.SamplePosition(target.position, out hit, maxDistanceForSample, NavMesh.AllAreas))
                result = hit.position;
            else
                result = transform.position;

            if (destination != result)
            {
                destination = result;
                Move(destination);
            }

            yield return null;
        }

        Stop();
    }

    private void Move(Vector3 target)
    {
        agent.destination = target;
    }

    private void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.SetDestination(transform.position);
        OnFollowStoped?.Invoke();
    }

    private void OnDisable()
    {
        if(followRoutine  != null)
            StopCoroutine(followRoutine);
    }
}