using System;
using UnityEngine;

[Serializable]
public class ItemLoot
{
    public Item item;
    public int amount;
    public float probability;

    public ItemLoot(Item _item, int _amount, float _probability)
    {
        item = _item;
        amount = _amount;
        probability = _probability;
    }
}
