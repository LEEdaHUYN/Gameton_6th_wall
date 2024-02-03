using System;
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


    public class UI_FoodBar : SerializedMonoBehaviour,IFoodDistribute
    {
        #region  property

        [SerializeField]
        private TextMeshProUGUI _overFoodText;

        [SerializeField] private string _itemName;

        [SerializeField] private float _clickAmount;

        public float GetClickAmount => _clickAmount;
        [SerializeField] private FoodType _foodType;

        FoodType IFoodDistribute.GetFoodType() => _foodType;
        public FoodType GetFoodType => _foodType; 
        
        private float _currentFood = 0f;

        private Image _image;

        private const float MaxValue = 6.0f;

        private Button _button;

        #endregion


        private void Awake()
        {
            _image = GetComponent<Image>();
            this.ObserveEveryValueChanged(_=> _currentFood).Subscribe(x => ShowAmountUpdate());
            _button = Utils.GetOrAddComponent<Button>(this.gameObject);
            _button.onClick.AddListener(() =>
            {
                if(_isDistribute)
                    OnBackIn();
                else
                    OnDistribute();
            });
        }

        private void SetCurrentAmount()
        {
            if (Managers.Game.GetFindByItemName(_itemName) is Item item)
            {
                _currentFood = item.GetAmount();
            }
       
        }

        public void NextDay()
        {
            SetCurrentAmount();
        }
      
        private void ShowAmountUpdate()
        {
            _image.fillAmount = _currentFood / MaxValue;
        }



  
        #region Toggle
        public bool CheckFoodDistribute()
        {
            return _currentFood - _clickAmount >= 0;
        }
        public void CharacterFoodDistribute()
        {
            _currentFood -= _clickAmount;
        }

        public void CharacterFoodBackIn()
        {
            _currentFood += _clickAmount;
        }

        private List<Toggle> _toggles = new List<Toggle>();
        public void AddCharacterInfo(Toggle toggle)
        {
            _toggles.Add(toggle);
        }

        private bool _isDistribute = false;
        private void OnDistribute()
        {
            var onToggles = _toggles.FindAll(x => !x.isOn);
           
            if (onToggles.Count * _clickAmount > _currentFood)
                return;
            
            foreach (var t in onToggles)
            {
                t.isOn = true;
            }

            _isDistribute = true;
        }

        private void OnBackIn()
        {
            var onToggles = _toggles.FindAll(x => x.isOn);
            Debug.Log($"{onToggles.Count()} Count!!");
            foreach (var t in onToggles)
            {
                t.isOn = false;
            }

            _isDistribute = false;
        }
        #endregion
  
    }
