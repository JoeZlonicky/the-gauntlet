using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public PlayerController player;
    public EnemyController orc;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform leftWalkToTarget;
    public Transform rightWalkToTarget;

    private bool _spawnRight;
    private const float SpawnDelay = 3.0f;

    public void Start()
    {
        StartCoroutine(nameof(SpawnEnemies));
    }

    IEnumerator SpawnEnemies()
    {
        for (;;) {
            if (player.isDead) {
                break;
            }
            EnemyController enemy = Instantiate(orc, _spawnRight ? rightSpawnPoint.position : leftSpawnPoint.position, Quaternion.identity);
            enemy.walkToTarget = _spawnRight ? rightWalkToTarget : leftWalkToTarget;
            enemy.player = player;

            _spawnRight = !_spawnRight;
            
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
}
