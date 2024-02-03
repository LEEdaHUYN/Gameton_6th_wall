using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnimationScene : UI_Scene
{
    public override void SceneChange()
    {
        Managers.Scene.LoadScene(Define.Scene.GameScene); 
    }
}
