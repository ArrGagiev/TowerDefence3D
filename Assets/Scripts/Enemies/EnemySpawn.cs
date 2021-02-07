using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameController Controller;
    //--------------------------------//

    public Enemy[] Enemies;
    private int _waveCount;
    private float _waveDelay;
    private int _enemiesCount;
    private int _healthBonus = 10;
    private int _waveValue;
    private float _enemySpawnDelay = 1f;

    private void Awake()
    {
        _waveDelay = Controller.waveDelay;
        _waveCount = Controller.waveCount;
        _waveValue = _waveCount;
        StartCoroutine(WaveTrigger());
    }

    IEnumerator EnemySpawner()
    {
        _enemiesCount = _waveCount + Random.Range(0, 5); // рандомное количество врагов в каждой волне (K + X)
        for (int i = _enemiesCount; i > 0; i--)
        {
            int randomEnemy = Random.Range(0, Enemies.Length);
            Enemy enemy = Lean.Pool.LeanPool.Spawn(Enemies[randomEnemy], transform.position, Quaternion.identity);

            //---------------------------------------------------<< передаем врагам бонус здоровья
            enemy.Increase(_healthBonus);
            //---------------------------------------------------<<

            yield return new WaitForSeconds(_enemySpawnDelay);
        }
        yield return StartCoroutine(WaveTrigger());
    }

    IEnumerator WaveTrigger()
    {
        if (_waveCount <= 0)
        {
            StopCoroutine(WaveTrigger());
        }

        yield return new WaitForSeconds(_waveDelay);

        if (_waveCount <= 0)
        {
            StopCoroutine(EnemySpawner());
        }
        else
        {
            StartCoroutine(EnemySpawner());
        }
        HealthUp(); // увеличиваем бонус здоровья каждую волну
        _waveCount--;
    }

    public void HealthUp()
    {
        if (_waveCount != _waveValue)
        {
            _healthBonus += _healthBonus;
        }
    }

}