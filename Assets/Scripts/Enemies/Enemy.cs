using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IIncrease
{
    public EnemyData EnemyData;

    private Material _materialColor;
    private float _speed;
    private int _enemyDamage;
    private int _enemyHealth;
    private int _enemyGold;
    private int _life;
    

    private GameObject Tower;

    private Transform target;
    private int wayPointsIndex = 0;

    private void Awake()
    {
        _materialColor = EnemyData.MaterialColor;
        GetComponent<MeshRenderer>().material = _materialColor;
        _speed = EnemyData.Speed;
        _enemyDamage = EnemyData.EnemyDamage;
        _enemyHealth = EnemyData.EnemyHealth;
        _enemyGold = EnemyData.EnemyGold;
        _life = EnemyData.Life;
        //=================================================================
        
        Debug.Log(_enemyHealth);
        Tower = GameObject.FindWithTag("MyTower");
        target = WayPoints.points[0];
    }

    private void Update()//***
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * _speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            NextWayPoint();
        }
    }

    private void NextWayPoint()//***
    {
        if (wayPointsIndex >= WayPoints.points.Length - 1)
        {
            /*Destroy*/
            Lean.Pool.LeanPool.Despawn(gameObject);
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
                damageable.GetDamage(_enemyDamage);
            }
            gameObject.SetActive(false);
        }
    }

    public void GetDamage(int damageValue) // минус жизни врага, демидж от сторожевой башни
    {
        _enemyHealth -= damageValue;

        if (_enemyHealth <= 0)
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
            gold.GoldForMurder(_enemyGold);
        }
    }

    public void DeadPoint()
    {
        var dead = Tower.GetComponent<IDead>();
        if (dead != null)
        {
            dead.Dead(_life);
        }
    }

    public void Increase(int HealthUp)
    {
        _enemyHealth += HealthUp;
    }
}
