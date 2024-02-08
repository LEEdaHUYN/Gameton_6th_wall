using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Scene.UI.Selector.Food;
using UnityEngine;
using Object = UnityEngine.Object;

    public class FoodSelector : Selector
    {
        private Dictionary<Character,FoodCharacterItem> _characterList = new Dictionary<Character, FoodCharacterItem>();
        [SerializeField] private List<UI_FoodBar> _foodBars = new List<UI_FoodBar>();


        
        [SerializeField]
        private GameObject _characterContent;
        private void Start()
        {
            foreach (var character in Managers.Game.Characters)
            {
                Managers.Resource.Load<GameObject>("FoodSelector", (success) =>
                {
                    var infoItem = Object.Instantiate(success, _characterContent.transform).GetComponent<FoodCharacterItem>();
                    infoItem.SetFoodBarDistribute(_foodBars.Select(bar => (IFoodDistribute)bar).ToList());
                    infoItem.SetPersonInfoSprite(character.GetCharacterPortrait);
                    infoItem.SetInventory(_itemInventory);
                     _characterList.Add(character,infoItem);
                });
            }

        }

        public override void NextDay()
        {
               //TODO FOOD 정산, Character Status 정산 ect ..
                FoodAdjustment();
                ItemAdjustment();
        }

        private void ItemAdjustment()
        {
            foreach (var character in _characterList)
            {
                Item useItem = character.Value.GetSelectItem;
                if ( useItem == null||useItem.GetName == "Hand")
                    continue;
                Managers.Game.UseItem(useItem,character.Key,1);
                character.Value.InitSelectItem();
            }
        }

        private void FoodAdjustment()
        {
            var canFood = Managers.Game.GetFindByItemName("CanFood");
            var water = Managers.Game.GetFindByItemName("Water");
            var canFoodClickAmount = _foodBars.First(x => x.GetFoodType == FoodType.CanFood).GetClickAmount;
            var waterClickAmount = _foodBars.First(x => x.GetFoodType == FoodType.Water).GetClickAmount;
            foreach (var infoItem in _characterList)
            {
                infoItem.Value.isNextDay = true;
                if(infoItem.Value.GetIsEat(FoodType.CanFood))
                    Managers.Game.UseItem(canFood,infoItem.Key,canFoodClickAmount);
                if(infoItem.Value.GetIsEat(FoodType.Water))
                    Managers.Game.UseItem(water,infoItem.Key,waterClickAmount);

            }
        }

        public override void ShowCurrentDay()
        {
            //TODO 현재 식량 , 현재 캐릭터 상태값 표기.
            var deathCharacter = new List<Character>();
            foreach (var info in _characterList)
            {
                if (!Managers.Game.Characters.Contains(info.Key))
                {
                    info.Value.gameObject.SetActive(false);
                    deathCharacter.Add(info.Key);
                }
                info.Value.SetEatToggle(FoodType.CanFood, false);
                info.Value.SetEatToggle(FoodType.Water, false);
            }
           ShowDisplayStatusText();
           CurrentFoodBarUpdate();
           CurrentInventory();
           foreach (var character in deathCharacter)
           {
               _characterList.Remove(character);
           }
        }

        private List<Item> _itemInventory = new List<Item>();
        private void CurrentInventory()
        {
            _itemInventory.Clear();
            Managers.Resource.Load<Item>("Hand", (success) =>
            {
                _itemInventory.Add(success);
                foreach (var item in  Managers.Game.GetInventoryList().OfType<EatItem>())
                {
                    if (item.GetName is "CanFood" or "Water")
                    {
                        continue;
                    }
                
                    _itemInventory.Add(item);
                }
            });
         
        }

        private void CurrentFoodBarUpdate()
        {
            foreach (var foodBar in _foodBars)
            {
                foodBar.NextDay();
            }
        }
        private void ShowDisplayStatusText()
        {
            foreach (var character in _characterList)
            {
                string text = "";
                foreach (var displayStatus in character.Key.DisplayStatusText)
                {
                    text += displayStatus;
                }
                character.Value.SetDisplayStatusText(text);
            }
        }
    }
