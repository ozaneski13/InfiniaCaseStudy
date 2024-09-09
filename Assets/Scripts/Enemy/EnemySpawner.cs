using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private List<GameObject> spawnPrefabs;

    [SerializeField] private Transform poolParent;
    [SerializeField] private int poolSize;
    [SerializeField] private int rePoolThreshold;

    [SerializeField] private float durationBeforeFirstSpawn;
    [SerializeField] private float spawnInterval;

    [SerializeField] private SFXSO sfxSO;
    [SerializeField] private AudioSource source;

    private Dictionary<int, List<GameObject>> spawnPool = new Dictionary<int, List<GameObject>>();

    private IEnumerator spawnRoutine;

    private void Awake()
    {
        InitPools();
    }

    private void InitPools()
    {
        for (int i = 0; i < spawnPrefabs.Count; i++)
        {
            spawnPool.Add(i, new List<GameObject>());
            InitPool(i);
        }
    }

    private void InitPool(int index)
    {
        List<GameObject> pool = spawnPool[index];
        int sizeCount = poolSize - pool.Count;

        for (int j = 0; j < sizeCount; j++)
        {
            GameObject spawnGO = Instantiate(spawnPrefabs[index], poolParent);
            spawnGO.SetActive(false);

            pool.Add(spawnGO);
        }

        spawnPool[index] = pool;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(durationBeforeFirstSpawn);

        source.PlayOneShot(sfxSO.GetSFXSettingsByCardType(ESFXType.BattleStart).Clip);

        spawnRoutine = SpawnRoutine();
        StartCoroutine(spawnRoutine);
    }

    private IEnumerator SpawnRoutine()
    {
        int index = Random.Range(0, spawnPrefabs.Count);
        GameObject spawn = spawnPool[index].FirstOrDefault(x => !x.activeInHierarchy);
        spawn.transform.position = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
        spawn.SetActive(true);

        spawn.GetComponent<Soldier>().SetAffiliate(false);

        if (spawnPool[index].Count <= rePoolThreshold)
            InitPool(index);

        yield return new WaitForSeconds(spawnInterval);

        spawnRoutine = SpawnRoutine();
        StartCoroutine(spawnRoutine);
    }

    private void OnDestroy()
    {
        if (spawnRoutine != null) 
            StopCoroutine(spawnRoutine);
    }
}