
using System;

public class TitleScene : BaseScene
{
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_Title>(this.gameObject);
    }
}
