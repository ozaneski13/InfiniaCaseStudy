using UnityEngine;

public class Barrack : MonoBehaviour
{
    [SerializeField] private PlayerHealthSO playerHealthSO;

    [SerializeField] private int startHealth;

    private int currentHealth;

    private void Start()
    {
        currentHealth = startHealth;
    }

    public void OnHit(int amount)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            playerHealthSO.TakeDamage();
    }
}