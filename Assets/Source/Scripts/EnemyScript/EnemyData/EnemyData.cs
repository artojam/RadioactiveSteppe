using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TypeAttack
{
    Default = 0,
    Bullet,
    Rad,
    Chemical,
    Physical,
    Electro
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Game/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Data")]
    [SerializeField]
    private EnemyAnimEvent Skin;
    [SerializeField]
    private int HealthMax;

    [SerializeField]
    private Vector2Int Damage;
    [field: SerializeField]
    public TypeAttack AttackType { get; private set; }

    [SerializeField]
    private string Name;

    [SerializeField, Range(0f, 100f)]
    private float EscapeChance;

    [SerializeField]
    private List<ItemLoot> Loot;


    public int GetHealthMax() => HealthMax;
    public Vector2Int GetDamage() => Damage;
    public string GetName() => Name;
    public float GetEscapeChance() => EscapeChance;
    public List<ItemLoot> GetLoot() => Loot;
    public EnemyAnimEvent GetSkin() => Skin;

}
