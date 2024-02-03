
    public class SetActiveShowCharacterStatusAction : TriggerAction
    {
        public bool active;
        public override void RunAction()
        {
            Managers.Game.ActiveShowCharacterStatus(active);
        }
    }
