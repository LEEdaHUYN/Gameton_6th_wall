
using Script.Manager.Contents;
using UnityEngine;

public class UI_TestScene :UI_Scene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        Managers.Game.SetUiCanvas(this.gameObject);
        PreResourceLoad();
        for (int i = 0; i< Managers.Game.GetInventoryList().Count;i++)
        {
            Debug.Log(Managers.Game.GetInventoryList()[i].GetName);
      
        }
        return true;
    }

    #region ResourceLoad
    private bool isPreload = false;

    private void PreResourceLoad()
    {
        Managers.Resource.LoadAllAsync<Object>(Define.ResourceLabel.PreLoad.ToString(), (key, count, totalCount) =>
        {
            Debug.Log("Load");
            if (totalCount == count)
            {
                Debug.Log("end");
                isPreload = true;

                string[] characterNames = { "민영", "감자", "주호" };
                for (int i= 1; i < 4; i++)
                {
                    var i2 = i;
                    Managers.Resource.Load<GameObject>("Character", (success) =>
                    {
                        Character character = Object.Instantiate(success).GetComponent<Character>();
                        character.SetName(characterNames[i2-1]);
                        var i1 = i2;
                        Managers.Resource.Load<Sprite>($"{i2}.sprite", (spriteSuccess) =>
                        {
                            character.SetPortrait(spriteSuccess);
                            //임시 주인공 설정 
                            if (i1 == 2)
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
                    Managers.Game.AddItem(success);
                });
            }
            
        });
    }

    #endregion

}
