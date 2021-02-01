using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IIncrease
{
    private float speed = 1f;

    public Material materialColor;
    public int enemyDamage = 50;
    public int enemyHealth = 50;
    public int enemyGold = 50;
    public int life = 1;

    private GameObject Tower;

    private Transform target;
    private int wayPointsIndex = 0;

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = materialColor;
        Debug.Log(enemyHealth);
        Tower = GameObject.FindWithTag("MyTower");
        target = WayPoints.points[0];
    }

    void Update()//***
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            NextWayPoint();
        }
    }

    private void NextWayPoint()//***
    {
        if (wayPointsIndex >= WayPoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }
        wayPointsIndex++;
        target = WayPoints.points[wayPointsIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyTower"))
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(enemyDamage);
            }
            gameObject.SetActive(false);
        }
    }

    public void GetDamage(int damageValue) // минус жизни врага, демидж от сторожевой башни
    {
        enemyHealth -= damageValue;

        if (enemyHealth <= 0)
        {
            GiveGold();
            DeadPoint();
            Destroy(gameObject);
        }
    }

    public void GiveGold()
    {
        var gold = Tower.GetComponent<IGold>();
        if (gold != null)
        {
            gold.GoldForMurder(enemyGold);
        }
    }

    public void DeadPoint()
    {
        var dead = Tower.GetComponent<IDead>();
        if (dead != null)
        {
            dead.Dead(life);
        }
    }

    public void Increase(int HealthUp)
    {
        enemyHealth += HealthUp;
    }
}
