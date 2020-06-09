using UnityEngine;

public class GreenEnemy : MonoBehaviour, IDamageable, IIncrease
{
    private float speed = 1f;

    private int enemyDamage = 60;
    private int enemyHealth = 60;
    private int enemyGold = 60;
    private int life = 1;

    private GameObject tower;

    private Transform target;
    private int wayPointsIndex = 0;

    void Start()
    {
        tower = GameObject.FindWithTag("MyTower");
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

    public void GetDamage(int damageValue)
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
        var gold = tower.GetComponent<IGold>();
        if (gold != null)
        {
            gold.GoldForMurder(enemyGold);
        }
    }

    public void DeadPoint()
    {
        var dead = tower.GetComponent<IDead>();
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
