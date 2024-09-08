using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DeckController : MonoBehaviour
{
    [SerializeField] private CardTypeSO cardTypeSO;
    [SerializeField] private DeckLockSO deckLockSO;

    [SerializeField] private Transform cardHolder;
    [SerializeField] private int deckSize;

    [SerializeField] private HandController handController;

    private List<Card> deck;

    private void Awake()
    {
        InitDeck();

        handController.OnNeedRefill += FillHand;
    }

    private void InitDeck()
    {
        deck = new List<Card>();
        GameObject goToCreate;

        for (int i = 0; i < deckSize; i++)
        {
            ECardType cardType = (ECardType)Enum.ToObject(typeof(ECardType), UnityEngine.Random.Range(0, Enum.GetValues(typeof(ECardType)).Length));

            goToCreate = cardTypeSO.GetCardSettingsByCardType(cardType).CardPrefab;

            GameObject cardGO = Instantiate(goToCreate, cardHolder);
            Card card = cardGO.GetComponent<Card>();

            card.Init(cardType);

            cardGO.SetActive(false);
            deck.Add(card);
        }

        deck = ShuffleList(deck);
    }

    public List<Card> ShuffleList(List<Card> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            Card temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;
    }

    private void Start()
    {
        FillHand();
    }

    private void FillHand()
    {
        deckLockSO.SetLock(true);

        int neededCount = handController.NeededCardCount;

        if (neededCount == 0)
            return;

        StartCoroutine(FillRoutine(neededCount));
    }

    private IEnumerator FillRoutine(int neededCount)
    {
        deck = ShuffleList(deck);

        for (int i = 0; i < neededCount; i++)
        {
            Card card = deck[0];
            card.gameObject.SetActive(true);
            card.ActivateVisual();
            card.OnCardUsed += RePoolCard;

            deck.Remove(card);

            card.transform.DOMove(handController.MovePoint.position, 2f);
            yield return new WaitForSeconds(3f);

            handController.Fill(card);
        }

        deckLockSO.SetLock(false);
    }

    private void RePoolCard(Card card)
    {
        card.OnCardUsed -= RePoolCard;

        card.transform.SetParent(cardHolder);
        card.gameObject.SetActive(false);
        card.transform.position = Vector3.zero;
        deck.Add(card);
    }

    private void OnDestroy()
    {
        handController.OnNeedRefill -= FillHand;
    }
}