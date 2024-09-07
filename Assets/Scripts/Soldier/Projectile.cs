using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float despawnThreshold;

    private IEnumerator despawnRoutine;

    private void OnEnable()
    {
        despawnRoutine = DespawnRoutine();
        StartCoroutine(despawnRoutine);
    }

    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(despawnThreshold);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Attackable"))
            return;

        if(despawnRoutine != null)
            StopCoroutine(despawnRoutine);

        gameObject.SetActive(false);
    }
}