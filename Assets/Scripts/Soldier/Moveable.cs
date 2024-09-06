using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Moveable : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float followInterval = 0.5f;

    public float nav = 10f;

    private IEnumerator followRoutine;

    private Vector3 result;

    public Action OnFollowStoped;

    protected void Follow(Transform target, float range)
    {
        if (followRoutine != null)
            StopCoroutine(followRoutine);
        
        followRoutine = FollowRoutine(target, range);
        StartCoroutine(followRoutine);
    }

    private IEnumerator FollowRoutine(Transform target, float range)
    {
        NavMeshHit hit;
        Vector3 destination = target.position;
        
        while(Vector3.Distance(transform.position, destination) >= range)
        {
            if (NavMesh.SamplePosition(target.position, out hit, nav, NavMesh.AllAreas))
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

        OnFollowStoped?.Invoke();
    }

    private void Move(Vector3 target)
    {
        agent.destination = target;
    }

    protected void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }
}