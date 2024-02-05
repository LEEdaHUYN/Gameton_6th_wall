
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class RandomLoseItemAction : TriggerAction
    {
        public List<ItemInfo> items = new ();
        public override void RunAction()
        {   
            
            List<ItemInfo> availableItems = 
                items.Where(item => Managers.Game.GetInventoryList().All(invItem => invItem.GetName == item.itemName)).
                    ToList();

            if (availableItems.Count == 0)
            {
                return;
            }
            Dictionary<ItemInfo, int> weightItem = 
                availableItems.ToDictionary(key => key,
                    value => value.weight);

            var item = WeightedRandomizer.From(weightItem).TakeOne();
            Debug.Log($"{item.itemName},{item.amount}");
            Managers.Game.SubItem(item.itemName,item.amount);
            

        }
    }
