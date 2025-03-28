using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform InventoryPanel;

    [SerializeField]
    private GameObject ButtonPanel;

    [SerializeField]
    private List<InventoryCell> Cells = new List<InventoryCell>();

    public int IDUseCell;

    public void Craet()
    {
        ButtonPanel.SetActive(false);
        
        InventoryPanel = transform;
        for (int i = 0; i < InventoryPanel.childCount; i++)
        {
            InventoryCell cell = InventoryPanel.GetChild(i)?.GetComponent<InventoryCell>();
            cell.SetCellType(CellType.Enemy);
            cell.SetID(i);
            cell.Create();
            Cells.Add(cell);
        }
        Debug.Log("Create Inventory Enemy");
    }

    public bool AddItem(ItemInstance _item, int amount)
    {
        int remainder = amount;
        foreach (InventoryCell cell in Cells)
        {
            if (cell.item.data && cell.item.data.ID == _item.data.ID)
            {
                if (cell.amount + amount <= cell.item.data.MaxAmount)
                {
                    cell.amount += amount;
                    cell.SetTextAmount(cell.amount);
                    return true;
                }
                else
                {
                    int HowCan = cell.item.data.MaxAmount - cell.amount;
                    cell.amount += HowCan;
                    cell.SetTextAmount(cell.amount);
                    remainder = amount - HowCan;
                }
            }
        }

        foreach (InventoryCell cell in Cells)
        {
            if (!cell.item.data)
            {
                if(amount < _item.data.MaxAmount)
                {
                    cell.amount = remainder;
                    cell.SetTextAmount(cell.amount);

                    cell.item = _item;
                    cell.SetIcon(_item.data);

                    return true;
                }
                else
                {
                    int HowCan = remainder - _item.data.MaxAmount;
                    cell.amount = _item.data.MaxAmount;
                    cell.SetTextAmount(cell.amount);

                    cell.item = _item;
                    cell.SetIcon(_item.data);

                    return AddItem(_item, HowCan) && true;
                }   
            }
        }
        Debug.LogError("Инвентарь полон");
        return false;
    }

    public void ClearItem()
    {
        Cells[IDUseCell].ClearItem();
    }

    public void ClearItem(int id)
    {
        InventoryCell cell = Cells[id];
        Cells[id].ClearItem();
    }

    public void Take()
    {
        GameController.GC.Inventory.AddItem(Cells[IDUseCell].item, Cells[IDUseCell].amount);
        ClearItem();
        ButtonPanel.SetActive(false);
    }

    public void TakeAll()
    {
        foreach(InventoryCell cell in Cells)
        {
            if(cell.item != null)
            {                                
                GameController.GC.Inventory.AddItem(cell.item, cell.amount);
                ClearItem(cell.GetID());
            }
            
        }
    }

    public void AddItems(List<ItemLoot> Loot)
    {
        foreach (ItemLoot item in Loot)
        {
            AddItem(new ItemInstance(item.item), item.amount);
        }
    }

    public void Clear()
    {
        foreach (InventoryCell cell in Cells)
        {
            if(cell.item != null)
                ClearItem(cell.GetID());
        }
    }

    public void ActiveButton() => ButtonPanel.SetActive(true); 

}
