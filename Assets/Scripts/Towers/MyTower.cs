using UnityEngine;
using UnityEngine.UI;

public class MyTower : MonoBehaviour, IDamageable, IGold, IDead
{
    public GameController controller;
    //--------------------------------//

    public Text myGoldText;
    public Text myHealthText;
    public Text deadEnemiesText;
    private int myHealth = 1000;
    private int myGold = 0;
    private int deadEnemies = 0;

    public GameObject restartWindowUI;
    public Text deadEnemiesTextUI;

    void Update()
    {
        myGoldText.text = "My Gold:" + myGold;
        myHealthText.text = "My Health:" + myHealth;
        deadEnemiesText.text = "Killed:" + deadEnemies;
        deadEnemiesTextUI.text = "Killed:" + deadEnemies;
    }

    public void GetDamage(int damageValue)
    {
        myHealth -= damageValue;

        if (myHealth <= 0)
        {
            myHealth = 0;
            gameObject.SetActive(false);
            restartWindowUI.SetActive(true);
        }
    }

    public void GoldForMurder(int goldValue)
    {
        myGold += goldValue;
    }

    public void Dead(int deadEnemiesValue)
    {
        deadEnemies += deadEnemiesValue;
    }
}
