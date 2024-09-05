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

    private bool isToggled = false;

    private Vector3 panelDefaultPos;

    private void OnEnable()
    {
        panelDefaultPos = panel.transform.position;
    }

    private void Start()
    {
        winEventSO.Subscribe(()=>
        {
            winText.SetActive(true);
            TogglePanel(); 
        });

        loseEventSO.Subscribe(()=> 
        {
            loseText.SetActive(true);
            TogglePanel(); 
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
}