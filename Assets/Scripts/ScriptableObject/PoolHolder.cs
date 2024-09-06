using UnityEngine;

[CreateAssetMenu(fileName = "PoolHolder", menuName = "Scriptable Objects/PoolHolder", order = 6)]
public class PoolHolder : ScriptableObject
{
    [SerializeField] private GameObject poolPrefab;

    private GameObject pool;
    public GameObject Pool => pool;

    private void Awake()
    {
        pool = Instantiate(poolPrefab);
    }
}