using System;
using UnityEngine;

public class Building : MonoBehaviour, IAttackable
{
    [SerializeField] private PlayerHealthSO playerHealthSO;
    [SerializeField] private int startHealth;
    [SerializeField] private bool isFriendly;

    private int currentHealth;

    public Action<IAttackable> OnDied { get; set; }
    public Action<int, int> OnHit { get; set; }

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

        if (currentHealth <= 0)
            playerHealthSO.TakeDamage();
    }
}