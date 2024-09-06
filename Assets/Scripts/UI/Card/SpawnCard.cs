using System;
using UnityEngine;

public class SpawnCard : Card
{
    //[SerializeField] private PoolHolder pool;
    private ESpawnType spawnType;

    public override void Init(ECardType cardType)
    {
        spawnType = (ESpawnType)Enum.ToObject(typeof(ESpawnType), UnityEngine.Random.Range(0, Enum.GetValues(typeof(ESpawnType)).Length));
        settings = (cardTypeSO.GetCardSettingsByCardType(cardType).TypeSO as SpawnSO).GetSpawnSettingsByType(spawnType);
    }

    public override void Use(Vector3 pos)
    {
        GameObject go = SpawnPoolController.Instance.GetSpawnByType(spawnType);
        go.SetActive(true);
        go.transform.position = pos;

        go.GetComponent<Soldier>().SetAffiliate(true);
    }
}