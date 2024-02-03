
public class TestScene : BaseScene
{
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_TestScene>(this.gameObject);
    }
}
