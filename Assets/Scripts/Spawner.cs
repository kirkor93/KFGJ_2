﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : Singleton<Spawner>
{
    [Header("Night phase")]
    public GameObject EnemyPrefab;
    public GameObject HipisPrefab;
    public List<Transform> SpawnPoints;

    private int _enemiesOnLevel = 0;
    private int _hipisOnLevel = 0;

    [Header("Day phase")]
    public GameObject SunPrefab;
    public GameObject WormPrefab;

    private Vector3 _minimumPoint;
    private Vector3 _maximumPoint;

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
        GameController.Instance.OnDay += OnDay;
    }

    void Start()
    {
        if(SpawnPoints == null)
        {
            return;
        }
    }

    private void OnDay()
    {
        _minimumPoint = GameController.Instance.MainCamera.ScreenToWorldPoint(Vector3.zero);
        _maximumPoint = GameController.Instance.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Invoke("SpawnSun", 1.0f);
        Invoke("SpawnWorm", 0.3f);
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

    private void SpawnSun()
    {
        if (GameController.Instance.CurrentPeriod == Period.NIGHT)
        {
            return;
        }

        float x = Random.Range(_minimumPoint.x, _maximumPoint.x);
        float y = Random.Range(_minimumPoint.y, _maximumPoint.y);
        Vector3 position = new Vector3(x, y, 0.0f);
        Instantiate(SunPrefab, position, Quaternion.identity);

        Invoke("SpawnSun", Random.Range(1.0f, 2.0f));
    }

    private void SpawnWorm()
    {
        if(GameController.Instance.CurrentPeriod == Period.NIGHT)
        {
            return;
        }

        bool isXRandom = Random.Range(0, 2) == 0;

        float x = 0.0f;
        float y = 0.0f;
        if(isXRandom)
        {
            x = Random.Range(_minimumPoint.x, _maximumPoint.x);
            bool isTop = Random.Range(0, 2) == 0;
            y = isTop ? _maximumPoint.y : _minimumPoint.y;
        }
        else
        {
            y = Random.Range(_minimumPoint.y, _maximumPoint.y);
            bool isLeft = Random.Range(0, 2) == 0;
            x = isLeft ? _minimumPoint.x : _maximumPoint.x;
        }
        Vector3 position = new Vector3(x, y, 0.0f);
        Instantiate(WormPrefab, position, Quaternion.identity);

        Invoke("SpawnWorm", Random.Range(1.0f, 4.0f));
    }
}