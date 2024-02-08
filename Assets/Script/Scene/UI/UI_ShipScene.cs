using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ShipScene : UI_Scene
{
    public override void SceneChange()
    {
        Managers.Scene.LoadScene(Define.Scene.AnimationScene);
    }

    public void Start()
    {
        Managers.Sound.PlayBGM("ShipBGM");
    }
}
