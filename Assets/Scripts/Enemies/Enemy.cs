using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IIncrease
{
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
    //говорят тут что-то было ))
    //это духи _полей которых тут больше нет
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

    public EnemyData EnemyData;
    private GameObject Tower;
    private Transform target;
    private int wayPointsIndex = 0;

    private void Awake()
    {
        GetComponent<MeshRenderer>().material = EnemyData.MaterialColor; ;
        Tower = GameObject.FindWithTag("MyTower");
        target = WayPoints.points[0];
    }

    private void Update()//***
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * EnemyData.Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            NextWayPoint();
        }
    }

    private void NextWayPoint()//***
    {
        if (wayPointsIndex >= WayPoints.points.Length - 1)
        {
            Destroy(gameObject); //----------------------------<<<   болячка*
            return;
        }
        wayPointsIndex++;
        target = WayPoints.points[wayPointsIndex];
    }

    private void OnTriggerEnter(Collider other) //враг наносит урон и умирает
    {
        if (other.CompareTag("MyTower"))
        {
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(EnemyData.EnemyDamage);
            }
            Lean.Pool.LeanPool.Despawn(gameObject);
        }
    }

    public void GetDamage(int damageValue) // минус жизни врага, демидж от сторожевой башни
    {
        EnemyData.EnemyHealth -= damageValue;

        if (EnemyData.EnemyHealth <= 0)
        {
            GiveGold();
            //DeadPoint();
            Lean.Pool.LeanPool.Despawn(gameObject);
        }
    }

    public void GiveGold()
    {
        var gold = Tower.GetComponent<IGold>();
        if (gold != null)
        {
            gold.GoldForMurder(EnemyData.EnemyGold);
        }
    }

    //public void DeadPoint()
    //{
    //    var dead = Tower.GetComponent<IDead>();
    //    if (dead != null)
    //    {
    //        dead.Dead(_life);
    //    }
    //}

    public void Increase(int HealthUp)
    {
        EnemyData.EnemyHealth += HealthUp;
    }
}
