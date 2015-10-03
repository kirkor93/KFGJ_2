using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : Singleton<Spawner>
{
    public GameObject EnemyPrefab;
    public GameObject HipisPrefab;
    public List<Transform> SpawnPoints;

    private int _enemiesOnLevel = 0;
    private int _hipisOnLevel = 0;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        while(GameController.Instance == null)
        {
            yield return null;
        }
        GameController.Instance.OnNight += OnNight;
    }

    void Start()
    {
        if(SpawnPoints == null)
        {
            return;
        }
    }

    private void OnNight()
    {
        Invoke("SpawnEnemy", 1.0f);
        Invoke("SpawnHipis", 2.5f);
    }

    private void SpawnEnemy()
    {
        if(GameController.Instance.CurrentPeriod == Period.DAY)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, SpawnPoints.Count);
        Instantiate(EnemyPrefab, SpawnPoints[spawnPointIndex].position, Quaternion.identity);

        _enemiesOnLevel += 1;

        Invoke("SpawnEnemy", 2.0f * (_enemiesOnLevel + 1));
    }

    private void SpawnHipis()
    {
        if (GameController.Instance.CurrentPeriod == Period.DAY)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, SpawnPoints.Count);
        Instantiate(HipisPrefab, SpawnPoints[spawnPointIndex].position, Quaternion.identity);

        _hipisOnLevel += 1;

        Invoke("SpawnHipis", 3.0f * (_hipisOnLevel + 1));
    }

    public void EnemyDead()
    {
        _enemiesOnLevel -= 1;
    }

    public void HipisDead()
    {
        _hipisOnLevel -= 1;
    }
}
