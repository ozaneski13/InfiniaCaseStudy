using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCurrencyInventory", menuName = "Scriptable Objects/PlayerCurrencyInventory", order = 3)]
public class PlayerCurrencyInventory : ScriptableObject
{
    [SerializeField] private int currency;
    public int Currency => currency;

    public void Gain(int amount) => currency += amount;

    public bool CanAfford(int amount) => currency >= amount;

    public void Spent(int amount) => currency -= amount;
}