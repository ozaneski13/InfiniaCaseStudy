using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SpawnSettings
{
    [SerializeField] private ESpawnType type;
    public ESpawnType Type => type;

    [SerializeField] private GameObject spawnPrefab;
    public GameObject SpawnPrefab => spawnPrefab;

    [SerializeField] private int health;
    public int Health => health;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private int range;
    public int Range => range;
}

[CreateAssetMenu(fileName = "SpawnSO", menuName = "Scriptable Objects/SpawnSO", order = 1)]
public class SpawnSO : ScriptableObject
{
    [SerializeField] private List<SpawnSettings> spawnSettings;

    public SpawnSettings GetSpawnSettingsByType(ESpawnType type)
    {
        return spawnSettings.FirstOrDefault(spawn => spawn.Type == type);
    }
}