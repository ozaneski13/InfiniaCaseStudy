using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Soldier : Moveable, IAttackable
{
    [SerializeField] private SpawnSO spawnSO;
    [SerializeField] private ESpawnType type;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material enemyMaterial;
    [SerializeField] private Material friendlyMaterial;
    [SerializeField] private Material hitMaterial;

    [SerializeField] private VisualEffect hitEffect;

    private List<IAttackable> enemiesInRange = new List<IAttackable>();

    protected IAttackable closestEnemy;

    private IEnumerator attackRoutine;

    private Material[] hitMaterialSet;
    private Material[] defaultMaterialSet;

    private int totalHealth;
    protected int damage;
    protected int range;
    protected float interval;

    private int currentHealth;

    private bool isFriendly = false;

    public Action<IAttackable> OnDied { get; set; }
    public Action<int, int> OnHit { get; set; }

    protected virtual void Awake()
    {
        SpawnSettings settings = spawnSO.GetSpawnSettingsByType(type);

        totalHealth = settings.Health;
        currentHealth = totalHealth;
        damage = settings.Damage;
        range = settings.Range;
        interval = settings.AttackInterval;

        OnFollowStoped += CheckAttack;
    }

    public void SetAffiliate(bool isFriendly)
    {
        this.isFriendly = isFriendly;
        Material[] materials = meshRenderer.materials;

        if (isFriendly)
            materials[0] = friendlyMaterial;
        else
            materials[0] = enemyMaterial;

        meshRenderer.materials = materials;

        defaultMaterialSet = meshRenderer.materials;
        hitMaterialSet = new Material[2] { meshRenderer.materials[0], hitMaterial };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Attackable"))
            return;

        IAttackable attackable = other.GetComponentInParent<IAttackable>();

        if (attackable.IsFriendly() == isFriendly)
            return;

        if (!enemiesInRange.Contains(attackable))
        {
            enemiesInRange.Add(attackable);
            attackable.OnDied += RemoveAttackableFromList;
        }

        CheckClosest();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Attackable"))
            return;

        IAttackable attackable = other.GetComponentInParent<IAttackable>();

        if (attackable.IsFriendly() == isFriendly)
            return;

        if (enemiesInRange.Contains(attackable))
            enemiesInRange.Remove(attackable);

        CheckClosest();
    }

    private void CheckClosest()
    {
        if (enemiesInRange.Count == 0)
            return;

        IAttackable closestTemp = null;
        float distance = Mathf.Infinity;

        foreach (IAttackable attackable in enemiesInRange)
        {
            if (Vector3.Distance(transform.position, attackable.GetTransform().position) < distance)
            {
                closestTemp = attackable;
                distance = Vector3.Distance(transform.position, attackable.GetTransform().position);
            }
        }

        if (closestTemp != closestEnemy)
        {
            closestEnemy = closestTemp;
            Follow(closestEnemy.GetTransform(), range);
        }
    }

    private void CheckAttack()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = AttackRoutine(interval);
        StartCoroutine(attackRoutine);
    }

    protected virtual IEnumerator AttackRoutine(float interval)
    {
        closestEnemy.OnDied += StopAttackRoutine;

        while (true)
        {
            animationController.ChangeState("Attack");
            yield return new WaitForSeconds(interval);
            closestEnemy.GetHit(damage);
        }
    }

    protected virtual void StopAttackRoutine(IAttackable attackable)
    {
        closestEnemy.OnDied -= StopAttackRoutine;
        StopCoroutine(attackRoutine);
        CheckClosest();
    }

    private void RemoveAttackableFromList(IAttackable attackable)
    {
        if (enemiesInRange.Contains(attackable))
        {
            attackable.OnDied -= RemoveAttackableFromList;
            enemiesInRange.Remove(attackable);
            CheckClosest();
        }
    }

    public bool IsFriendly()
    {
        return isFriendly;
    }

    public void GetHit(int damage)
    {
        currentHealth -= damage;
        OnHit?.Invoke(currentHealth, totalHealth);

        meshRenderer.materials = hitMaterialSet;

        Invoke("MaterialChanger", 0.1f);

        hitEffect.Play();

        if (currentHealth <= 0)
        {
            OnDied?.Invoke(this);

            if (isFriendly)
                SpawnPoolController.Instance.RefillPool(type, gameObject);
            else
                gameObject.SetActive(false);
        }
    }

    private void MaterialChanger()
    {
        meshRenderer.materials = defaultMaterialSet;
    }

    private void OnDestroy()
    {
        OnFollowStoped -= CheckAttack;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}