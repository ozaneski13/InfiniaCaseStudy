using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthSO", menuName = "Scriptable Objects/PlayerHealthSO", order = 4)]
public class PlayerHealthSO : ScriptableObject
{
    [SerializeField] private int health = 3;
    public int Health => health;

    [SerializeField] private VoidEventSO loseEventSO;

    public void TakeDamage()
    {
        health--;

        if (health == 0)
            loseEventSO.FireEvent();
    }
}