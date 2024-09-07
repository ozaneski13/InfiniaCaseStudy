using System.Collections;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private BoxCollider colliderArea;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float damageInterval;

    private IEnumerator effectRoutine;

    public void StartEffect(int damage)
    {
        effectRoutine = EffectRoutine(damage);
        StartCoroutine(effectRoutine);
    }

    private IEnumerator EffectRoutine(int damage)
    {
        while (true)
        {
            Collider[] hitColliders = Physics.OverlapBox(transform.position,
                new Vector3(transform.localScale.x * colliderArea.size.x, transform.localScale.y * colliderArea.size.y, transform.localScale.z * colliderArea.size.z) / 2,
                Quaternion.identity,
                layerMask);

            yield return new WaitForSeconds(damageInterval);

            foreach (Collider collider in hitColliders)
            {
                collider.GetComponent<IAttackable>().GetHit(damage);
            }
        }
    }

    public void StopEffect()
    {
        StopCoroutine(effectRoutine);
    }
}