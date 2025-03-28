using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private Transform InventoryPanel;

    [Header("Info Data")]
    [SerializeField]
    private GameObject InfoItemPanel;

    [SerializeField]
    private TMP_Text CarryingCapacityText;
    [SerializeField]
    private float CarryingCapacity;

    [SerializeField]
    private List<InventoryCell> Cells = new List<InventoryCell>();

    private float Weight;

    public int IDUseCell;
    public bool isArtifact;

    private int CountBuy = 1;

    public void Craet()
    {
        InfoItemPanel.SetActive(false);
        InventoryPanel = transform;
        for (int i = 0; i < InventoryPanel.childCount; i++)
        {
            InventoryCell cell = InventoryPanel.GetChild(i)?.GetComponent<InventoryCell>();
            cell.SetCellType(CellType.Player);
            cell.SetID(i);
            cell.Create();
            Cells.Add(cell);
        }
        CarryingCapacityText.text = $"{Weight}/{CarryingCapacity}";
        Debug.Log("Create");
    }

    public bool AddItem(ItemInstance _item, int _amount)
    {
        Debug.Log($"{_item.data.Name}");
        int remainder = _amount;
        float WeightInCell = (_item.data.Weight * _amount);
        Weight += (float)Math.Round(WeightInCell, 1);
        CarryingCapacityText.text = $"{Weight}/{CarryingCapacity}";
        foreach (InventoryCell cell in Cells)
        {
            if (cell.item.data && cell.item.data.ID == _item.data.ID)
            {
                if (cell.amount + _amount <= cell.item.data.MaxAmount)
                {
                    cell.amount += _amount;
                    cell.SetTextAmount(cell.amount);
                    return true;
                }
                else
                {
                    int HowCan = cell.item.data.MaxAmount - cell.amount;
                    Debug.Log($"HowCan: {HowCan}");
                    cell.amount += HowCan;
                    cell.SetTextAmount(cell.amount);
                    remainder = _amount - HowCan;
                }
            }
        }

        foreach (InventoryCell cell in Cells)
        {
            if (!cell.item.data)
            {
                if (remainder <= _item.data.MaxAmount)
                {
                    cell.amount = remainder;
                    cell.SetTextAmount(cell.amount);

                    cell.item.data = _item.data;
                    cell.item.CurrentDurability = _item.CurrentDurability;

                    cell.SetIcon(_item.data);

                    return true;
                }
                else
                {
                    int HowCan = remainder - _item.data.MaxAmount;
                    cell.amount = _item.data.MaxAmount;
                    cell.SetTextAmount(cell.amount);

                    cell.item.data = _item.data;
                    cell.SetIcon(_item.data);

                    return AddItem(_item, HowCan) && true;
                }
            }
        }
        Debug.LogError("Инвентарь полон");
        return false;
    }

    public InventoryCell PathItem(int _id_item)
    {
        foreach(InventoryCell cell in Cells)
        {
            if (cell.item.data)
            {
                if (cell.item.data.ID == _id_item)
                    return cell;
            }
        }
        return null;
    }

    public void CalculationWeight(InventoryCell cell, int op)
    {
        Weight += ((cell.item.data.Weight * cell.amount) * op);
        CarryingCapacityText.text = $"{Math.Round(Weight, 1)}/{CarryingCapacity}"; ;
    }

    public void CalculationWeight(InventoryCell cell, int _amount, int op)
    {
        Weight += ((cell.item.data.Weight * _amount) * op);
        CarryingCapacityText.text = $"{Math.Round(Weight, 1)}/{CarryingCapacity}"; ;
    }


    public void CalculationWeight()
    {
        Weight = 0;
        foreach(InventoryCell cell in Cells)
        {
            if(cell.item.data != null) 
                CalculationWeight(cell, 1);
        }
    }

    public void ClearItem()
    {
        ClearItem(IDUseCell);
    }

    public void ClearItem(int id)
    {
        CalculationWeight(Cells[id], -1);
        Cells[id].ClearItem();
        InfoItemPanel.SetActive(false);
    }

    public void ClearAmountItem(int _amount)
    {
        InventoryCell cell = Cells[IDUseCell];
        if(cell.amount - _amount <= 0 || cell.amount < _amount)
        {
            ClearItem();
            return;
        }
        else
        {
            Cells[IDUseCell].amount -= _amount;
            Cells[IDUseCell].SetTextAmount(Cells[IDUseCell].amount);
        }
        CalculationWeight();
    }

    public void OpenInfoItem(ItemInstance _item)
    {
        if (_item.data)
        {
            bool isTrade = GameController.GC.GetIDHome() == (int)IdPanel.Trader;
            bool isUse = _item.data.IsUse && !isTrade;
            bool isDefuld = !isTrade;
            bool isDivide = (Cells[IDUseCell].amount > 1);

            string _info = "";
            _info = _item.Info();

            GameController.GC.InfoPanel
                .Active(_item.data.Name, _info,
                    isUse: isUse, isDrop: isDefuld, isDivige: isDivide && !isTrade, isSell: isTrade);

            if (!isTrade) return;

            int fee = Mathf.RoundToInt(_item.data.Cost * GameController.GC.Trade.GetFee());
            int money = (int)_item.data.Cost - fee;
            int count = 1;
            GameController.GC.InfoPanel.GetSellText().text = $"{ money }";

            GameController.GC.InfoPanel.GetCountSellSlider().value = count;
            GameController.GC.InfoPanel.GetCountSellSlider().maxValue = Cells[IDUseCell].amount;
            Debug.Log($"amount: {Cells[IDUseCell].amount}, ID: {IDUseCell}");

            GameController.GC.InfoPanel.GetCountSellInput().text = count.ToString();

            GameController.GC.InfoPanel.GetCountSellInput().gameObject.SetActive(isDivide);
            GameController.GC.InfoPanel.GetCountSellSlider().gameObject.SetActive(isDivide);

        }
    }

    public void Divide()
    {
        int old_amount = Cells[IDUseCell].amount;
        
        if (old_amount < 2) return;

        int _amount = Mathf.RoundToInt(old_amount / 2);
        
        Cells[IDUseCell].amount = Cells[IDUseCell].item.data.MaxAmount;
        
        if(AddItem(Cells[IDUseCell].item, _amount))
        {
            Cells[IDUseCell].amount = old_amount;
            Cells[IDUseCell].amount -= _amount;
            Cells[IDUseCell].SetTextAmount(Cells[IDUseCell].amount);
            CalculationWeight();
        }
        else
        {
            Cells[IDUseCell].amount = old_amount;
        }
    } 
    
    public void OnSellItem()
    {
        Debug.Log("test");
        int fee = Mathf.RoundToInt(Cells[IDUseCell].item.data.Cost * GameController.GC.Trade.GetFee());
        int money = (int)Cells[IDUseCell].item.data.Cost - fee;
        ClearAmountItem(CountBuy);
        GameController.GC.player.UpdateMoney(money * CountBuy);
        GameController.GC.InfoPanel.Close();
    }

    public void EndEditCountBuy()
    {
        int fee = Mathf.RoundToInt(Cells[IDUseCell].item.data.Cost * GameController.GC.Trade.GetFee());
        int money = (int)Cells[IDUseCell].item.data.Cost - fee;

        int maxcount = (int)Cells[IDUseCell].item.data.MaxAmount;
        string textcount = GameController.GC.InfoPanel.GetCountSellInput().text;
        int count = 0;
        if (textcount.Length <= 0)
            count = 1;
        else
            count = int.Parse(textcount);

        if (count < 1)
        {
            textcount = 1.ToString();
            count = 1;
        }

        if (count > maxcount)
        {
            textcount = maxcount.ToString();
            count = maxcount;
        }

        CountBuy = count;
        GameController.GC.InfoPanel.GetCountSellInput().text = textcount;
        GameController.GC.InfoPanel.GetCountSellSlider().value = count;

        GameController.GC.InfoPanel.GetSellText().text = $"{(money * count)}";
    }

    public void OnSlide()
    {
        int fee = Mathf.RoundToInt(Cells[IDUseCell].item.data.Cost * GameController.GC.Trade.GetFee());
        int money = (int)Cells[IDUseCell].item.data.Cost - fee;

        int count = (int)GameController.GC.InfoPanel.GetCountSellSlider().value;
        CountBuy = count;
        GameController.GC.InfoPanel.GetCountSellInput().text = count.ToString();
        GameController.GC.InfoPanel.GetCostText().text = (money * count).ToString();
    }

    public void OnRemovedItem()
    {
        InfoItemPanel.SetActive(false);
        if (isArtifact)
        {
            AddItem(GameController.GC.ArtifactEquipment.GetCell().item, 1);
            GameController.GC.ArtifactEquipment.ClearItem();
            return;
        }
        AddItem(GameController.GC.inventoryEquipment.GetCell().item, 1);
        GameController.GC.inventoryEquipment.ClearItem();
        
    }

    public void OnUseItem()
    {
        float _dur = 0f;
        ItemInstance _item = Cells[IDUseCell].item;
        _dur = _item.CurrentDurability;

        Cells[IDUseCell].item.data.Use(_dur);
    }
    public float GetWeight() => CarryingCapacity;

    public static void SetCell(InventoryCell cell, Item _item)
    {
        cell.amount = 1;
        cell.SetTextAmount(cell.amount);

        cell.item.data = _item;
        cell.SetIcon(_item);
    }

}
