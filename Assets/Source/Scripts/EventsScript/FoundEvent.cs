using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFoundEvent", menuName = "Game/Event/New Found Event")]
public class GameFoundEvent : GameEvent
{
    [SerializeField]
    private List<Item> items = new List<Item>();
    [SerializeField]
    private int amount;
    
    private int _id;

    private void Awake()
    {
        e_Type = TypeEvent.FOUND; 
    }

    public override void Execute(int id)
    {
        _id = Random.Range(0, items.Count);
        UpdateInfoText(id);
        GameController.GC.Inventory.AddItem( new ItemInstance(items[_id]), amount);
    }

    private void UpdateInfoText(int id)
    {
        if(GameController.GC.isDebug)
            GameController.GC.InfoText.text = $"Ёвент под ID: {id} произашол\nполучин: <color=#CDAE01>\"{items[_id].Name}\"x{amount}</color>";
        else
            GameController.GC.InfoText.text = $"€ нашол: <color=#CDAE01>\"{items[_id].Name}\"x{amount}</color>";
    }
}
