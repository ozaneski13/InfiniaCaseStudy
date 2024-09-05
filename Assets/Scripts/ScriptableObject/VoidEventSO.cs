using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidEventSO", menuName = "Scriptable Objects/VoidEventSO", order = 5)]
public class VoidEventSO : ScriptableObject
{
    private event Action voidEvent;

    public void Subscribe(Action subscriber)
    {
        voidEvent += subscriber;
    }

    public void Unsubscribe(Action subscriber)
    {
        voidEvent -= subscriber;
    }

    public void FireEvent()
    {
        voidEvent?.Invoke();
    }
}
