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
   
   private ItemInstance _itemInstance;

   public string GetItemInstanceId => _itemInstance.ItemInstanceId;

   private Dictionary<string, string> _itemCustomData = new();

   private CharSkillSelect _selector;

   private Image _buttonColorImage;
   
   UnityAction clickAction;
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
       _buttonColorImage = Utils.GetOrAddComponent<Image>(this.gameObject);
       _button = Utils.GetOrAddComponent<Button>(this.gameObject);
       if (_isHave)
       {
           SetColor();
           clickAction = SelectSkill;
       }
       else
       {
           _buttonColorImage.color = Color.red;
           clickAction = PurchaseSkill;
       }
       _button.onClick.AddListener(clickAction);
   }

   public void SelectSkill()
   {
       SetColor();
       _selector.SelectSkill(this);

   }

   void SetColor()
   {
       _buttonColorImage.color = _isSelected ? Color.yellow : Color.white;
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
         _isHave = true;
         SetColor();
         _button.onClick.RemoveAllListeners();
         clickAction = SelectSkill;
         _button.onClick.AddListener(clickAction);
         _itemInstance = Managers.Back.GetItem(_skillId);
         Debug.Log("아이템 구매 ");
     }, () =>
     {
         //TODO
     },"SkillShop");  

   }

}
