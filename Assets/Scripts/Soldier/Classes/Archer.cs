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

    protected override IEnumerator AttackRoutine(float interval)
    {
        closestEnemy.OnDied += StopAttackRoutine;

        while (true)
        {
            GameObject projectile = projectilePool.FirstOrDefault(x => !x.activeInHierarchy);
            projectile.transform.position = projectilePoint.position;
            projectile.SetActive(true);

            projectile.transform.DOMove(closestEnemy.GetTransform().position, interval);

            yield return new WaitForSeconds(interval);
            closestEnemy.GetHit(damage);
        }
    }

    protected override void StopAttackRoutine(IAttackable attackable)
    {
        base.StopAttackRoutine(attackable);

        projectilePool.ForEach(x => x.SetActive(false));
    }
}