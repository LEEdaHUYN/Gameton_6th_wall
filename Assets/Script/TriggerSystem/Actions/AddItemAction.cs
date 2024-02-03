
    public class AddItemAction : TriggerAction
    {
        public string itemName;
        public float amount; 
        public override void RunAction()
        { 
            Managers.Game.AddItem(itemName,amount);
        }
    }
