
    public class RandomCharacterSetStatusAction : TriggerAction
    {        
        private Define.SetStatusAction calculate;
        private Define.CharacterStatus status;
        private float value;
        public override void RunAction()
        {
            Managers.Game.RandomCharacterStatusCalculate(status,calculate,value);
        }
    }
