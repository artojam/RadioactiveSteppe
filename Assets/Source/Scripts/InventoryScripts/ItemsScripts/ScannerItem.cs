using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScannerItem", menuName = "Game/Item/Create new Scanner item")]
public class ScannerItem : Item
{
    [Header("Scanner")]
    [SerializeField, Range(0f, 100f)]
    private float ProbabilityPathArtifact;

    [SerializeField]
    private Item PowerType;

    [SerializeField]
    private float MaxPowerUser;

    public float GetProbabilityPathArtifact() => ProbabilityPathArtifact;
    public Item GetPowerType() => PowerType;
    public float GetMaxPowerUser() => MaxPowerUser;

    public override string Infor()
    {
        string text = base.Infor() + $"доп. шанс найти арифакт: +{ProbabilityPathArtifact}%\n"
                                   + $"тип питания: {PowerType.Name}\n";
        return text;
    }

    public override void Use(float _dur)
    {
        base.Use(_dur);
        ItemInstance _item = new ItemInstance(this);
        _item.CurrentDurability = _dur;
        GameController.GC.inventoryEquipment.AddCell(InventoryEquipment.DetectorCell, _item);
        GameController.GC.Inventory.ClearItem();
    }
}
