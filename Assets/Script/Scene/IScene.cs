using System;
using UnityEngine;

public interface IScene {
    public void SceneLoad(Action callBack = null);
    public UI_Scene GetUIScene();
}