using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : Draggable
{
    [SerializeField] protected CardTypeSO cardTypeSO;
    [SerializeField] private PlayerCurrencyInventory playerCurrencyInventory;

    [SerializeField] private LayerMask layerMask;
    public Action<Card> OnCardUsed;

    private int cost;

    public abstract void Use(Vector3 pos);

    public void ActivateVisual()
    {

    }

    public abstract void Init(ECardType cardType);

    public override bool OnDragFinished(PointerEventData eventData)
    {
        if (!playerCurrencyInventory.CanAfford(cost))
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

            playerCurrencyInventory.Spent(cost);

            OnCardUsed?.Invoke(this);

            return true;
        }

        return false;
    }
}