
    public class ShowPictureAndTextAction : TriggerAction
    {
        public string text;
        public string imageNmae;
        public override void RunAction()
        {
            Managers.Game.ShowPictureAndText(text,imageNmae);
        }
    }
