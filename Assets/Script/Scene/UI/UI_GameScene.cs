using Script.Manager.Contents;
using UnityEngine;

public class UI_GameScene : UI_Scene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false; 
        PreResourceLoad();
        //TODO UI 
        return true;
    }
    #region ResourceLoad
    private bool isPreload = false;

    private void PreResourceLoad()
    {
        Managers.Resource.LoadAllAsync<Object>(Define.ResourceLabel.PreLoad.ToString(), (key, count, totalCount) =>
        {
            if (totalCount == count)
            {
                isPreload = true;
            }

        });
    }

    #endregion
}
