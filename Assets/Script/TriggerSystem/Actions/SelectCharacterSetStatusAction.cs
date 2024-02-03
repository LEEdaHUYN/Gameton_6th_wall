
    public class SelectCharacterSetStatusAction : TriggerAction
    {   
        private Define.SetStatusAction calculate;
        private Define.CharacterStatus status;
        private float value;
        public override void RunAction()
        {
           Managers.Game.SelectCharacterStatusCalculate(status,calculate,value);
        }
    }
