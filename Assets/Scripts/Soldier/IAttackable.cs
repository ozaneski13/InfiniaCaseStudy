using System;
using UnityEngine;

public interface IAttackable
{
    bool IsEnemy();
    Action<int, int> OnHit { get; set; }
    Action<IAttackable> OnDied { get; set; }
    Transform GetTransform();
    void GetHit(int damage);
}