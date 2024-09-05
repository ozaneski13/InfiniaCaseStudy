using System;
using UnityEngine;

public class SpawnCard : Card
{
    private SpawnSettings settings;

    private ESpawnType spawnType;

    public override void Init(ECardType cardType)
    {
        spawnType = (ESpawnType)Enum.ToObject(typeof(ESpawnType), UnityEngine.Random.Range(0, Enum.GetValues(typeof(ESpawnType)).Length));
        settings = (cardTypeSO.GetCardSettingsByCardType(cardType).TypeSO as SpawnSO).GetSpawnSettingsByType(spawnType);
    }

    public override void Use(Vector3 pos)
    {

    }
}