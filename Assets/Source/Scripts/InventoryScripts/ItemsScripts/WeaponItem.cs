using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum WeaponType
{
    Pistol = 0,
    ShotGun,
    AutomaticRifle,
    ShiperRifle
}

[CreateAssetMenu(fileName = "DefaultItem", menuName = "Game/Item/Create new Weapon item")]
public class WeaponItem : Item
{
    [Header("Weapon")]
    [SerializeField]
    private Sprite sprite; 
    [SerializeField]
    private Vector2Int Damage;
    [SerializeField]
    private int CriticalDamage;
    [SerializeField, Range(0f, 100f)]
    private float ProbabilityCriticalDamage;

    [SerializeField]
    private WeaponType TypeWeapon;
    [SerializeField]
    private int CountBulletMax;
    [SerializeField]
    private int NumberOfShots;
    [SerializeField]
    private float MaxDurability;
    [SerializeField]
    private Item Bullet;

    public override string Infor()
    {
        string text = base.Infor() + $"Урон: {Damage} - {CriticalDamage}\n" +
                                     $"Пули в абойме: {CountBulletMax}\n" +
                                     $"Кол-во выстрелов за раз: {NumberOfShots}\n" +
                                     $"Тип: {TypeWeapon}\n";
        return text ;
    }

    public Vector2Int GetDamage() => Damage;
    public int GetCountBulletMax() => CountBulletMax;
    public WeaponType GetWeaponType() => TypeWeapon;
    public int GetNumberOfShots() => NumberOfShots;
    public int GetCriticalDamage() => CriticalDamage;
    public float GetProbabilityCriticalDamage() => ProbabilityCriticalDamage;
    public Sprite GetSprite() => sprite;
    public float GetMaxDurability() => MaxDurability;
    public Item GetBullet() => Bullet;

    public override void Use(float _dur)
    {
        base.Use(_dur);
        ItemInstance _item = new ItemInstance(this);
        _item.CurrentDurability = _dur;
        GameController.GC.inventoryEquipment.AddCell(InventoryEquipment.WeaponCell, _item);
        GameController.GC.Inventory.ClearItem();
        GameController.GC.player.weapon.SetData(_item);
    }
}
