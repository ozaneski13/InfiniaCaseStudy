using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] protected ScriptableObject settingsSO;
    [SerializeField] protected int poolSize;
    [SerializeField] protected int refillThreshold;
    [SerializeField] protected Transform poolParent;

    private void Awake()
    {
        InitPools();
    }

    protected virtual void InitPools()
    {

    }
}