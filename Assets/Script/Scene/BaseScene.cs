using System;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour, IScene
{
    protected UI_Scene _uiScene;

    protected abstract UI_Scene createUIScene();
    protected virtual void Init()
    {
        _uiScene = createUIScene();
    }
    private void Start()
    {
        Init();
    }

    public void SceneLoad(Action callBack = null)
    {
        callBack?.Invoke();
    }

    public UI_Scene GetUIScene()
    {
        return _uiScene;
    }
}