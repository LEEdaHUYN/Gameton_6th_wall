
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Title : UI_Scene
{
    enum GameObjects
    {
        Title,
      //  LoadingSlider,
    }

    enum Buttons
    {
        //StartButton
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        PreResourceLoad();
        //TODO : BIND End StartButton Event
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        //GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        //{
        //    if (isLogin)
        //    {
        //        Managers.Scene.LoadScene(Define.Scene.ShipScene);
        //    }
        //});
        return true;
    }

    #region ResourceLoad
    private bool isPreload = false;
    private bool isLogin = false;

    private void PreResourceLoad()
    {
        Managers.Resource.LoadAllAsync<Object>(Define.ResourceLabel.PreLoad.ToString(), (key, count, totalCount) =>
        {
            if (totalCount == count)
            {
                
                Managers.Sound.Init(()=>
                {
                    Managers.Sound.MusicVolume = 0.35f;
                    Managers.Sound.PlayBGM("TitleBGM");
                });
                Managers.Back.OnLogin(() =>
                {
                    isLogin = true;
                });
                isPreload = true;
            }
        });
    }

    #endregion

    
}
