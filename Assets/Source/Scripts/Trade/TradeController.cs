using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class TradeController : MonoBehaviour
{
    [SerializeField]
    private Transform TradePanel;

    [SerializeField]
    private float Fee;

    public List<Item> SellableItems = new List<Item>();

    [SerializeField]
    private List<InventoryCell> Cells = new List<InventoryCell>();

    public int IDUseCell;

    private int CountBuy = 0;

    public void Craet()
    {
        for (int i = 0; i < TradePanel.childCount; i++)
        {
            InventoryCell cell = TradePanel.GetChild(i)?.GetComponent<InventoryCell>();
            cell.SetCellType(CellType.Trade);
            cell.SetID(i);
            cell.Create();

            Cells.Add(cell);
        }

        for (int i = 0; i < SellableItems.Count; i++)
        {
            Item item = SellableItems[i];

            Cells[i].item = new ItemInstance(item);
            Cells[i].SetIcon(item);
        }

        Debug.Log("Create Trade");
    }

    public void Buy()
    {
        ItemInstance BuyItem = Cells[IDUseCell].item;
        int cost = (int)BuyItem.data.Cost * CountBuy; 
        if(GameController.GC.player.GetMoney() >= cost)
        {
            GameController.GC.player.UpdateMoney(-cost);
            GameController.GC.Inventory.AddItem(BuyItem, CountBuy);
        }
        else
        {
            GameController.GC.InfoPanel.Active("Недостаточно денег для покупки",$"У вас не достаточно денег для покупки { BuyItem.data.Name }\n" +
                $"Для покупки не хватает { cost - GameController.GC.player.GetMoney() } монет ");
        }

    }

    public void OpenInfoItem()
    {
        ItemInstance item = Cells[IDUseCell].item;
        if (!item.data) return;
        GameController.GC.InfoPanel.GetCostText().text = item.data.Cost.ToString();
        GameController.GC.InfoPanel.GetCountBuySlider().maxValue = item.data.MaxAmount;
        GameController.GC.InfoPanel.Active(item.data.Name, item.data.Info + item.data.Infor(), isTrade: true);

        bool isCount = Cells[IDUseCell].item.data.MaxAmount > 1;

        GameController.GC.InfoPanel.GetCountBuyInput().gameObject.SetActive(isCount);
        GameController.GC.InfoPanel.GetCountBuySlider().gameObject.SetActive(isCount);

        CountBuy = 1;
        GameController.GC.InfoPanel.GetCountBuyInput().text = CountBuy.ToString();
        GameController.GC.InfoPanel.GetCountBuySlider().value = CountBuy;
    }

    public void EndEditCountBuy()
    {
        int cost = (int)Cells[IDUseCell].item.data.Cost;
        int maxcount = (int)Cells[IDUseCell].item.data.MaxAmount;
        string textcount = GameController.GC.InfoPanel.GetCountBuyInput().text;
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

        if(count > maxcount)
        {
            textcount = maxcount.ToString();
            count = maxcount;
        }

        CountBuy = count;
        GameController.GC.InfoPanel.GetCountBuyInput().text = textcount;
        GameController.GC.InfoPanel.GetCountBuySlider().value = count;

        GameController.GC.InfoPanel.GetCostText().text = (cost * count).ToString();
    }

    public void OnSlide()
    {
        int cost = (int)Cells[IDUseCell].item.data.Cost;
        int count = (int)GameController.GC.InfoPanel.GetCountBuySlider().value;
        CountBuy = count;
        GameController.GC.InfoPanel.GetCountBuyInput().text = count.ToString();
        GameController.GC.InfoPanel.GetCostText().text = (cost * count).ToString();
    }

    public float GetFee() => Fee;

}
