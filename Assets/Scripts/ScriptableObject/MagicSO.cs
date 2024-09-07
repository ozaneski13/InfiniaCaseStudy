using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MagicSettings:Settings
{
    [SerializeField] private EMagicType type;
    public EMagicType Type => type;

    [SerializeField] private int damage;
    public int Damage => damage;

    [SerializeField] private float duration;
    public float Duration => duration;
}

[CreateAssetMenu(fileName = "MagicSO", menuName = "Scriptable Objects/MagicSO", order = 2)]
public class MagicSO : ScriptableObject
{
    [SerializeField] private List<MagicSettings> magicSettings;

    public MagicSettings GetMagicSettingsByType(EMagicType type)
    {
        return magicSettings.FirstOrDefault(magic => magic.Type == type);
    }
}