using System;
using UnityEngine;

[Serializable]
public class PriceConfig
{
    [Header("Health")]
    public int healthPrice;
    public float healthPricePerLevel;
    [Header("Attack")]
    public int attackPrice;
    public float attackPricePerLevel;
    [Header("Attack speed")]
    public int attackSpeedPrice;
    public float attackSpeedPricePerLevel;
}
