using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkilItem : MonoBehaviour
{

   private Button _button;
   
   [SerializeField]
   private TMP_Text _text;

   private string _skillId;

   private uint _cost;

   private bool _isHave;

   [SerializeField]
   private bool _isSelected;

   private ColorBlock _buttonColorBlock;

   private ItemInstance _itemInstance;

   private Dictionary<string, string> _itemCustomData = new();

   private CharSkillSelect _selector;
   public void Init( string id, string text, uint cost,ItemInstance itemInstance,CharSkillSelect selector)
   {

      this._text.text = text;
      this._skillId = id;
      this._cost = cost;
      this._itemInstance = itemInstance;
      this._isHave = false;
      this._isSelected = false;
      _selector = selector;
      ItemInit();
      ButtonInit();
   }

   private void ItemInit(){
       if (_itemInstance == null)
       {
           return;
       }
       _itemCustomData = _itemInstance.CustomData;
       if (_itemCustomData["equip"] == "true")
       {
           _isSelected = true;
       }
       _isHave = true;

   }
   private void ButtonInit()
   {
       _button = Utils.GetOrAddComponent<Button>(this.gameObject);
       _buttonColorBlock = _button.colors;

       UnityAction clickAction;
       if (_isHave)
       {
           _buttonColorBlock.normalColor = _isSelected ? Color.yellow : Color.white;
           clickAction = SelectSkill;
       }
       else
       {
           _buttonColorBlock.normalColor = Color.red;
           clickAction = PurchaseSkill;
       }
       _button.colors = _buttonColorBlock;
       _button.onClick.AddListener(clickAction);
   }

   public void SelectSkill()
   {
       //TODO GameManager Noti
       SetColor();
       _selector.SelectSkill(this);

   }

   void SetColor()
   {
       if (_isSelected)
       {
           _buttonColorBlock.selectedColor = Color.yellow;
       }
       else
       {
           _buttonColorBlock.selectedColor = Color.white;
       }
       
       _button.colors = _buttonColorBlock;
   }
   
   
   public void SetChangeSelect(bool select)
   {
       _isSelected = select;
       SetColor();
   }
   private void PurchaseSkill()
   {
     Managers.Back.PurchaseItem(_skillId,(int)_cost,Define.Diamond, () =>
     {
         _isSelected = false;
         SetColor();
         Debug.Log("아이템 구매 ");
     }, () =>
     {
         //TODO
     },"SkillShop");  

   }

}
