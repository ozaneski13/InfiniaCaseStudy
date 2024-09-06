using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCurrencyInventory", menuName = "Scriptable Objects/PlayerCurrencyInventory", order = 3)]
public class PlayerCurrencyInventory : ScriptableObject
{
    [SerializeField] private int maxCurrency;
    public int MaxCurrency => maxCurrency;

    [SerializeField] private int currency;
    public int Currency => currency;

    [SerializeField] private int passiveCurrencyGain;
    public int PassiveCurrencyGain => passiveCurrencyGain;

    [SerializeField] private float currencyGainInterval;
    public float CurrencyGainInterval => currencyGainInterval;

    public Action OnCurrencyChanged;

    public void Gain(int amount)
    {
        if (currency + amount > maxCurrency)
            currency = maxCurrency;

        else
            currency += amount;

        OnCurrencyChanged?.Invoke();
    }
    
    public bool CanAfford(int amount) => currency >= amount;

    public void Spent(int amount) 
    { 
        currency -= amount;
        OnCurrencyChanged?.Invoke();
    }
}