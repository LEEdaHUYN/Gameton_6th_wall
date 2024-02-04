
    using System.Collections.Generic;
    using UnityEngine;

    public class RandomItemAddAction : TriggerAction
    {
        public List<ItemInfo> items = new List<ItemInfo>();
        public override void RunAction()
        {
            //50 , 25 , 25
            float random = Random.Range(0,100);
            ItemInfo selectItem = new ItemInfo();
            foreach (var item in items)
            {
                if (random <= item.randomValue)
                {
                    selectItem = item;
                    break;
                }
            }
            Managers.Game.AddItem(selectItem.itemName,selectItem.amount);
           
        }
    }
