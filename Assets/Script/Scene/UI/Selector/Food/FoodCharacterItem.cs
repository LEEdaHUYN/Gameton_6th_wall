﻿using System;
using System.Collections.Generic;
using System.Linq;
using Script.Scene.UI.Selector.Food;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FoodCharacterItem : SerializedMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _displayStatus;

        [SerializeField] private PersonInfo _personInfo;

        private List<IFoodDistribute> _foodBarDistributes = new List<IFoodDistribute>();

        [SerializeField] private Dictionary<FoodType, Toggle> _foodToggles = new Dictionary<FoodType, Toggle>();

        [SerializeField] private Button _handButton;
        
        private Item _selectItem = null;

        private List<Item> _inventoryList;
        public void SetInventory(List<Item> inventory)
        {
            _inventoryList = inventory;
        }


        private const int Hand = 0;
        private int _selectCountIdx = Hand;
        private void OnSelectItem()
        {
            if(_selectCountIdx != Hand)
                _selectItem.SetAmount(_selectItem.GetAmount() + 1);

            while (true)
            {
                _selectCountIdx++;
                if (_selectCountIdx >= _inventoryList.Count)
                {
                    _selectCountIdx = Hand;
                    break;
                }

                if (_inventoryList[_selectCountIdx].GetAmount() > 0)
                {
                    _selectItem = _inventoryList[_selectCountIdx];
                    _inventoryList[_selectCountIdx].SetAmount(_inventoryList[_selectCountIdx].GetAmount() - 1);
                    break;
                }
            }
            
            _selectItem = _inventoryList[_selectCountIdx];
            _handButton.GetComponent<Image>().sprite = _selectItem.GetSprite;


        }
        
        //TODO Item Image Show
        //isEatFood isEatWater
        public bool GetIsEat(FoodType foodType)
        {
            return _foodToggles[foodType].isOn;
        }

        //TODO UI_FoodBar All Distic.. And BackIn 
        public void SetEatToggle(FoodType foodType,bool toggle)
        {
            _foodToggles[foodType].isOn = toggle;
        }
        private void Start()
        {
            foreach (var (foodType, value) in _foodToggles)
            {
                value.onValueChanged.AddListener(_=>
                {
                    OnFoodToggle(foodType,value);
                });
                FoodBarMapping(foodType,value);
                
                this.ObserveEveryValueChanged(_ => _foodToggles[foodType].isOn)
                    .Subscribe(_ =>
                    {
                        float alpha = _foodToggles[foodType].isOn ? 5 : 1;
                        var block = _foodToggles[foodType].colors;
                        block.colorMultiplier = alpha;
                        _foodToggles[foodType].colors = block;
                    });
            }
          
            _handButton.onClick.AddListener(OnSelectItem);
        }


        private void FoodBarMapping(FoodType foodType, Toggle toggle)
        {
            var foodDistribute = _foodBarDistributes.Find(x => x.GetFoodType() == foodType);
            foodDistribute.AddCharacterInfo(toggle);
        }

        public bool isNextDay = false;
        public bool isChoice = false;
        private void OnFoodToggle(FoodType foodType, Toggle toggle)
        {
            if (isNextDay)
            {
                isNextDay = false;
                isChoice = false;
                return;
            }
               
            var foodDistribute = _foodBarDistributes.Find(x => x.GetFoodType() == foodType);

         
            if (foodDistribute.CheckFoodDistribute())
            {
                if (toggle.isOn)
                {
                    foodDistribute.CharacterFoodDistribute();
                    isChoice = true;
                    return;
                }
            }
            
            if (!toggle.isOn && isChoice)
            {
                foodDistribute.CharacterFoodBackIn();
                isChoice = false;
                return;
            }
          
            toggle.isOn = false;
            
        }
        public void SetFoodBarDistribute(List<IFoodDistribute> foodDistributes)
        {
            _foodBarDistributes = foodDistributes;
        }
        
        public void SetDisplayStatusText(string text)
        {
            _displayStatus.text = text;
        }

        public void SetPersonInfoSprite(Sprite sprite)
        {
            _personInfo.SetPersonInfo(sprite);
        }

    }
