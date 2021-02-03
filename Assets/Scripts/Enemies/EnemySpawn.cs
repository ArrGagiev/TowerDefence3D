using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameController controller;
    //--------------------------------//

    public GameObject[] enemies; // я менял тип массива, но тогда... {str30}
    private int waveCount;
    private float waveDelay;
    private int enemiesCount;
    private int healthBonus = 10;
    private int waveValue;
    private float enemyDelay = 1f;

    private void Awake()
    {
        waveDelay = controller.waveDelay;
        waveCount = controller.waveCount;
        waveValue = waveCount;
        StartCoroutine(WaveTrigger());
    }

    IEnumerator EnemySpawner()
    {
        enemiesCount = waveCount + Random.Range(0, 5); // рандомное количество врагов в каждой волне (K + X)
        for (int i = enemiesCount; i > 0; i--)
        {
            int randomEnemy = Random.Range(0, enemies.Length); //...крашится вот это выражение..........................................
            /*Instantiate*/
            GameObject enemy = Lean.Pool.LeanPool.Spawn(enemies[randomEnemy], transform.position, Quaternion.identity) as GameObject; //......и это тоже...

            //---------------------------------------------------<< передаем врагам бонус здоровья
            var instEnemy = enemy.GetComponent<IIncrease>();
            if (instEnemy != null)
            {
                instEnemy.Increase(healthBonus);
            }
            //---------------------------------------------------<<

            yield return new WaitForSeconds(enemyDelay);
        }
        yield return StartCoroutine(WaveTrigger());
    }

    IEnumerator WaveTrigger()
    {
        if (waveCount <= 0)
        {
            StopCoroutine(WaveTrigger());
        }

        yield return new WaitForSeconds(waveDelay);

        if (waveCount <= 0)
        {
            StopCoroutine(EnemySpawner());
        }
        else
        StartCoroutine(EnemySpawner());

        HealthUp(); // увеличиваем бонус здоровья каждую волну

        waveCount--;
    }

    public void HealthUp()
    {
        if (waveCount != waveValue)
        {
            healthBonus += healthBonus;
        }
    }

}