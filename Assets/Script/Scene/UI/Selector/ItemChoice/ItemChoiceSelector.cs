    using System;
    using System.Collections.Generic;
    using Script.TriggerSystem;
    using TMPro;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class ItemChoiceSelector : Selector
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _content;
        private List<ItemBox> _itemBoxList = new List<ItemBox>();
        private Action _nextPageAction;
        public void Init(string text, List<ItemFlag> itemFlagList, Action nextPage)
        {
            _nextPageAction = nextPage;
            _text.text = text;
            foreach (var itemFlag in itemFlagList)
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

        private void CreateItemBox(Item item,Flag flagAction,bool haveItem)
        {
            Managers.Resource.Load<GameObject>("ItemBox", (success) =>
            {
               var itemBox = Object.Instantiate(success, _content.transform).GetComponent<ItemBox>();
               itemBox.Init(item.GetSprite,_nextPageAction,flagAction,haveItem);
                _itemBoxList.Add(itemBox);
            });
        }
        public override void ShowCurrentDay()
        {
            //TODO
        }

        public override void NextDay()
        {
            // foreach (var itemBox in _itemBoxList)
            // {
            //     var itemBoxObject = itemBox;
            //     _itemBoxList.Remove(itemBox);
            //     Destroy(itemBoxObject);
            // }
        }


    }
