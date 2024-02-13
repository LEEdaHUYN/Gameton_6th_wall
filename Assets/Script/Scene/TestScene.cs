
using UnityEngine;

public class TestScene : BaseScene
{
   public GameObject characterListParent;
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_TestScene>(this.gameObject);
    }

  
}
