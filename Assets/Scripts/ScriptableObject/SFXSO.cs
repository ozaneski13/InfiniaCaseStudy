using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ESFXType
{
    BuildingGotHit,
    SoldierGotHit,
    KnightAttack,
    GiantAttack,
    ArcherAttack,
    BattleStart,
    BattleWin,
    BattleLost,
}

[Serializable]
public class SFXSettings
{
    [SerializeField] private ESFXType type;
    public ESFXType Type => type;

    [SerializeField] private AudioClip clip;
    public AudioClip Clip => clip;
}

[CreateAssetMenu(fileName = "SFXSO", menuName = "Scriptable Objects/SFXSO", order = 9)]
public class SFXSO : ScriptableObject
{
    [SerializeField] private List<SFXSettings> sfxs;

    public SFXSettings GetSFXSettingsByCardType(ESFXType type)
    {
        return sfxs.FirstOrDefault(sfx => sfx.Type == type);
    }
}