using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public IScene GetCurrentScene { get; private set; }
    public void SetCurrentScene(IScene scene)=> GetCurrentScene = scene; 
     
    public void LoadScene(Define.Scene changeScene)
    {
        GetCurrentScene.SceneLoad(() =>
        {
            //managerClear랑 Destory가 필요한지는 좀 더 분석 후 넣겠음
            SceneManager.LoadScene(convertSceneName(changeScene));
        });
    }

    
    private string convertSceneName(Define.Scene type)
    {
        return System.Enum.GetName(typeof(Define.Scene), type);
    }


}