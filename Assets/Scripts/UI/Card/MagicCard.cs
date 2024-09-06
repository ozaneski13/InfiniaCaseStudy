using System;
using UnityEngine;

public class MagicCard : Card
{
    private MagicSettings settings;

    private EMagicType magicType;

    public override void Init(ECardType cardType)
    {
        magicType = (EMagicType)Enum.ToObject(typeof(EMagicType), UnityEngine.Random.Range(0, Enum.GetValues(typeof(EMagicType)).Length));
        settings = (cardTypeSO.GetCardSettingsByCardType(cardType).TypeSO as MagicSO).GetMagicSettingsByType(magicType);
    }

    public override void Use(Vector3 pos)
    {
        GameObject go = MagicPoolController.Instance.GetMagicByType(magicType);
        go.SetActive(true);
        go.transform.position = pos;
    }
}