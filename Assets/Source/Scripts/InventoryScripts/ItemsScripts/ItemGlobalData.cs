using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemGlobalData", menuName = "Game/Item/Create Item Global Data")]
public class ItemGlobalData : ScriptableObject
{
    [SerializeField]
    private List<Item> ListItems = new List<Item>();

    private void OnEnable()
    {
        HashSet<Item> HeshSetItem = new HashSet<Item>(ListItems);

        ListItems = new List<Item>(HeshSetItem);
    }

    private void OnValidate()
    {
        Create();
    }

    public void Create()
    {
        for (int i = 0; i < ListItems.Count; i++)
        {
            if (ListItems[i] != null)
                ListItems[i].SetId(i);
            else ListItems.Remove(ListItems[i]);
        }
        HashSet<Item> HeshSetItem = new HashSet<Item>(ListItems);

        ListItems.Clear();

        ListItems = new List<Item>(HeshSetItem);
    }

    public void Add(Item item)
    {
        if(!ListItems.Contains(item))
            ListItems.Add(item);
        Create();
    }

    public void Remove(Item item)
    {
        ListItems.Remove(item);
        Create();
    }
}
