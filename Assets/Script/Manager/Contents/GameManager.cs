
using System.Collections.Generic;
using System.Linq;
using Script.Scene.Game;
using Script.TriggerSystem;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    #region  Inventory
    
    private Inventory _inventory = new Inventory();

    public List<Item> GetInventoryList()
    {
        return _inventory.GetItemList.Values.ToList();
    }

    public Item GetFindByItemName(string name)
    {
        return _inventory.FindByItemName(name);
    }

    public void AddItem(Item item,float amount = 0)
    {
        _inventory.AddCountableItem(item,amount);

    }

    public void RemoveItem(string itemName)
    {
        Managers.Resource.Load<Item>(itemName, (success) =>
        {
            if (!_inventory.FindByItemName(success.GetName))
                return;
            
            AddTextNote($"-{success.GetItemIconIndex} {itemName} 을 전부 잃어버렸어!");
            _inventory.RemoveItem(itemName);
        });
    }
    public void SubItem(string itemName, float amount = 0)
    {
        Managers.Resource.Load<Item>(itemName, (success) =>
        {
            AddTextNote($"-{success.GetItemIconIndex} {itemName}을 {amount}만큼 잃었어!");
            _inventory.SubItem(success,amount);
        });
    }

    public void AddItem(string itemName, float amount = 0, bool isShowText = true)
    {
        Managers.Resource.Load<Item>(itemName, (success) =>
        {
            if (isShowText)
            {
                AddTextNote($"+{success.GetItemIconIndex} {itemName} 을 얻었어!");
            }
            _inventory.AddCountableItem(success,amount);
        });
    }

    public void UseItem(Item item, Character character, float amount = 0)
    {
       _inventory.UseCountableItem(item,amount,character);
    }
    
    
    public bool CheckHaveItem(string itemName, int amount)
    {

        var item = GetFindByItemName(itemName);
        if(item == null)
            return false;

        return item.GetAmount() >= amount;
    }

    #endregion

    #region Day

    public int CurrentDay { get; private set; } = 0;
    private TriggerEvent _triggerEvent = null;

    public TriggerEvent GetTriggerEvent => _triggerEvent;
    public void NextDay()
    {
        if (_triggerEvent == null)
        {
            GameObject triggerEventObject = new GameObject();
            _triggerEvent = triggerEventObject.GetOrAddComponent<TriggerEvent>();
        }
        //ShowCharacterStatus.
      
        CurrentDay++;
        FadeInOut();
        AbilityRun();
        CharacterNextDayStatus();
        if (isGameOver)
        {
            AddTextNote("Game Over....");
            EndNote();
            return;
        }
        _triggerEvent.StartTrigger(()=>
        {
            ShowCharacterText();
            EndNote();
        });
        Managers.Back.AddCurrency(80,Define.Coin);
    }

    private void CharacterNextDayStatus()
    {
        var deathCharacter = new List<Character>();
        foreach (var character in Characters)
        {
            character.NextDayStatus();
            if(character.GetIsAlive == false)
                deathCharacter.Add(character);
        }

        foreach (var character in deathCharacter)
        {
            if (character.isPlayer)
            {
                isGameOver = true;
            }
            DeathCharacter(character);
        }
    }

    #endregion


    public bool isGameOver { get; private set; } = false;
    public int CurrentDay_int=0;
    
    #region Book

    private Book _book;

    public Book Book
    {
        get => _book;
        set => _book = value;
    }

    //TODO NOTE System

    private void EndNote()
    {
        Book.EndText();
    }


    public void AddTextNote(string text)
    {
        Book.AddText(text+"\n");
       // Note.AddCurrentPageText(text);
    }

    #endregion

    #region CharacterText

    public List<Character> Characters{get; private set; } = new List<Character>();
    private bool _activeShowStatus;
    public Character SelectCharacter { get; set; }
    public void AddCharacter(Character character)
    {
        Characters.Add(character);
    }

    private void InitCharacterText()
    {
        foreach (var character in Characters)
        {
            character.StatusText.Clear();
            character.DisplayStatusText.Clear();
        }
    }

    private void ShowCharacterText()
    {
        InitCharacterText();
        CheckStatus();
        if (_activeShowStatus)
        {
            foreach (var character in Characters)
            {
                foreach (var statusText in character.StatusText)
                {
                    AddTextNote(statusText);
                }
            }
        }
    }



    private void CheckStatus()
    {
        foreach (var status in Managers.Data.CharacterStatusDatas)
        {
            var checkStatus = status.Value.status;
            var checkMinValue = status.Value.minValue;
            var checkMaxValue = status.Value.maxValue;
            var noteText = status.Value.text;
            var displayText = status.Value.display;
            foreach (var character in Characters)
            {
                if (Utils.InRange(character.GetStatusValue(checkStatus),checkMinValue,checkMaxValue) )
                {
                    character.StatusText.Add(character.GetName() + ": " + noteText);
                    character.DisplayStatusText.Add(displayText);

                }
                
            }
        }
    }

    public void ActiveShowCharacterStatus(bool active)
    {
        _activeShowStatus = active;
    }
    #endregion

    public void AllCharacterStatusCalculate(Define.CharacterStatus status, Define.SetStatusAction calculate, float value)
    {
        foreach (var character in Characters)
        {
            float calculateValue = character.GetStatusValue(status);
            switch (calculate)
            {
                case Define.SetStatusAction.Add:
                    calculateValue += value;
                    break;
                case Define.SetStatusAction.Mod:
                    calculateValue = value;
                    break;
                case Define.SetStatusAction.Sub:
                    calculateValue -= value;
                    break;
            }

            character.SetStatusValue(status, calculateValue);
        }
    }

    public void SelectCharacterStatusCalculate(Define.CharacterStatus status, Define.SetStatusAction calculate,
        float value)
    {
        float calculateValue = SelectCharacter.GetStatusValue(status);
        switch (calculate)
        {
            case Define.SetStatusAction.Add:
                calculateValue += value;
                break;
            case Define.SetStatusAction.Mod:
                calculateValue = value;
                break;
            case Define.SetStatusAction.Sub:
                calculateValue -= value;
                break;
        }
        SelectCharacter.SetStatusValue(status,calculateValue);
        SelectCharacter = null;
    }

    public void CharacterStatusCalculate(Character character, Define.CharacterStatus status,
        Define.SetStatusAction calculate, float value)
    {
        float calculateValue = character.GetStatusValue(status);
        switch (calculate)
        {
            case Define.SetStatusAction.Add:
                calculateValue += value;
                break;
            case Define.SetStatusAction.Mod:
                calculateValue = value;
                break;
            case Define.SetStatusAction.Sub:
                calculateValue -= value;
                break;
        }

        character.SetStatusValue(status, calculateValue);
    }
 

    #region Flag

    public List<Flag> FlagList { get; private set; } = new List<Flag>();
    public bool CheckFlag(Flag flag)
    {
        Flag findFlag = FlagList.Find(f => f.name == flag.name);

        if (findFlag == null && flag.value == 0)
        {
            FlagList.Add(flag);
            return true;
        }
            
        
        if (findFlag == null )
            return false;

        return findFlag.value == flag.value;
    }
    
    public void SetFlag(Flag flag)
    {
        Flag findFlag = FlagList.Find(f => f.name == flag.name);
        if (findFlag == null)
        {
            FlagList.Add(flag);
        }
        else
        {
            findFlag.value = flag.value;
        }
        
    }
    
    public void ShowYesOrNoAction(string text, Flag yesFlag, Flag noFlag,string image)
    {
        _book.AddYesOrNoBox(text,yesFlag,noFlag,image);
    }

    public void ShowItemChoice(string text, List<ItemFlag> itemFlagList,string image)
    {
         _book.AddItemChoiceBox(text, itemFlagList,image);
    }

    public void ShowPictureAndText(string text, string image)
    {
        _book.AddPictureTextBox(text,image);
    }
    #endregion
    #region FadeInOut

    private UI_Fade _fadeUI;

    public void SetFadeUI(UI_Fade fade)
    { 
        _fadeUI = fade;
    }
    private void FadeInOut(float duration = 1.0f)
    {
        _fadeUI.FadeIn(CurrentDay,duration);
    }

    #endregion


    #region InGameSprite

    private GameBackGround _backGround;

    public void SetBackGround(GameBackGround back)
    {
        _backGround = back;
    }
    public void BackGroundChange(string spriteName)
    {

        Managers.Resource.Load<Sprite>(spriteName, (success) =>
        {
            _backGround.SetBackGround(success);
        });
        
    }
    

    #endregion


    #region UICanvas Controller

    private GameObject _uiCanvas;
    public void SetUiCanvas(GameObject gameObject) => _uiCanvas = gameObject;


    public void CloseUiCanvas()
    {
        _uiCanvas.SetActive(false);
    }

    public void OnUiCanvas()
    {
        _uiCanvas.SetActive(true);
    }
    
    #endregion

    public void DeathCharacter(Character character)
    {
        Characters.Remove(character);
    }

    public void GameOver()
    {
        isGameOver = false;
        CurrentDay_int = CurrentDay;
        CurrentDay = 0;
        _inventory.ClearInventory();
        Characters.Clear();
        FlagList.Clear();
        _endingList.Clear();
        _abilityList.Clear();
        _time = 60f;
        Managers.Scene.LoadScene(Define.Scene.TitleScene);
    }

 

    public EndingPage EndingPage;
    private List<EndingStruct> _endingList = new List<EndingStruct>();

    public List<EndingStruct> GetEndingList => _endingList;
    public void AddEnding(string text, string imageName)
    {
        Managers.Resource.Load<Sprite>(imageName, (success) =>
        {
            EndingStruct ending;
            ending.text = text;
            ending.sprite = success;
            _endingList.Add(ending);
        });
    }

    public void ShowEnding()
    {
        EndingPage.gameObject.SetActive(true);
        EndingPage.ShowEnding();
     
    }


    private List<IAbilityRun> _abilityList = new List<IAbilityRun>();

    public void AddAbility(IAbilityRun ability)
    {
        _abilityList.Add(ability);
    }
    
    private void AbilityRun()
    {
        foreach (var ability in _abilityList)
        {
            ability.RunAbility();
        }
    }

    public void EquipSkillUpdate()
    {
        var skillInventory = Managers.Back.GetClientUserInventory;
        foreach (var skill in skillInventory)
        {
            if (skill.CustomData == null)
            {
                continue;
            } 
            
            if (skill.CustomData["equip"] == "true")
            {
                Managers.Resource.Load<ScriptableObject>(skill.ItemId, (success) =>
                {
                    var ability = (IAbilityRun)success;
                    AddAbility(ability);
                });
            }
        }
    }

    private float _time = 60f;
    public float GetTime => _time;
    public void SetTime(float time)
    {
        _time = time;
    }

}
