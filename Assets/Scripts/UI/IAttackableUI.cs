using UnityEngine;
using UnityEngine.UI;

public class IAttackableUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;

    private IAttackable attackable;

    private void OnEnable()
    {
        slider.value = 1;
        fill.color = gradient.Evaluate(slider.value);

        attackable = GetComponent<IAttackable>();
        attackable.OnHit += UpdateUI;
    }

    private void UpdateUI(int currentHealth, int totalHealth)
    {
        slider.value = (float)currentHealth / totalHealth;
        fill.color = gradient.Evaluate(slider.value);
    }

    private void FixedUpdate()
    {
        slider.transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    private void OnDisable()
    {
        attackable.OnHit -= UpdateUI;
    }
}