
    public class AddItemAction : TriggerAction
    {
        public ItemInfo item;
        public override void RunAction()
        { 
            Managers.Game.AddItem(item.itemName,item.amount);
        }
    }
