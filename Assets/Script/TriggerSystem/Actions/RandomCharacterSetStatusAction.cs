
    public class RandomCharacterSetStatusAction : TriggerAction
    {        
        public Define.SetStatusAction calculate;
        public Define.CharacterStatus status;
        public float value;
        public override void RunAction()
        {
            Managers.Game.RandomCharacterStatusCalculate(status,calculate,value);
        }
    }
