
    public class ShowYesOrNoAction : TriggerAction
    {
        public string text;
        public Flag yesFlag;
        public Flag noFlag;
        public override void RunAction()
        {
            Managers.Game.ShowYesOrNoAction(text, yesFlag, noFlag);
        }
    }
