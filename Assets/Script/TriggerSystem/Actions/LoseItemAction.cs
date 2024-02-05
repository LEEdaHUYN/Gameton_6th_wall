
    public class LoseItemAction : TriggerAction
    {
        public ItemInfo item;
        public override void RunAction()
        {
            Managers.Game.SubItem(item.itemName,item.amount);
        }
    }
