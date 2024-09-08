using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private BoxCollider colliderArea;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float damageInterval;

    private IEnumerator effectRoutine;

    private List<IAttackable> attackables = new List<IAttackable>();

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
            attackables.Add(other.GetComponentInParent<IAttackable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
            attackables.Remove(other.GetComponentInParent<IAttackable>());
    }

    public void StartEffect(EMagicType type, int damage, float duration)
    {
        StartCoroutine(TimerRoutine(type, duration, damage));
    }

    private IEnumerator TimerRoutine(EMagicType type, float duration, int damage)
    {
        effectRoutine = EffectRoutine(damage);
        StartCoroutine(effectRoutine);

        yield return new WaitForSeconds(duration);

        StopCoroutine(effectRoutine);
        MagicPoolController.Instance.RefillPool(type, gameObject);
    }

    private IEnumerator EffectRoutine(int damage)
    {
        yield return new WaitForSeconds(damageInterval);

        foreach (IAttackable attackable in attackables)
        {
            attackable.GetHit(damage);
        }

        effectRoutine = EffectRoutine(damage);
        StartCoroutine(effectRoutine);
    }
}