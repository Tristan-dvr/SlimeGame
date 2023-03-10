using System;
using UnityEngine;

[Serializable]
public class SlimeConfig
{
    [Header("Health")]
    public float slimeHealth;
    public float slimeHealthPerLevel;
    [Header("Attack")]
    public float attack;
    public float attackPerLevel;
    [Header("Attack speed")]
    public float attackSpeed;
    public float attackSpeedPerLevel;
}
