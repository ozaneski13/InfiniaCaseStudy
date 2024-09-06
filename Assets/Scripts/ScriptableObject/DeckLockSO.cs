using UnityEngine;

[CreateAssetMenu(fileName = "DeckLockSO", menuName = "Scriptable Objects/DeckLockSO", order = 7)]
public class DeckLockSO : ScriptableObject
{
    private bool isLocked = false;
    public bool IsLocked => isLocked;

    public void SetLock(bool status) => isLocked = status;
}