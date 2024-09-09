using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthSO", menuName = "Scriptable Objects/PlayerHealthSO", order = 4)]
public class PlayerHealthSO : ScriptableObject
{
    [SerializeField] private int health = 3;

    [SerializeField] private VoidEventSO winEventSO;
    [SerializeField] private VoidEventSO loseEventSO;

    [SerializeField] private bool isFriendly;

    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = health;
    }

    public void TakeDamage()
    {
        currentHealth--;

        if (currentHealth == 0)
        {
            if (isFriendly)
                loseEventSO.FireEvent();
            else
                winEventSO.FireEvent();
        }
    }

    public void TakeFullDamage()
    {
        currentHealth = -1;

        if (isFriendly)
            loseEventSO.FireEvent();
        else
            winEventSO.FireEvent();
    }
}