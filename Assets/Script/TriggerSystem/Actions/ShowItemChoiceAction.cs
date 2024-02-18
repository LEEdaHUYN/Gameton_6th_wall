
    using System.Collections.Generic;
    using Script.TriggerSystem;
    using UnityEngine;

    public class ShowItemChoiceAction : TriggerAction
    {
        public string text;
        public List<ItemFlag> itemFlagList = new List<ItemFlag>();
        public string imageName;
        public override void RunAction()
        {
            Managers.Game.ShowItemChoice(text, itemFlagList,imageName);
        }
        
    }
