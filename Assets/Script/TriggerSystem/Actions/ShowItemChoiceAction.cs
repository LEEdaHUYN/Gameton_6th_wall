
    using System.Collections.Generic;
    using Script.TriggerSystem;

    public class ShowItemChoiceAction : TriggerAction
    {
        public string text;
        public List<ItemFlag> itemFlagList = new List<ItemFlag>();
        public override void RunAction()
        {
            Managers.Game.ShowItemChoice(text, itemFlagList);
        }
    }
