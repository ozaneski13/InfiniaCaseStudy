using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagicPoolController : Pool
{
    private Dictionary<EMagicType, List<GameObject>> magicPool = new Dictionary<EMagicType, List<GameObject>>();

    public static MagicPoolController Instance;

    private void Awake()
    {
        Instance = this;
        InitPools();
    }

    protected override void InitPools()
    {
        foreach (EMagicType type in Enum.GetValues(typeof(EMagicType)))
        {
            magicPool.Add(type, new List<GameObject>());
            FillPool(type);
        }
    }

    private void FillPool(EMagicType type)
    {
        List<GameObject> magicList = magicPool[type];

        MagicSettings magicSettings = (settingsSO as MagicSO).GetMagicSettingsByType(type);

        for (int i = 0; i < poolSize - magicList.Count; i++)
        {
            GameObject magic = Instantiate(magicSettings.MagicPrefab, poolParent);
            magic.SetActive(false);

            magicList.Add(magic);
        }
    }

    public GameObject GetMagicByType(EMagicType type)
    {
        GameObject spawn = magicPool[type].FirstOrDefault(magic => !magic.activeInHierarchy);
        magicPool[type].Remove(spawn);

        if (magicPool[type].Count <= refillThreshold)
            FillPool(type);

        return spawn;
    }

    public void RefillPool(EMagicType type, GameObject magic)
    {
        magic.SetActive(false);
        magicPool[type].Add(magic);
    }
}