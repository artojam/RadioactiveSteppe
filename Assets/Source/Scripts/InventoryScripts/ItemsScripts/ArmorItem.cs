using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType
{
    Head = 0,
    Fase,
    Body,
    Leg
}

[CreateAssetMenu(fileName = "ArmorItem", menuName = "Game/Item/Create new armor item")]
public class ArmorItem : Item
{
    [Header("Armor")]
    [SerializeField]
    private ArmorType armorType;
    [SerializeField]
    private Protection Data;
    [SerializeField]
    private float MaxDurability;

    [SerializeField]
    private List<Sprite> Sprites = new List<Sprite>();

    public List<Sprite> GetSprite() => Sprites;
    public Protection GetData() => Data;
    public ArmorType GetArmorType() => armorType;
    public float GetMaxDurability() => MaxDurability;

    public override void Use(float _dur)
    {
        base.Use(_dur);
        int id_invent = 0;
        if (armorType == ArmorType.Head)
            id_invent = InventoryEquipment.HeadCell;
        else if (armorType == ArmorType.Fase)
            id_invent = InventoryEquipment.FaseCell;
        else if (armorType == ArmorType.Body)
            id_invent = InventoryEquipment.BodyCell;
        else if (armorType == ArmorType.Leg)
            id_invent = InventoryEquipment.LegCell;

        ItemInstance _item = new ItemInstance(this);
        _item.CurrentDurability = _dur;
        GameController.GC.inventoryEquipment.AddCell(id_invent, _item );
        GameController.GC.Inventory.ClearItem();
        GameController.GC.player.armor.SetArmor(armorType, _item );
    }

    public override string Infor() 
    {
        string _info = "";
        if (Data.BulletProtection != 0)
            _info += $"Защита от пуль: {Data.BulletProtection}\n";
        if (Data.RadProtection != 0)
            _info += $"Рад. Защита: {Data.RadProtection}\n";
        if (Data.ChemicalProtection != 0)
            _info += $"Хим. Защита: {Data.ChemicalProtection}\n";
        if (Data.PhysicalProtection != 0)
            _info += $"Физ. Защита: {Data.PhysicalProtection}\n";
        if (Data.ElectroProtection != 0)
            _info += $"Элеко. Защита: { Data.ElectroProtection }\n";

        return base.Infor() + _info;
            
    }
}