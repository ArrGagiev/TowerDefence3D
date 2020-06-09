using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTower : MonoBehaviour
{
    public GameController controller;
    //--------------------------------//

    Queue<GameObject> QueueEnemyObjects = new Queue<GameObject>();
    
    public GameObject towerHead;
    private GameObject targetEnemy;
    private int damage;
    private float delayDamage;
    private LineRenderer lr;



    void Start()
    {
        lr = GetComponent<LineRenderer>();
        damage = controller.defenceTowerDamage;
        delayDamage = controller.delayBetweenDamage;

        StartCoroutine(Shoot());
    }
    
    void Update()
    {
        if (lr.enabled)
        {
            lr.enabled = false;
        }
    }

    private void LineRenderer()
    {
        if (!lr.enabled)
        {
            lr.enabled = true;
        }
        lr.SetPosition(0, towerHead.transform.position);
        lr.SetPosition(1, targetEnemy.transform.position);
    }

    private void OnTriggerEnter(Collider otherEnter)
    {
        if (otherEnter.CompareTag("Enemy"))
        {
            targetEnemy = otherEnter.gameObject;

            QueueEnemyObjects.Enqueue(targetEnemy);
        }
    }

    private void OnTriggerExit(Collider otherExit)
    {
        if (otherExit.CompareTag("Enemy") && targetEnemy)
        {
            targetEnemy = QueueEnemyObjects.Dequeue();
        }
    }
    

    IEnumerator Shoot()
    {
        while (true)
        {
            if (QueueEnemyObjects.Count != 0)
            {
                targetEnemy = QueueEnemyObjects.Peek();

                if (targetEnemy == null)
                {
                    QueueEnemyObjects.Dequeue();

                    if (QueueEnemyObjects.Count != 0)
                    {
                        targetEnemy = QueueEnemyObjects.Peek();
                    }
                    else
                    {
                        while (QueueEnemyObjects.Count == 0)
                        {
                            yield return null;
                        }
                    }
                }

                //---------------------------------------------------------<<>>

                var damageable = targetEnemy.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.GetDamage(damage);
                    LineRenderer();
                    Debug.Log("SHOOT!!!");
                }

            }
            
            yield return new WaitForSeconds(delayDamage);

            while (QueueEnemyObjects.Count == 0)
            {
                yield return null;
            }
        }
    }
}
