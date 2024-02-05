
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class RandomItemAddAction : TriggerAction
    {
        public List<ItemInfo> items = new ();
        public override void RunAction()
        {
            Dictionary<ItemInfo, int> weightItem = 
                items.ToDictionary(key => key,
                    value => value.weight);

                var item = WeightedRandomizer.From(weightItem).TakeOne();
                Managers.Game.AddItem(item.itemName,item.amount);

        }
    }
