using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScene : BaseScene
{
    protected override UI_Scene createUIScene()
    {
        return Utils.GetOrAddComponent<UI_AnimationScene>(this.gameObject);
    }

}
