using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public PlayerController player;  // Needed to pass to enemies
    public EnemyController orc;
    public EnemyController ogre;
    public EnemyController minion;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform leftWalkToTarget;
    public Transform rightWalkToTarget;
    public float timeBetweenEnemiesInWave = 0.25f;
    public float beginnerWavesStartTime = 30.0f;
    public float intermediateWavesStartTime = 75.0f;
    public float hardWavesStartTime = 210.0f;

    public EnemyWave[] startWaves;
    public EnemyWave[] beginnerWaves;
    public EnemyWave[] intermediateWaves;
    public EnemyWave[] hardWaves;
    public EnemyWave finalWave;

    public UnityEvent finalWaveKilled;

    private bool _inFinalWave;
    private int _numEnemiesAlive;

    public void SpawnWave(float waveTime)
    {
        if (waveTime > hardWavesStartTime) {
            StartCoroutine(SpawnWave(hardWaves[Random.Range(0, hardWaves.Length)]));
        } else if (waveTime > intermediateWavesStartTime) {
            StartCoroutine(SpawnWave(intermediateWaves[Random.Range(0, intermediateWaves.Length)]));
        } else if (waveTime > beginnerWavesStartTime) {
            StartCoroutine(SpawnWave(beginnerWaves[Random.Range(0, beginnerWaves.Length)]));
        } else {
            StartCoroutine(SpawnWave(startWaves[Random.Range(0, startWaves.Length)]));
        }
    }

    public void SpawnFinalWave()
    {
        StartCoroutine(SpawnWave(finalWave, true));
    }

    IEnumerator SpawnWave(EnemyWave wave, bool isFinalWave = false)
    {
        for (int i = 0; i < wave.numMinions; ++i)
        {
            SpawnEnemy(minion);
            yield return new WaitForSeconds(timeBetweenEnemiesInWave);
        }
        
        for (int i = 0; i < wave.numOrcs; ++i)
        {
            SpawnEnemy(orc);
            yield return new WaitForSeconds(timeBetweenEnemiesInWave);
        }
        
        for (int i = 0; i < wave.numOgres; ++i)
        {
            SpawnEnemy(ogre);
            yield return new WaitForSeconds(timeBetweenEnemiesInWave);
        }

        _inFinalWave = isFinalWave;
    }

    private void SpawnEnemy(EnemyController enemyType)
    {
        var spawnRight = Random.Range(0.0f, 1.0f) > 0.5f;
        EnemyController enemy = Instantiate(enemyType, spawnRight ? rightSpawnPoint.position : leftSpawnPoint.position, Quaternion.identity);
        enemy.walkToTarget = spawnRight ? rightWalkToTarget : leftWalkToTarget;
        enemy.player = player;
        enemy.onDeath.AddListener(EnemyDied);
        ++_numEnemiesAlive;
    }

    private void EnemyDied()
    {
        --_numEnemiesAlive;
        if (_numEnemiesAlive == 0 && _inFinalWave) {
            finalWaveKilled.Invoke();
        }
    }
}

[Serializable]
public class EnemyWave
{
    public int numOrcs;
    public int numOgres;
    public int numMinions;
}
