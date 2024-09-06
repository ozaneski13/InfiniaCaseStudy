using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Settings
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    [SerializeField] private int cost;
    public int Cost => cost;

    [SerializeField] private Sprite sprite;
    public Sprite Sprite => sprite;
}

[Serializable]
public class SpawnSettings: Settings
{
    [SerializeField] private ESpawnType type;
    public ESpawnType Type => type;

    [SerializeField] private int health;
    public int Health => health;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private int range;
    public int Range => range;

    [SerializeField] private float attackInterval;
    public float AttackInterval => attackInterval;
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