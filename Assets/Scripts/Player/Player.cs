using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCurrencyInventory inventory;

    private void Start()
    {
        StartCoroutine(CurrencyRoutine());
    }

    private IEnumerator CurrencyRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(inventory.CurrencyGainInterval);
            inventory.Gain(inventory.PassiveCurrencyGain);
        }
    }
}