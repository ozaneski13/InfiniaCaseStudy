using System;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private int cardCount;
    [SerializeField] private Transform movePoint;
    public Transform MovePoint => movePoint;

    private int currentCardCount = 0;

    public int NeededCardCount => cardCount - currentCardCount;

    public Action OnNeedRefill;

    public void Fill(Card card)
    {
        currentCardCount++;
        card.transform.SetParent(transform);
        card.OnCardUsed += RemoveCard;
    }

    private void RemoveCard(Card card)
    {
        currentCardCount--;
        card.OnCardUsed -= RemoveCard;

        OnNeedRefill?.Invoke();
    }
}