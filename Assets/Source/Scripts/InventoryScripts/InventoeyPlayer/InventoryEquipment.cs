using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEquipment : MonoBehaviour
{
    public static int HeadCell = 0;
    public static int FaseCell = 1;
    public static int BodyCell = 2;
    public static int LegCell = 3;
    public static int WeaponCell = 4;
    public static int BackpackCell = 5;
    public static int DetectorCell = 6;

    [SerializeField]
    private bool isArtifact = false;
    
    public List<InventoryCell> Cells = new List<InventoryCell>();

    private Transform tr;

    public int IDUseCell;

    public void Creat()
    {
        tr = transform;
        for (int i = 0; i < tr.childCount; i++)
        {
            InventoryCell cell = tr.GetChild(i).GetComponent<InventoryCell>();
            cell.SetID(i);
            cell.Create();
            Cells.Add(cell);
            if (isArtifact)
                cell.SetCellType(CellType.Artifact);
            else
                cell.SetCellType(CellType.Equipment);
        }
        Debug.Log($"Creat Equipment, isArtifact: { isArtifact }, name: {name}");
    }

    public void AddCell(int id, ItemInstance _item)
    {
        Debug.Log($"{id} {_item.data.Name}");
        Cells[id].item = _item;
        Cells[id].SetIcon(_item.data);

        if (id == WeaponCell)
        {
            WeaponItem item = ((WeaponItem)_item.data);
            GameController.GC.player.weapon.SetData(_item);
        }
    }

    public void AddCell(ItemInstance _item)
    {
        foreach(InventoryCell cell in Cells)
        {
            if(cell.item.data == null)
            {
                cell.item = _item;
                cell.SetIcon(_item.data);
                return;
            }
        }
    }

    public void ClearItem(int id) {
        if (!isArtifact)
        {
            if (id == WeaponCell)
            {
                GameController.GC.player.weapon.SetData(new ItemInstance());
            }
            else if (id == HeadCell)
            {
                GameController.GC.player.armor.ClearArmor(0);
            }
            else if (id == FaseCell)
            {
                GameController.GC.player.armor.ClearArmor(1);
            }
            else if (id == BodyCell)
            {
                GameController.GC.player.armor.ClearArmor(2);
            }
            else if (id == LegCell)
            {
                GameController.GC.player.armor.ClearArmor(3);
            }
        }
        else
        {
            ArtifactItem _item = (ArtifactItem)Cells[id].item.data;
            foreach(EffectArtifact effect in _item.Effects)
                GameController.GC.player.RemoveEffect(effect);
        }

        Cells[id].ClearItem();
    }

    public void ClearItem() => ClearItem(IDUseCell);

    public InventoryCell GetCell()
    {
        return Cells[IDUseCell];
    }

    public void OpenInfoItem(ItemInstance _item)
    {
        if (_item.data)
        {
            string _info = "";
            _info = _item.Info();

            GameController.GC.InfoPanel.Active(_item.data.Name, _info, isRemove: true);
            GameController.GC.Inventory.isArtifact = isArtifact;
        }
    }
}
