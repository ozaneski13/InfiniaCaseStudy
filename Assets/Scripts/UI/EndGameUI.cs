using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private VoidEventSO winEventSO;
    [SerializeField] private VoidEventSO loseEventSO;

    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;

    [SerializeField] private GameObject panel;
    [SerializeField] private float animDuration = 0.6f;

    [SerializeField] private SFXSO sfxSO;
    [SerializeField] private AudioSource source;

    private bool isToggled = false;

    private Vector3 panelDefaultPos;

    private void Start()
    {
        panelDefaultPos = panel.transform.position;

        winEventSO.Subscribe(()=>
        {
            winText.SetActive(true);
            TogglePanel();
            source.PlayOneShot(sfxSO.GetSFXSettingsByCardType(ESFXType.BattleWin).Clip);
        });

        loseEventSO.Subscribe(()=> 
        {
            loseText.SetActive(true);
            TogglePanel();
            source.PlayOneShot(sfxSO.GetSFXSettingsByCardType(ESFXType.BattleLost).Clip);
        });
    }

    private void TogglePanel()
    {
        isToggled = !isToggled;

        if (isToggled)
        {
            panel.SetActive(isToggled);
            panel.transform.DOMoveY(transform.position.y, animDuration).OnComplete(() => OnToggleComplete(isToggled));
        }

        else
        {
            panel.transform.DOMoveY(panelDefaultPos.y, animDuration).OnComplete(() => OnToggleComplete(isToggled));
        }
    }

    private void OnToggleComplete(bool isToggled)
    {
        if (!isToggled)
        {
            panel.SetActive(isToggled);
            Time.timeScale = 1f;
        }

        else
            Time.timeScale = 0f;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        winEventSO.Unsubscribe(() =>
        {
            winText.SetActive(true);
            TogglePanel();
        });

        loseEventSO.Unsubscribe(() =>
        {
            loseText.SetActive(true);
            TogglePanel();
        });
    }
}