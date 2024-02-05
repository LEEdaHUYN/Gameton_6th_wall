using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dahyeon
{

    public class Inventory : MonoBehaviour
    {
        public List<Item> Items = new List<Item>();

        [SerializeField]
        private Transform slotParent;

        public Slot[] Slots;


        private void Awake()
        {
            Slots = slotParent.GetComponentsInChildren<Slot>();
        }
        public void AddItem(Item _item)
        {
            if (Items.Count < Slots.Length)
            {
                Items.Add(_item);
                int selectItemIndex = Items.Count - 1;
                Slots[selectItemIndex].ShowItemSlot(Items[selectItemIndex]);
            }
            else
            {
                print("½½·ÔÀÌ °¡µæ Â÷ ÀÖ½À´Ï´Ù.");
            }
        }

        internal void ClearSlot()
        {
       
            foreach (var slot in Slots)
            {
                if (slot.item == null)
                {
                    continue;
                }
                Managers.Game.AddItem(slot.item.GetName, 1);
                slot.ShowHandSlot();
            }
            Items.Clear();
        }
    }
}