
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class RandomItemAddAction : TriggerAction
    {
        public List<ItemInfo> items = new ();
        public int addCount;
        public override void RunAction()
        {

            for (int i = 0; i < addCount; i++)
            {
                List<ItemInfo> availableItems = 
                    items.Where(item => Managers.Game.GetInventoryList().All(invItem => invItem.GetName != item.itemName)).
                        ToList();

                if (availableItems.Count != 0)
                {
                    Dictionary<ItemInfo, int> weightItem = 
                        availableItems.ToDictionary(key => key,
                            value => value.weight);

                    var item = WeightedRandomizer.From(weightItem).TakeOne();
                    Debug.Log($"{item.itemName},{item.amount}");
                    Managers.Game.AddItem(item.itemName,item.amount);
                }
                else
                {
                    Managers.Game.AddItem("CanFood",1);
                    Managers.Game.AddItem("Water",1);
                    Debug.Log($"CanFood& Water 획득");
                }
            }
        }
    }
