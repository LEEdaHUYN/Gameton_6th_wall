
using Unity.VisualScripting;
using UnityEngine;

public class UI_Title : UI_Scene
{
    enum GameObjects
    {
        Title,
      //  LoadingSlider,
    }

    enum Buttons
    {
        StartButton
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        //TODO : BIND End StartButton Event
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            if (isPreload)
            {
                //Managers.Scene.LoadScene(Define.Scene.LobbyScene);
                //// 씬 이동 할 필요없이 버튼만 추가하면 될 듯?
                /// TEST
            }
        });
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
