using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthSO", menuName = "Scriptable Objects/PlayerHealthSO", order = 4)]
public class PlayerHealthSO : ScriptableObject
{
    [SerializeField] private int health = 3;

    [SerializeField] private VoidEventSO loseEventSO;

    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = health;
    }

    public void TakeDamage()
    {
        currentHealth--;

        if (currentHealth == 0)
            loseEventSO.FireEvent();
    }

    public void TakeFullDamage()
    {
        currentHealth = -1;
        
        loseEventSO.FireEvent();
    }
}