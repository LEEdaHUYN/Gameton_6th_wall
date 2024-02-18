    using System;
    using System.Collections.Generic;
    using Script.TriggerSystem;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Object = UnityEngine.Object;

    public class ItemChoiceSelector : Selector
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _content;
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _defalutSprite;
        private List<ItemBox> _itemBoxList = new List<ItemBox>();
        private Action _nextPageAction;
        private List<ItemFlag> _itemFlags = new List<ItemFlag>();
        public void Init(string text, List<ItemFlag> itemFlagList, Action nextPage, string image = null)
        {
            _nextPageAction = nextPage;
            _text.text = text;
            _itemFlags = itemFlagList;
            if (image == null)
            {
                _image.sprite = _defalutSprite;
            }
            else
            {
                Managers.Resource.Load<Sprite>(image, (success) =>
                {
                    _image.sprite = success;
                });
            }
        }

        private void CreateItemBox(Item item,Flag flagAction,bool haveItem)
        {
            Managers.Resource.Load<GameObject>("itemList", (success) =>
            {
               var itemBox = Object.Instantiate(success, _content.transform).GetComponent<ItemBox>();
               itemBox.Init(item.GetSprite,_nextPageAction,flagAction,haveItem);
                _itemBoxList.Add(itemBox);
            });
        }
        public override void ShowCurrentDay()
        {
            foreach (var itemFlag in _itemFlags)
            {
                bool haveItem = false;
                var item = Managers.Game.GetFindByItemName(itemFlag.itemName);
                if (item != null)
                {
                    haveItem = item.GetAmount() > 0;
                }
                Managers.Resource.Load<Item>(itemFlag.itemName, (success) =>
                {
                    item = success;
                    CreateItemBox(item,itemFlag.flag,haveItem);
                });
            }
        }

        public override void NextDay()
        {
            foreach (var itemBox in _itemBoxList)
            {
                Destroy(itemBox.gameObject);
            }
            _itemBoxList.Clear();
            // foreach (var itemBox in _itemBoxList)
            // {
            //     var itemBoxObject = itemBox;
            //     _itemBoxList.Remove(itemBox);
            //     Destroy(itemBoxObject);
            // }
        }


    }
