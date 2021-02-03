using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public Material MaterialColor;
    public float Speed;//1
    public int EnemyDamage;
    public int EnemyHealth;
    public int EnemyGold;
}
