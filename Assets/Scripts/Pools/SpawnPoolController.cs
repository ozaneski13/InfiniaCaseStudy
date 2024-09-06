using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPoolController : Pool
{
    private Dictionary<ESpawnType, List<GameObject>> spawnPool = new Dictionary<ESpawnType, List<GameObject>>();

    public static SpawnPoolController Instance;

    private void Awake()
    {
        Instance = this;
        InitPools();
    }

    protected override void InitPools()
    {
        foreach(ESpawnType type in Enum.GetValues(typeof(ESpawnType)))
        {
            spawnPool.Add(type, new List<GameObject>());
            FillPool(type);
        }
    }

    private void FillPool(ESpawnType type)
    {
        List<GameObject> spawnList = spawnPool[type];

        SpawnSettings spawnSettings = (settingsSO as SpawnSO).GetSpawnSettingsByType(type);

        for (int i = 0; i < poolSize - spawnList.Count; i++)
        {
            GameObject spawn = Instantiate(spawnSettings.Prefab, poolParent);
            spawn.SetActive(false);

            spawnList.Add(spawn);
        }
    }

    public GameObject GetSpawnByType(ESpawnType type)
    {
        GameObject spawn = spawnPool[type].FirstOrDefault(spawn => !spawn.activeInHierarchy);
        spawnPool[type].Remove(spawn);

        if (spawnPool[type].Count <= refillThreshold)
            FillPool(type);

        return spawn;
    }

    public bool RefillPool(ESpawnType type, GameObject spawn)
    {
        spawn.SetActive(false);
        spawnPool[type].Add(spawn);

        return true;
    }
}