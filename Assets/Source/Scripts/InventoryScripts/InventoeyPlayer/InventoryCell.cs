using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public enum CellType
{
    Player,
    Enemy,
    Trade,
    Equipment,
    Artifact
}

public class InventoryCell : MonoBehaviour
{
    public ItemInstance item;
    public int amount;

    [SerializeField]
    private Image Icon;
    [SerializeField]
    private TMP_Text TextAmount;
    [SerializeField]
    private GameObject TextAmountPanel;
    
    private CellType Type;

    private int ID;

    public void Create()
    {
        item = new ItemInstance();
        Icon.sprite = null;
        Icon.preserveAspect = true;
        Icon.color = new Color(0,0,0,0);
        TextAmountPanel.SetActive(false);
        if(TextAmount)
            TextAmount.text = "";
    }

    public void SetIcon(Item _item)
    {
        Icon.sprite = _item.Icon;
        Icon.color = new Color(1, 1, 1, 1);
    }

    public void SetTextAmount(int _amount)
    {
        TextAmountPanel.SetActive(_amount > 1);
        TextAmount.text = _amount > 1 ? _amount.ToString() : "";
    }

    public void ClearItem()
    {
        Icon.sprite = null;
        Icon.color = new Color(0, 0, 0, 0);
        if(TextAmount) TextAmount.text = "";
        item.data = null;
        amount = 0;
        TextAmountPanel.SetActive(false);
    }

    public void SetID(int id) => ID = id;
    public int GetID() => ID;

    public CellType GetCellType() => Type;
    public void SetCellType(CellType type) => Type = type;

    public void OpenInfoItem()
    {
        switch (Type)
        {
            case(CellType.Player):
                GameController.GC.Inventory.IDUseCell = ID;
                GameController.GC.Inventory.OpenInfoItem(item);
                break;

            case (CellType.Enemy):
                GameController.GC.EnemyInventory.IDUseCell = ID;
                GameController.GC.EnemyInventory.ActiveButton();
                break;

            case (CellType.Trade):
                GameController.GC.Trade.IDUseCell = ID;
                GameController.GC.Trade.OpenInfoItem();
                break;

            case (CellType.Equipment):
                GameController.GC.inventoryEquipment.IDUseCell = ID;
                GameController.GC.inventoryEquipment.OpenInfoItem(item);
                break;
            case (CellType.Artifact):
                GameController.GC.ArtifactEquipment.IDUseCell = ID;
                GameController.GC.ArtifactEquipment.OpenInfoItem(item);
                break;

        }



    }
}
