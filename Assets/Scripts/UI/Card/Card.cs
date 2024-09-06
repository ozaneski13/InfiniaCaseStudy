using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Card : Draggable
{
    [SerializeField] protected CardTypeSO cardTypeSO;
    [SerializeField] private DeckLockSO deckLockSO;
    [SerializeField] private PlayerCurrencyInventory playerCurrencyInventory;

    [SerializeField] private Image cardImg;

    [SerializeField] private LayerMask layerMask;

    protected Settings settings;

    public Action<Card> OnCardUsed;

    public abstract void Use(Vector3 pos);

    public void ActivateVisual()
    {
        cardImg.sprite = settings.Sprite;
    }

    public abstract void Init(ECardType cardType);

    public override bool OnDragFinished(PointerEventData eventData)
    {
        if (deckLockSO.IsLocked)
            return false;

        if (!playerCurrencyInventory.CanAfford(settings.Cost))
        {
            isDraggable = true;
            SetDefaultParent();

            return false;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        { 
            Use(hit.point);

            playerCurrencyInventory.Spent(settings.Cost);

            OnCardUsed?.Invoke(this);

            if (this is MagicCard)
                return MagicPoolController.Instance.RefillPool((settings as MagicSettings).Type, gameObject);
            else
                return SpawnPoolController.Instance.RefillPool((settings as SpawnSettings).Type, gameObject);
        }

        return false;
    }
}