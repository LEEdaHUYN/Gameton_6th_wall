namespace Script.TriggerSystem.Actions
{
    public class SetBackGroundAction : TriggerAction
    {
        public string imageName;
        public override void RunAction()
        {
            Managers.Game.BackGroundChange(imageName);
        }
    }
}