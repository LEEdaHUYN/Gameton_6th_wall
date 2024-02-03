using System;
using System.Collections.Generic;
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

        private bool _isAlive = true;

        [SerializeField] private Dictionary<FoodType, Toggle> _foodToggles = new Dictionary<FoodType, Toggle>();
        
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
          
        }


        private void FoodBarMapping(FoodType foodType, Toggle toggle)
        {
            var foodDistribute = _foodBarDistributes.Find(x => x.GetFoodType() == foodType);
            foodDistribute.AddCharacterInfo(toggle);
        }
        private void OnFoodToggle(FoodType foodType, Toggle toggle)
        {  
            var foodDistribute = _foodBarDistributes.Find(x => x.GetFoodType() == foodType);

            if (foodDistribute.CheckFoodDistribute())
            {
                if (toggle.isOn)
                {
                    foodDistribute.CharacterFoodDistribute();
                }
                else 
                {
                    foodDistribute.CharacterFoodBackIn();
                }
            }
            else
            {
                toggle.isOn = false;
            }
      

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

        public void SetAliveCharacterInfo(bool isAlive)
        {
            _isAlive = isAlive;
        }
    }
