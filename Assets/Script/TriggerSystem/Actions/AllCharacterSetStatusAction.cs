
    public class AllCharacterSetStatusAction : TriggerAction
    {
        public Define.SetStatusAction calculate;
        public Define.CharacterStatus status;
        public float value;
        public override void RunAction()
        {
            Managers.Game.AllCharacterStatusCalculate(status,calculate,value);
        }
    }
