
public class ShowTextAction : TriggerAction
{
    public string text;
    public override void RunAction()
    {
        Managers.Game.AddTextNote(text);
    }
}
