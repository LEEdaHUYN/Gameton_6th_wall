using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dahyeon
{

    public class Inventory : MonoBehaviour
    {
        public List<NomalItem> Items;

        [SerializeField]
        private Transform slotParent;

        public Slot[] Slots;


        private void Awake()
        {
            Slots = slotParent.GetComponentsInChildren<Slot>();
        }
        public void AddItem(NomalItem _item)
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
            Items.Clear();
            foreach (var slot in Slots)
            {
                slot.ShowHandSlot();
            }
        }
    }
}