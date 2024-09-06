using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrencyUI : MonoBehaviour
{
    [SerializeField] private PlayerCurrencyInventory inventory;

    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Slider slider;
    [SerializeField] private Image background;
    [SerializeField] private Gradient sliderGradient;

    private void Awake()
    {
        inventory.OnCurrencyChanged += UpdateUI;
    }

    private void UpdateUI()
    {
        slider.value = (float)inventory.Currency / inventory.MaxCurrency;
        background.color = sliderGradient.Evaluate(slider.value);
        currencyText.text = inventory.Currency.ToString() + " / " + inventory.MaxCurrency.ToString();
    }

    private void OnDestroy()
    {
        inventory.OnCurrencyChanged -= UpdateUI;
    }
}