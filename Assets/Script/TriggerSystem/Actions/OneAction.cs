
/// <summary>
/// 단 한 번만 실행하고 해당 트리거를 삭제합니다. id에는 Trigger Id를 넣습니다.
/// </summary>
    public class OneAction : TriggerAction
    {
        public int id;
        public override void RunAction()
        {
           var trigger = Managers.Game.GetTriggerEvent;
           trigger.RemoveTrigger(id);
        }
    }
