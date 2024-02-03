
    public class AllCharacterSetStatusAction : TriggerAction
    {
        private Define.SetStatusAction calculate;
        private Define.CharacterStatus status;
        private float value;
        public override void RunAction()
        {
            Managers.Game.AllCharacterStatusCalculate(status,calculate,value);
        }
    }
