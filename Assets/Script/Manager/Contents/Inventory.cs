
using System.Collections.Generic;

public class Inventory
{
    public Dictionary<string, Item> GetItemList { get; private set; } = new Dictionary<string, Item>();

    public void AddItem(Item Item)
    {
        if (!GetItemList.ContainsKey(Item.GetName))
        {
            GetItemList.Add(Item.GetName,Item);
            return;
        }
        GetItemList[Item.GetName] = Item;
    }

    public void AddCountableItem(Item item, float amount)
    {
        if (!GetItemList.ContainsKey(item.GetName))
        {
            GetItemList.Add(item.GetName,item);
            item.SetAmount(amount);
            return;
        }
        item.SetAmount(item.GetAmount() + amount);
    }

    public void UseCountableItem(Item item, float amount,Character character)
    {
        if (!GetItemList.ContainsKey(item.GetName))
        {
            return;
        }
        GetItemList[item.GetName].UseItem(character);
        item.SetAmount(item.GetAmount() - amount);
    }
    public Item FindByItemName(string name)
    {
        if (!GetItemList.ContainsKey(name))
        {
            return null;
        }

        return GetItemList[name];
    }
    
}
