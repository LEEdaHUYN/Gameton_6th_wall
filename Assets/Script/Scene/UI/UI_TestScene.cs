
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

                string[] characterNames = { "소피아", "제이슨", "피터" };
                for (int i= 1; i < 4; i++)
                {
                 
                    Managers.Resource.Load<GameObject>("Character", (success) =>
                    {
                        Character character = Object.Instantiate(success).GetComponent<Character>();
                        character.SetName(characterNames[i-1]);
                        Managers.Resource.Load<Sprite>($"{i}.sprite", (spriteSuccess) =>
                        {
                            character.SetPortrait(spriteSuccess);
                        });
                        Managers.Game.AddCharacter(character);
                    
                    });   
                }
                //
                // CanFood eatItemFood = new CanFood();
                // Managers.Game.AddItem(eatItemFood,10);
                // Water water = new Water();
                // Managers.Game.AddItem(water,0.1f);
                
                Managers.Resource.Load<GameObject>("Book", (success) =>
                {
                     Managers.Game.Book = Object.Instantiate(success, this.transform).GetComponent<Book>();
                    Managers.Data.Init();
                    Managers.Game.NextDay();
                });
            }
            
        });
    }

    #endregion

}
