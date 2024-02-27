
using System.Collections.Generic;

public class Inventory
{
    public Dictionary<string, Item> GetItemList { get; private set; } = new Dictionary<string, Item>();

    public void ClearInventory()
    {
        GetItemList.Clear();
    }
    public void AddItem(Item Item)
    {
        if ( Item == null||!GetItemList.ContainsKey(Item.GetName))
        {
            GetItemList.Add(Item.GetName,Item);
            return;
        }
        GetItemList[Item.GetName] = Item;
    }

    public void SubItem(Item item,float amount)
    {
        if ( item == null||!GetItemList.ContainsKey(item.GetName))
        {
            return;
        }

        var currentItem = GetItemList[item.GetName];
        float calculateAmount = currentItem.GetAmount() - amount;
        if (calculateAmount <= 0)
        {
            GetItemList.Remove(currentItem.GetName);
        }
        else
        {
            GetItemList[item.GetName].SetAmount(calculateAmount);
        }
      
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
        if ( item == null||!GetItemList.ContainsKey(item.GetName))
        {
            return;
        }
        GetItemList[item.GetName].UseItem(character);
        SubItem(item, amount);
    }
    public Item FindByItemName(string name)
    {
        if (!GetItemList.ContainsKey(name))
        {
            return null;
        }

        return GetItemList[name];
    }

    public void RemoveItem(string itemName)
    {
        if (!GetItemList.ContainsKey(itemName))
        {
            return;
        }

        GetItemList.Remove(itemName);

    }
}
