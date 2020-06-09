using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Controller")]
public class GameController : ScriptableObject
{
    public int waveCount;

    public int defenceTowerDamage;

    public float delayBetweenDamage;

    public float waveDelay;
}

