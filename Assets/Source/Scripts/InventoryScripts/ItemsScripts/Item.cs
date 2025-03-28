using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEditor.Progress;

public enum TypeItem
{
    Default = 0,
    Food,
    Weapon,
    Armor,
    Artifact,
    Scanner
}
[CreateAssetMenu(fileName = "DefaultItem", menuName = "Game/Item/Create new default item")]
public class Item : ScriptableObject
{
    [field: SerializeField]
    public Sprite Icon { get; private set; } = null;
    [field: SerializeField]
    public string Name { get; private set; } = "Item";
    [field: SerializeField, TextArea(1, 10)]
    public string Info { get; private set; } = "Not";

    [field: SerializeField, Space(15)]
    public int MaxAmount { get; private set; } = 8;

    [field: SerializeField]
    public float Weight { get; private set; } = 0.1f;

    [field: SerializeField]
    public int Cost { get; private set; } = 10;

    [field: SerializeField]
    public TypeItem ItemType { get; private set; } = TypeItem.Default;

    [field: SerializeField]
    public bool IsUse { get; private set; } = true;


    [field: SerializeField]
    public int ID { get; private set; } = 0;

    public void SetId(int _id) => ID = _id;

    protected virtual void OnEnable()
    {
        ItemGlobalData itemGlobalData = Resources.Load<ItemGlobalData>("ItemGlobalData");

        if (itemGlobalData != null) itemGlobalData.Add(this);
        else Debug.LogError("not path ItemGlobalData");
    }

    public string GetInfo()
    {
        return Info;
    }

    public virtual void Use(float _dur) 
    {
        Debug.Log($"Use item  name: { name }, id: { ID }, type: { ItemType },Weight: {Weight}, Durability: { _dur }");
    }

    public virtual string Infor() { return $"\n\nВес: {Weight}\n"; }

}

[Serializable]
public class ItemInstance
{
    public Item data;
    public float CurrentDurability;

    public ItemInstance(Item _item)
    {
        data = _item;
        if (_item.ItemType == TypeItem.Weapon)
            CurrentDurability = ((WeaponItem)_item).GetMaxDurability();
        else if(_item.ItemType == TypeItem.Armor)
            CurrentDurability = ((ArmorItem)_item).GetMaxDurability();
        else if (data.ItemType == TypeItem.Scanner)
            CurrentDurability = ((ScannerItem)data).GetMaxPowerUser();
    }

    /*public ItemInstance(WeaponItem _item)
    {
        data = _item;
        CurrentDurability = _item.GetMaxDurability();
    }

    public ItemInstance(ArmorItem _item)
    {

        data = _item;
        CurrentDurability = _item.GetMaxDurability();

    }*/

    public ItemInstance()
    {
        data = null;
        CurrentDurability = 0;
    }

    public string Info()
    {
        string _dur = "";
        if (data.ItemType == TypeItem.Weapon || data.ItemType == TypeItem.Armor)
            _dur = $"прочность: {CurrentDurability}\n";
        else if (data.ItemType == TypeItem.Scanner)
        {
            float _max = ((ScannerItem)data).GetMaxPowerUser();
            float _percent = (float)Math.Round(CurrentDurability / _max, 3) * 100;
            _dur = $"Заряд: {_percent}%({CurrentDurability})\n";
        }
        return data.Info + data.Infor() + _dur;
    }

    public bool OnDurability(float _value)
    {
        CurrentDurability -= _value;
        return CurrentDurability <= 0;
    }

}
