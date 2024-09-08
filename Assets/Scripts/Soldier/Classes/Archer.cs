using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Archer : Soldier
{
    [SerializeField] private Transform projectilePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform poolParent;
    [SerializeField] private int poolSize;

    private List<GameObject> projectilePool;

    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }

    private void InitPool()
    {
        projectilePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, poolParent);
            projectile.SetActive(false);

            projectilePool.Add(projectile);
        }
    }

    protected override IEnumerator AttackRoutine(float interval, IAttackable attackable)
    {
        GameObject projectile = projectilePool.FirstOrDefault(x => !x.activeInHierarchy);
        projectile.transform.position = projectilePoint.position;
        projectile.SetActive(true);

        projectile.transform.DOMove(attackable.GetTransform().position, interval);

        yield return new WaitForSeconds(interval);
        attackable.GetHit(damage);

        if (!attackable.GetTransform().gameObject.activeInHierarchy)
            yield break;

        attackRoutine = AttackRoutine(interval, attackable);
        StartCoroutine(attackRoutine);
    }

    protected override void StopAttackRoutine(IAttackable attackable)
    {
        attackable.OnDied -= StopAttackRoutine;
        StopCoroutine(AttackRoutine(interval, attackable));
        CheckClosest();

        projectilePool.ForEach(x => x.SetActive(false));
    }
}