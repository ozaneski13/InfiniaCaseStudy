using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Building : MonoBehaviour, IAttackable
{
    [SerializeField] private PlayerHealthSO playerHealthSO;
    [SerializeField] private int startHealth;
    [SerializeField] private bool isFriendly;
    [SerializeField] private bool isMainBuilding;

    [SerializeField] private LayerMask hittableLayerMask;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material hitMaterial;

    [SerializeField] private Transform projectilePoint;
    [SerializeField] private int damage;
    [SerializeField] private float interval;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform poolParent;
    [SerializeField] private int poolSize;

    private List<GameObject> projectilePool;

    private Material[] hitMaterialSet;
    private Material[] defaultMaterialSet;

    private List<IAttackable> attackables = new List<IAttackable>();
    private IAttackable currentAttackable;

    private IEnumerator attackRoutine;

    private int currentHealth;

    public Action<IAttackable> OnDied { get; set; }
    public Action<int, int> OnHit { get; set; }

    private void Awake()
    {
        defaultMaterialSet = meshRenderer.materials;
        hitMaterialSet = new Material[2] { meshRenderer.materials[0], hitMaterial };

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

    private void Start()
    {
        currentHealth = startHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((hittableLayerMask & (1 << other.gameObject.layer)) != 0)
        {
            IAttackable attackable = other.GetComponentInParent<IAttackable>();

            if (attackable.IsFriendly() == isFriendly)
                return;

            attackables.Add(attackable);
            attackable.OnDied += RemoveAttackable;
            
            CheckAttackables();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((hittableLayerMask & (1 << other.gameObject.layer)) != 0)
        { 
            IAttackable attackable = other.GetComponentInParent<IAttackable>();

            if (attackable.IsFriendly() == isFriendly)
                return;

            attackable.OnDied -= RemoveAttackable;
            attackables.Remove(attackable);

            if (attackable == currentAttackable)
                CheckAttackables();
        }
    }

    private void RemoveAttackable(IAttackable attackable)
    {
        if (attackables.Contains(attackable))
            attackables.Remove(attackable);

        if (attackable == currentAttackable)
            CheckAttackables();
    }

    private void CheckAttackables()
    {
        if (attackables.Count == 0)
            return;

        currentAttackable = attackables[0];

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = AttackRoutine(interval);

        if (!gameObject.activeInHierarchy)
            return;

        StartCoroutine(attackRoutine);
    }

    private IEnumerator AttackRoutine(float interval)
    {
        currentAttackable.OnDied += StopAttackRoutine;

        while (true)
        {
            GameObject projectile = projectilePool.FirstOrDefault(x => !x.activeInHierarchy);
            projectile.transform.position = projectilePoint.position;
            projectile.SetActive(true);

            projectile.transform.DOMove(currentAttackable.GetTransform().position, interval);

            yield return new WaitForSeconds(interval);

            currentAttackable.GetHit(damage);
        }
    }

    private void StopAttackRoutine(IAttackable attackable)
    {
        currentAttackable.OnDied -= StopAttackRoutine;
        StopCoroutine(attackRoutine);

        projectilePool.ForEach(x => x.SetActive(false));

        CheckAttackables();
    }

    public bool IsFriendly()
    {
        return isFriendly;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void GetHit(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        OnHit?.Invoke(currentHealth, startHealth);

        meshRenderer.materials = hitMaterialSet;

        Invoke("MaterialChanger", 0.1f);

        if (currentHealth <= 0)
        {
            OnDied?.Invoke(this);

            if (isFriendly)
            {
                if (isMainBuilding)
                    playerHealthSO.TakeFullDamage();
                else
                    playerHealthSO.TakeDamage();
            }
                
            gameObject.SetActive(false);
        }
    }

    private void MaterialChanger()
    {
        meshRenderer.materials = defaultMaterialSet;
    }
}