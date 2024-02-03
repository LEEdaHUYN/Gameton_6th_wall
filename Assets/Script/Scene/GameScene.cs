using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_GameScene>(this.gameObject);
    }
}
