
public class TestScene : BaseScene
{
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_TestScene>(this.gameObject);
    }

    private void Start()
    {
        Managers.Sound.PlayBGM("MainBGM");
    }
}
