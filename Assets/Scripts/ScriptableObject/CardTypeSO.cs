using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CardSettings
{
    [SerializeField] private ECardType type;
    public ECardType Type => type;

    [SerializeField] private GameObject cardPrefab;
    public GameObject CardPrefab => cardPrefab;

    [SerializeField] private ScriptableObject typeSO;
    public ScriptableObject TypeSO => typeSO;
}

[CreateAssetMenu(fileName = "CardTypeSO", menuName = "Scriptable Objects/CardTypeSO", order = 0)]
public class CardTypeSO : ScriptableObject
{
    [SerializeField] private List<CardSettings> cardTypes;

    public CardSettings GetCardSettingsByCardType(ECardType type)
    {
        return cardTypes.FirstOrDefault(card => card.Type == type);
    }
}