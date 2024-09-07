using System;
using UnityEngine;

public class Building : MonoBehaviour, IAttackable
{
    [SerializeField] private PlayerHealthSO playerHealthSO;
    [SerializeField] private int startHealth;
    [SerializeField] private bool isFriendly;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material hitMaterial;

    private Material[] hitMaterialSet;
    private Material[] defaultMaterialSet;

    private int currentHealth;

    public Action<IAttackable> OnDied { get; set; }
    public Action<int, int> OnHit { get; set; }

    private void Awake()
    {
        defaultMaterialSet = meshRenderer.materials;
        hitMaterialSet = new Material[2] { meshRenderer.materials[0], hitMaterial };
    }

    private void Start()
    {
        currentHealth = startHealth;
    }

    public bool IsEnemy()
    {
        return !isFriendly;
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
            playerHealthSO.TakeDamage();
            gameObject.SetActive(false);
        }
    }

    private void MaterialChanger()
    {
        meshRenderer.materials = defaultMaterialSet;
    }
}