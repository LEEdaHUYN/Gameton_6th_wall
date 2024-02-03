
    public class SelectCharacterSetStatusAction : TriggerAction
    {   
        public Define.SetStatusAction calculate;
        public Define.CharacterStatus status;
        public float value;
        public override void RunAction()
        {
           Managers.Game.SelectCharacterStatusCalculate(status,calculate,value);
        }
    }
