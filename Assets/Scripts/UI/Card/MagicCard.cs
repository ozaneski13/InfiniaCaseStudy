using System;
using System.Collections;
using UnityEngine;

public class MagicCard : Card
{
    private EMagicType magicType;

    public override void Init(ECardType cardType)
    {
        magicType = (EMagicType)Enum.ToObject(typeof(EMagicType), UnityEngine.Random.Range(0, Enum.GetValues(typeof(EMagicType)).Length));
        settings = (cardTypeSO.GetCardSettingsByCardType(cardType).TypeSO as MagicSO).GetMagicSettingsByType(magicType);
    }

    public override void Use(Vector3 pos)
    {
        StartCoroutine(MagicUseRoutine(pos));
    }

    private IEnumerator MagicUseRoutine(Vector3 pos)
    {
        GameObject go = MagicPoolController.Instance.GetMagicByType(magicType);
        go.SetActive(true);
        go.transform.position = pos;

        Magic magic = go.GetComponent<Magic>();
        magic.StartEffect((settings as MagicSettings).Damage);

        visual.SetActive(false);
        
        yield return new WaitForSeconds((settings as MagicSettings).Duration);

        magic.StopEffect();
        MagicPoolController.Instance.RefillPool(magicType, go);
        OnCardUsed?.Invoke(this);
    }
}