
// Flag를 추가 또는 값을 변경합니다.
    public class SetFlag : TriggerAction
    {
        public Flag flag;
        public override void RunAction()
        {
            Managers.Game.SetFlag(flag);
        }
    }
