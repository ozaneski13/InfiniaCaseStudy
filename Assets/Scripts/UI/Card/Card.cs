using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card : Draggable
{
    [SerializeField] protected CardTypeSO cardTypeSO;
    [SerializeField] private DeckLockSO deckLockSO;
    [SerializeField] private PlayerCurrencyInventory playerCurrencyInventory;

    [SerializeField] private Image cardImg;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private LayerMask layerMask;

    protected Settings settings;

    public Action<Card> OnCardUsed;

    public abstract void Use(Vector3 pos);

    public void ActivateVisual()
    {
        isDraggable = true;
        cardImg.sprite = settings.Sprite;

        costText.gameObject.SetActive(true);
        costText.text = settings.Cost.ToString() + " Orb";
    }

    public abstract void Init(ECardType cardType);

    public override bool OnDragFinished(PointerEventData eventData)
    {
        if (deckLockSO.IsLocked)
            return false;

        if (!playerCurrencyInventory.CanAfford(settings.Cost))
        {
            isDraggable = true;

            return false;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        { 
            Use(hit.point);

            playerCurrencyInventory.Spent(settings.Cost);

            return true;
        }

        return false;
    }
}