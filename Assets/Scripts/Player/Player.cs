using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCurrencyInventory inventory;

    private IEnumerator currencyRoutine;

    private void Start()
    {
        currencyRoutine = CurrencyRoutine();
        StartCoroutine(currencyRoutine);
    }

    private IEnumerator CurrencyRoutine()
    {
        yield return new WaitForSeconds(inventory.CurrencyGainInterval);
        inventory.Gain(inventory.PassiveCurrencyGain);

        currencyRoutine = CurrencyRoutine();
        StartCoroutine(currencyRoutine);
    }

    private void OnDestroy()
    {
        StopCoroutine(currencyRoutine);
    }
}