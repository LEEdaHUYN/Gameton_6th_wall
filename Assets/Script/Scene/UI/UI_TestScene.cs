
using Cysharp.Threading.Tasks;
using Script.Manager.Contents;
using UnityEngine;

public class UI_TestScene :UI_Scene
{
    private TestScene _scene;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        Managers.Game.SetUiCanvas(this.gameObject);

        _scene = GetComponent<TestScene>();
        
        PreResourceLoad().Forget();
        
        return true;
    }

    #region ResourceLoad
    private bool isPreload = false;

    private async UniTaskVoid PreResourceLoad()
    {
        //bool isLogin = false;
        //if (!Managers.Back.IsLogin)
        //{
        //    Managers.Back.OnLogin(() =>
        //    {
        //        isLogin = true;
        //    });          
        //}

        //await UniTask.WaitUntil(() =>
        //{
        //    return isLogin;
        //});
        Managers.Ad.LoadAdBanner();
        Managers.Resource.LoadAllAsync<Object>(Define.ResourceLabel.PreLoad.ToString(), (key, count, totalCount) =>
        {
            if (totalCount == count)
            {
                Managers.Sound.PlayBGM("MainBGM");
                isPreload = true;

                string[] characterNames = { "제임스","에릭", "케빈", "브라이언" };
                for (int i= 1; i < 5; i++)
                {
                    var i2 = i;
                    Managers.Resource.Load<GameObject>("Character", (success) =>
                    {
                        Character character = Object.Instantiate(success,_scene.characterListParent.transform).GetComponent<Character>();
                        character.SetName(characterNames[i2-1]);// 이름은 0번부터
                        var i1 = i2; //for문 값을 i1에 집어넣음
                        Managers.Resource.Load<Sprite>($"character{i2}.sprite", success =>
                        {
                            character.SetSprite(success);
                        });
                        
                        Managers.Resource.Load<Sprite>($"{i2}.sprite", (spriteSuccess) =>
                        {
                            character.SetPortrait(spriteSuccess);
                            //임시 주인공 설정 1로 해놨습니다
                            if (i1 == 1)
                                character.isPlayer = true;
                        });
                        Managers.Game.AddCharacter(character);
                    
                    });
                }
                Managers.Resource.Load<GameObject>("Book", (success) =>
                {
                     Managers.Game.Book = Object.Instantiate(success, this.transform).GetComponent<Book>();
                    Managers.Data.Init();
                    Managers.Game.NextDay();
                });

                Managers.Resource.Load<Item>("Hand", (success) =>
                {
                    Managers.Game.AddItem(success,1);
                });
               
            
                Managers.Back.GetUserInventory(() =>
                {
                    Managers.Game.EquipSkillUpdate();
                });
                
            }
            
        });
    }

    #endregion

}
